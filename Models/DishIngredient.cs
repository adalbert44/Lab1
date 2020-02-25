using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace lab1
{
    public partial class DishIngredient
    {
        public int Id { get; set; }
        [Display(Name = "Страва")]
        public int DishId { get; set; }
        [Display(Name = "Інгредієнт")]
        public int IngredientId { get; set; }

        [Display(Name = "Страва")]
        public virtual Dish Dish { get; set; }
        [Display(Name = "Інгедієнт")]
        public virtual Ingredient Ingredient { get; set; }
    }
}
