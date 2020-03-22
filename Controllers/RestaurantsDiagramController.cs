using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace lab1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RestaurantsDiagramController : ControllerBase
    {
        private readonly FoodDelivery_v2Context _context;

        public RestaurantsDiagramController(FoodDelivery_v2Context context)
        {
            _context = context;
        }

        [HttpGet("JsonData")]
        public JsonResult JsonData()
        {
            var restaurants = _context.Restaurant.Include(b => b.RestaurantLocation).ToList();

            List<object> catRestaurantLocations = new List<object>();
            catRestaurantLocations.Add(new[] { "Ресторан", "Кількість закладів" });

            foreach (var r in restaurants)
            {
                catRestaurantLocations.Add(new object[] { r.Name, r.RestaurantLocation.Count() });
            }

            return new JsonResult(catRestaurantLocations);
        }
    }
}