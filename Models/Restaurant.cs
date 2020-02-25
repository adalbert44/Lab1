using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace lab1
{
    public partial class Restaurant
    {
        public Restaurant()
        {
            Dish = new HashSet<Dish>();
            RestaurantLocation = new HashSet<RestaurantLocation>();
        }

        public int Id { get; set; }
        [Required(ErrorMessage = "Поле не повинно бути порожнім")]
        [Display(Name = "Назва")]
        public string Name { get; set; }

        public virtual ICollection<Dish> Dish { get; set; }
        public virtual ICollection<RestaurantLocation> RestaurantLocation { get; set; }
    }
}
