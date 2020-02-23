using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using lab1;

namespace lab1.Controllers
{
    public class DishesController : Controller
    {
        private readonly FoodDelivery_v2Context _context;

        public DishesController(FoodDelivery_v2Context context)
        {
            _context = context;
        }

        // GET: Dishes
        public async Task<IActionResult> Index(int? id, string? name)
        {
            if (id == null) RedirectToAction("Types", "Index");
            // Get dishes by type
            ViewBag.TypeId = id;
            ViewBag.TypeName = name;
            var dishesByType = _context.Dish.Where(b => b.TypeId == id).Include(b => b.Type);

            return View(await dishesByType.ToListAsync());
        }

        // GET: Dishes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dish = await _context.Dish
                .Include(d => d.Restaurant)
                .Include(d => d.Type)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (dish == null)
            {
                return NotFound();
            }

            return View(dish);
        }

        // GET: Dishes/Create
        public IActionResult Create(int typeId)
        {
            ViewData["RestaurantId"] = new SelectList(_context.Restaurant, "Id", "Name");
            ViewBag.TypeId = typeId;
            ViewBag.TypeName = _context.Type.Where(c => c.Id == typeId).FirstOrDefault().Name;
            return View();
        }

        // POST: Dishes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int typeId, [Bind("Id,RestaurantId,Name,TypeId,Recipe,Calories,Cost")] Dish dish)
        {
            dish.TypeId = typeId;
            if (ModelState.IsValid)
            {
                _context.Add(dish);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Dishes", new {id=typeId, name = _context.Type.Where(c => c.Id == typeId).FirstOrDefault().Name });
            }
            return RedirectToAction("Index", "Dishes", new { id = typeId, name = _context.Type.Where(c => c.Id == typeId).FirstOrDefault().Name });
        }

        // GET: Dishes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dish = await _context.Dish.FindAsync(id);
            if (dish == null)
            {
                return NotFound();
            }
            ViewData["RestaurantId"] = new SelectList(_context.Restaurant, "Id", "Name", dish.RestaurantId);
            ViewData["TypeId"] = new SelectList(_context.Type, "Id", "Name", dish.TypeId);
            return View(dish);
        }

        // POST: Dishes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,RestaurantId,Name,TypeId,Recipe,Calories,Cost")] Dish dish)
        {
            if (id != dish.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(dish);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DishExists(dish.Id))
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
            ViewData["RestaurantId"] = new SelectList(_context.Restaurant, "Id", "Name", dish.RestaurantId);
            ViewData["TypeId"] = new SelectList(_context.Type, "Id", "Name", dish.TypeId);
            return View(dish);
        }

        // GET: Dishes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dish = await _context.Dish
                .Include(d => d.Restaurant)
                .Include(d => d.Type)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (dish == null)
            {
                return NotFound();
            }

            return View(dish);
        }

        // POST: Dishes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var dish = await _context.Dish.FindAsync(id);
            _context.Dish.Remove(dish);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DishExists(int id)
        {
            return _context.Dish.Any(e => e.Id == id);
        }
    }
}
