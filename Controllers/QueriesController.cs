using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace lab1.Controllers
{
    public class QueriesController : Controller
    {
        private readonly FoodDelivery_v2Context _context;

        public QueriesController(FoodDelivery_v2Context context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
    
        public async Task<IActionResult> Execute([Bind("Id,Parameter,StringParameter")] Query query)
        {
            string q = "";
            IEnumerable<string> res;
            if (query.Id == 1)
            {
                q = String.Format("SELECT * FROM DISH WHERE ID IN(SELECT DISHID FROM(DISHINGREDIENT INNER JOIN DISH ON DISHID = DISH.ID) INNER JOIN INGREDIENT ON INGREDIENTID = INGREDIENT.ID WHERE(INGREDIENT.Name = '{0}'))", query.StringParameter);
                return View(await _context.Dish.FromSqlRaw(q).Select(x => x.Name).ToListAsync());
            }
            else if (query.Id == 2)
            {
                q = String.Format("SELECT * FROM restaurant WHERE restaurant.ID IN (SELECT restaurantId FROM dish WHERE cost > '{0}')",query.Parameter);
                return View(await _context.Restaurant.FromSqlRaw(q).Select(x => x.Name).ToListAsync());
            } else if (query.Id == 3)
            {
                q = String.Format("SELECT * FROM type WHERE type.ID NOT IN (SELECT typeID FROM dish INNER JOIN Restaurant ON dish.RestaurantId = Restaurant.Id WHERE Restaurant.Name='{0}')", query.StringParameter);
                return View(await _context.Type.FromSqlRaw(q).Select(x => x.Name).ToListAsync());
            }
            else if (query.Id == 4)
            {
                q = String.Format("SELECT * FROM type WHERE type.ID IN (SELECT typeID FROM dish INNER JOIN Restaurant ON dish.RestaurantId = Restaurant.Id WHERE Restaurant.Name!='{0}')", query.StringParameter);
                return View(await _context.Type.FromSqlRaw(q).Select(x => x.Name).ToListAsync());
            }
            else if (query.Id == 5)
            {
                q = String.Format("SELECT * FROM dish WHERE dish.ID IN (SELECT dish.ID FROM (dish INNER JOIN RestaurantLocation ON RestaurantLocation.RestaurantId = dish.RestaurantId) WHERE RestaurantLocation.Address = '{0}')", query.StringParameter);
                return View(await _context.Dish.FromSqlRaw(q).Select(x => x.Cost).Select(x => x.ToString()).ToListAsync());
            } else if (query.Id == 6)
            {
                q = String.Format("SELECT * FROM DISH as selected WHERE NOT EXISTS (SELECT * FROM DishIngredient INNER JOIN DISH ON DishIngredient.DishID = Dish.ID WHERE DISH.NAME = '{0}' AND DishIngredient.IngredientId NOT IN (SELECT IngredientId FROM DishIngredient WHERE DishIngredient.DishId = selected.Id))", query.StringParameter);
                return View(await _context.Dish.FromSqlRaw(q).Select(x => x.Name).ToListAsync());
            } else if (query.Id == 7)
            {
                q = String.Format("SELECT * FROM DISH as selected WHERE NOT EXISTS (SELECT * FROM DishIngredient as d1 INNER JOIN DishIngredient as d2 ON d1.IngredientId = d2.IngredientId WHERE (selected.Id = d1.DishId AND EXISTS (SELECT * FROM Dish WHERE Dish.Id = d2.DishId AND Dish.Name = '{0}' )))", query.StringParameter);
                return View(await _context.Dish.FromSqlRaw(q).Select(x => x.Name).ToListAsync());
            } else if (query.Id == 8)
            {
                q = String.Format("SELECT * FROM DISH as selected WHERE NOT EXISTS (SELECT * FROM DishIngredient WHERE DishId=selected.ID AND DishIngredient.IngredientId NOT IN (SELECT IngredientID FROM DishIngredient WHERE DishId IN (SELECT ID FROM DISH WHERE DISH.name = '{0}')))", query.StringParameter);
                return View(await _context.Dish.FromSqlRaw(q).Select(x => x.Name).ToListAsync());
            }
            else
            {
                return View();
            }
        }
    }
}