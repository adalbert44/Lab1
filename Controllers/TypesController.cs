using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using lab1;
using Microsoft.AspNetCore.Http;
using System.IO;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Authorization;

namespace lab1.Controllers
{
    public class TypesController : Controller
    {
        private readonly FoodDelivery_v2Context _context;

        public TypesController(FoodDelivery_v2Context context)
        {
            _context = context;
        }

        // GET: Types
        public async Task<IActionResult> Index()
        {
            return View(await _context.Type.ToListAsync());
        }

        // GET: Types/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @type = await _context.Type
                .FirstOrDefaultAsync(m => m.Id == id);
            if (@type == null)
            {
                return NotFound();
            }

            return RedirectToAction("Index", "Dishes", new {id = @type.Id, name = @type.Name });
        }

        // GET: Types/Create
        [Authorize(Roles = "admin")]
        public IActionResult Create()
        {
            return View();
        }

        public IActionResult Back()
        {
            return RedirectToAction("Index", "Types");
        }

        // POST: Types/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Create([Bind("Id,Name,Info")] Type @type)
        {
            if (ModelState.IsValid)
            {
                _context.Add(@type);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(@type);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Import(IFormFile fileExcel)
        {
            if(ModelState.IsValid)
            {
                if(fileExcel != null)
                {
                    using (var stream = new FileStream(fileExcel.FileName, FileMode.Create))
                    {
                        await fileExcel.CopyToAsync(stream);
                        using (XLWorkbook workBook = new XLWorkbook(stream, XLEventTracking.Disabled))
                        {
                            foreach (IXLWorksheet worksheet in workBook.Worksheets)
                            {
                                Type newcat;
                                var c = (from cat in _context.Type
                                         where cat.Name.Contains(worksheet.Name)
                                         select cat).ToList();
                                if (c.Count > 0)
                                {
                                    newcat = c[0];
                                }
                                else
                                {
                                    newcat = new Type();
                                    newcat.Name = worksheet.Name;
                                    newcat.Info = "from Excel";
                                    _context.Type.Add(newcat);
                                }
                                Console.WriteLine(newcat);
                                
                                foreach(IXLRow row in worksheet.RowsUsed().Skip(1))
                                {
                                    try
                                    {
                                        Dish dish = new Dish();
                                        dish.Name = row.Cell(1).Value.ToString();
                                        var restaurantName = row.Cell(2).Value.ToString();
                                        Restaurant restaurant;
                                        var restaurants = (from cat in _context.Restaurant
                                                           where cat.Name.Contains(restaurantName)
                                                           select cat).ToList();
                                        if (restaurants.Count > 0)
                                        {
                                            restaurant = restaurants[0];
                                        }
                                        else
                                        {
                                            restaurant = new Restaurant();
                                            restaurant.Name = restaurantName;
                                            _context.Restaurant.Add(restaurant);
                                        }
                                        dish.Restaurant = restaurant;
                                        dish.Recipe = row.Cell(3).Value.ToString();
                                        dish.Calories = Int32.Parse(row.Cell(4).Value.ToString());
                                        dish.Cost = Int32.Parse(row.Cell(5).Value.ToString());
                                        dish.Type = newcat;

                                        for (int i = 6; i <= 20; i++)
                                        {
                                            if(row.Cell(i).Value.ToString().Length > 0)
                                            {
                                                Ingredient ingredient;
                                                var ingredients = (from ing in _context.Ingredient
                                                         where ing.Name.Contains(row.Cell(i).Value.ToString())
                                                         select ing).ToList();
                                                if (ingredients.Count()>0)
                                                {
                                                    ingredient = ingredients[0];
                                                } else
                                                {
                                                    ingredient = new Ingredient();
                                                    ingredient.Name = row.Cell(i).Value.ToString();
                                                    ingredient.Info = "from Excell";
                                                    _context.Add(ingredient);
                                                }

                                                DishIngredient di = new DishIngredient();
                                                di.Dish = dish;
                                                di.Ingredient = ingredient;
                                                _context.DishIngredient.Add(di);
                                            }
                                        }
                                        _context.Dish.Add(dish);
                                    }
                                    catch (Exception e)
                                    {
                                        Console.WriteLine("Exception caught: {0}", e);
                                    }
                                }
                            }
                        }
                    }
                }
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        public ActionResult Export()
        {
            using (XLWorkbook workdish = new XLWorkbook(XLEventTracking.Disabled))
            {
                var types = _context.Type.Include("Dish").ToList();
                foreach (var t in types)
                {
                    var worksheet = workdish.Worksheets.Add(t.Name);
                    worksheet.Cell("A1").Value = "Назва";
                    worksheet.Cell("B1").Value = "Ресторан";
                    worksheet.Cell("C1").Value = "Рецепт";
                    worksheet.Cell("D1").Value = "Калорії";
                    worksheet.Cell("E1").Value = "Ціна";
                    worksheet.Cell("F1").Value = "Інгрідієнти";
                    worksheet.Row(1).Style.Font.Bold = true;
                    var dishes = t.Dish.ToList();

                    for (int i = 0; i < dishes.Count; i++)
                    {
                        worksheet.Cell(i + 2, 1).Value = dishes[i].Name;
                        var r = (from cat in _context.Restaurant
                                 where cat.Id == dishes[i].RestaurantId
                                 select cat).ToList();

                        worksheet.Cell(i + 2, 2).Value = dishes[i].Restaurant.Name;
                        worksheet.Cell(i + 2, 3).Value = r[0].Name;
                        worksheet.Cell(i + 2, 4).Value = dishes[i].Calories;
                        worksheet.Cell(i + 2, 5).Value = dishes[i].Cost;
                        var di = _context.DishIngredient.Where(a => a.DishId == dishes[i].Id).Include("Ingredient").ToList();

                        int j = 0;
                        foreach (var ing in di)
                        {
                            if (j + 6 < 16)
                                worksheet.Cell(i + 2, j + 6).Value = ing.Ingredient.Name;
                            j++;
                        }
                    }
                }

                using (var stream = new MemoryStream())
                {
                    workdish.SaveAs(stream);
                    stream.Flush();

                    return new FileContentResult(stream.ToArray(),
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                    {
                        FileDownloadName = $"menu_{ DateTime.UtcNow.ToShortDateString()}.xlsx"
                    };
                }
            }
        }

        // GET: Types/Edit/5
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @type = await _context.Type.FindAsync(id);
            if (@type == null)
            {
                return NotFound();
            }
            return View(@type);
        }

        // POST: Types/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Info")] Type @type)
        {
            if (id != @type.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(@type);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TypeExists(@type.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(@type);
        }

        // GET: Types/Delete/5
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @type = await _context.Type
                .FirstOrDefaultAsync(m => m.Id == id);
            if (@type == null)
            {
                return NotFound();
            }

            return View(@type);
        }

        // POST: Types/Delete/5
        [Authorize(Roles = "admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var @type = await _context.Type.FindAsync(id);

            var dishesByType = await  _context.Dish.Where(b => b.TypeId == id).Include(b => b.Type).ToListAsync();
            foreach (var dish in dishesByType)
            {
                var ingredientsByDish = await _context.DishIngredient.Where(b => b.DishId == dish.Id).ToListAsync();
                foreach (var dishIngredient in ingredientsByDish)
                {
                    _context.DishIngredient.Remove(dishIngredient);
                }

                _context.Dish.Remove(dish);
            }

            _context.Type.Remove(@type);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TypeExists(int id)
        {
            return _context.Type.Any(e => e.Id == id);
        }
    }
}
