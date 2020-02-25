using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace lab1
{
    public partial class RestaurantLocation
    {
        public RestaurantLocation()
        {
            OrderDish = new HashSet<OrderDish>();
        }

        public int Id { get; set; }
        [Display(Name = "Ресторан")]
        public int RestaurantId { get; set; }
        [Required(ErrorMessage = "Поле не повинно бути порожнім")]
        [Display(Name = "Адреса")]
        public string Address { get; set; }
        [Required(ErrorMessage = "Поле не повинно бути порожнім")]
        [Display(Name = "Час відкриття")]
        public TimeSpan OpeningTime { get; set; }
        [Required(ErrorMessage = "Поле не повинно бути порожнім")]
        [Display(Name = "Час закриття")]
        public TimeSpan ClosingTime { get; set; }

        public virtual Restaurant Restaurant { get; set; }
        public virtual ICollection<OrderDish> OrderDish { get; set; }
    }
}
