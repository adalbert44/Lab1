using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using lab1;
using Microsoft.AspNetCore.Authorization;

namespace lab1.Controllers
{
    public class RestaurantLocationsController : Controller
    {
        private readonly FoodDelivery_v2Context _context;

        public RestaurantLocationsController(FoodDelivery_v2Context context)
        {
            _context = context;
        }

        // GET: RestaurantLocations
        public async Task<IActionResult> Index(int? id, string? name)
        {
            if (id == null) RedirectToAction("Rastaurants", "Index");
            // Get locations by restaurant
            ViewBag.RestaurantId = id;
            ViewBag.RestaurantName = name;
            var locationsByRestaurant = _context.RestaurantLocation.Where(b => b.RestaurantId == id).Include(b => b.Restaurant);

            return View(await locationsByRestaurant.ToListAsync());
        }

        // GET: RestaurantLocations/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var restaurantLocation = await _context.RestaurantLocation
                .Include(r => r.Restaurant)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (restaurantLocation == null)
            {
                return NotFound();
            }

            return View(restaurantLocation);
        }

        // GET: RestaurantLocations/Create
        [Authorize(Roles = "admin")]
        public IActionResult Create(int restaurantId)
        {
            ViewBag.RestaurantId = restaurantId;
            ViewBag.RestaurantName = _context.Restaurant.Where(c => c.Id == restaurantId).FirstOrDefault().Name;
            return View();
        }

        // POST: RestaurantLocations/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Create(int restaurantId, [Bind("Id,RestaurantId,Address,OpeningTime,ClosingTime")] RestaurantLocation restaurantLocation)
        {
            restaurantLocation.RestaurantId = restaurantId;
            if (ModelState.IsValid)
            {
                _context.Add(restaurantLocation);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "RestaurantLocations", new { id=restaurantId, name=_context.Restaurant.Where(c=>c.Id==restaurantId).FirstOrDefault().Name});
            }
            return View(restaurantLocation);
        }

        // GET: RestaurantLocations/Edit/5
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var restaurantLocation = await _context.RestaurantLocation.FindAsync(id);
            if (restaurantLocation == null)
            {
                return NotFound();
            }
            ViewData["RestaurantId"] = new SelectList(_context.Restaurant, "Id", "Name", restaurantLocation.RestaurantId);
            return View(restaurantLocation);
        }

        // POST: RestaurantLocations/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,RestaurantId,Address,OpeningTime,ClosingTime")] RestaurantLocation restaurantLocation)
        {
            if (id != restaurantLocation.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(restaurantLocation);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RestaurantLocationExists(restaurantLocation.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index", "RestaurantLocations", new { id = restaurantLocation.RestaurantId, name = _context.Restaurant.Where(c => c.Id == restaurantLocation.RestaurantId).FirstOrDefault().Name });
            }
            ViewData["RestaurantId"] = new SelectList(_context.Restaurant, "Id", "Name", restaurantLocation.RestaurantId);
            return View(restaurantLocation);
        }

        public IActionResult Back()
        {
            return RedirectToAction("Index", "Restaurants");
        }


        // GET: RestaurantLocations/Delete/5
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var restaurantLocation = await _context.RestaurantLocation
                .Include(r => r.Restaurant)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (restaurantLocation == null)
            {
                return NotFound();
            }

            return View(restaurantLocation);
        }

        // POST: RestaurantLocations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var restaurantLocation = await _context.RestaurantLocation.FindAsync(id);
            _context.RestaurantLocation.Remove(restaurantLocation);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "RestaurantLocations", new { id = restaurantLocation.RestaurantId, name = _context.Restaurant.Where(c => c.Id == restaurantLocation.RestaurantId).FirstOrDefault().Name });
        }

        private bool RestaurantLocationExists(int id)
        {
            return _context.RestaurantLocation.Any(e => e.Id == id);
        }
    }
}
