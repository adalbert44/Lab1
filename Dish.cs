using System;
using System.Collections.Generic;

namespace lab1
{
    public partial class Dish
    {
        public Dish()
        {
            DishIngredient = new HashSet<DishIngredient>();
            OrderDish = new HashSet<OrderDish>();
        }

        public int Id { get; set; }
        public int RestaurantId { get; set; }
        public string Name { get; set; }
        public int TypeId { get; set; }
        public string Recipe { get; set; }
        public int? Calories { get; set; }
        public int Cost { get; set; }

        public virtual Restaurant Restaurant { get; set; }
        public virtual Type Type { get; set; }
        public virtual ICollection<DishIngredient> DishIngredient { get; set; }
        public virtual ICollection<OrderDish> OrderDish { get; set; }
    }
}
