using System;
using System.Collections.Generic;

namespace lab1
{
    public partial class RestaurantLocation
    {
        public RestaurantLocation()
        {
            Order = new HashSet<Order>();
        }

        public int Id { get; set; }
        public int RestaurantId { get; set; }
        public string Address { get; set; }
        public TimeSpan OpeningTime { get; set; }
        public TimeSpan ClosingTime { get; set; }

        public virtual Restaurant Restaurant { get; set; }
        public virtual ICollection<Order> Order { get; set; }
    }
}
