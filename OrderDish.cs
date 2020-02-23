using System;
using System.Collections.Generic;

namespace lab1
{
    public partial class OrderDish
    {
        public int Id { get; set; }
        public int DishId { get; set; }
        public int OrderId { get; set; }
        public int RestaurantLocationId { get; set; }

        public virtual Dish Dish { get; set; }
        public virtual Order Order { get; set; }
        public virtual RestaurantLocation RestaurantLocation { get; set; }
    }
}
