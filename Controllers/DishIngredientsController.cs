﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using lab1;

namespace lab1.Controllers
{
    public class DishIngredientsController : Controller
    {
        private readonly FoodDelivery_v2Context _context;

        public DishIngredientsController(FoodDelivery_v2Context context)
        {
            _context = context;
        }

        // GET: DishIngredients
        public async Task<IActionResult> Index(int? id, string? name)
        {
            if (id == null) RedirectToAction("Types", "Index");
            // Get ingredients by dish
            ViewBag.DishId = id;
            ViewBag.DishName = name;
            var dishIngredientsByDish = _context.DishIngredient.Where(b => b.DishId == id).Include(b => b.Ingredient);

            return View(await dishIngredientsByDish.ToListAsync());
        }

        // GET: DishIngredients/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dishIngredient = await _context.DishIngredient
                .Include(d => d.Dish)
                .Include(d => d.Ingredient)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (dishIngredient == null)
            {
                return NotFound();
            }

            return View(dishIngredient);
        }

        // GET: DishIngredients/Create
        public IActionResult Create()
        {
            ViewData["DishId"] = new SelectList(_context.Dish, "Id", "Name");
            ViewData["IngredientId"] = new SelectList(_context.Ingredient, "Id", "Name");
            return View();
        }

        // POST: DishIngredients/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,DishId,IngredientId")] DishIngredient dishIngredient)
        {
            if (ModelState.IsValid)
            {
                _context.Add(dishIngredient);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["DishId"] = new SelectList(_context.Dish, "Id", "Name", dishIngredient.DishId);
            ViewData["IngredientId"] = new SelectList(_context.Ingredient, "Id", "Name", dishIngredient.IngredientId);
            return View(dishIngredient);
        }

        // GET: DishIngredients/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dishIngredient = await _context.DishIngredient.FindAsync(id);
            if (dishIngredient == null)
            {
                return NotFound();
            }
            ViewData["DishId"] = new SelectList(_context.Dish, "Id", "Name", dishIngredient.DishId);
            ViewData["IngredientId"] = new SelectList(_context.Ingredient, "Id", "Name", dishIngredient.IngredientId);
            return View(dishIngredient);
        }

        // POST: DishIngredients/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,DishId,IngredientId")] DishIngredient dishIngredient)
        {
            if (id != dishIngredient.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(dishIngredient);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DishIngredientExists(dishIngredient.Id))
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
            ViewData["DishId"] = new SelectList(_context.Dish, "Id", "Name", dishIngredient.DishId);
            ViewData["IngredientId"] = new SelectList(_context.Ingredient, "Id", "Name", dishIngredient.IngredientId);
            return View(dishIngredient);
        }

        // GET: DishIngredients/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dishIngredient = await _context.DishIngredient
                .Include(d => d.Dish)
                .Include(d => d.Ingredient)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (dishIngredient == null)
            {
                return NotFound();
            }

            return View(dishIngredient);
        }

        // POST: DishIngredients/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var dishIngredient = await _context.DishIngredient.FindAsync(id);
            _context.DishIngredient.Remove(dishIngredient);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DishIngredientExists(int id)
        {
            return _context.DishIngredient.Any(e => e.Id == id);
        }
    }
}
