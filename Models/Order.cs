using System;
using System.Collections.Generic;

namespace lab1
{
    public partial class Order
    {
        public Order()
        {
            OrderDish = new HashSet<OrderDish>();
        }

        public int Id { get; set; }
        public DateTime Time { get; set; }
        public int ClientId { get; set; }
        public int RestaurantLocationId { get; set; }

        public virtual Client Client { get; set; }
        public virtual RestaurantLocation RestaurantLocation { get; set; }
        public virtual ICollection<OrderDish> OrderDish { get; set; }
    }
}
