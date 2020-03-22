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
    public class TypesDiagramController : ControllerBase
    {
        private readonly FoodDelivery_v2Context _context;

        public TypesDiagramController(FoodDelivery_v2Context context)
        {
            _context = context;
        }

        [HttpGet("JsonData")]
        public JsonResult JsonData()
        {
            var types = _context.Type.Include(b => b.Dish).ToList();

            List<object> catDishes = new List<object>();
            catDishes.Add(new[] { "Тип", "Кількість страв" });

            foreach (var t in types)
            {
                catDishes.Add(new object[] { t.Name, t.Dish.Count() });
            }

            return new JsonResult(catDishes);
        }

    }
}