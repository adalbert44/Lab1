using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace lab1
{
    public partial class Ingredient
    {
        public Ingredient()
        {
            DishIngredient = new HashSet<DishIngredient>();
        }

        public int Id { get; set; }
        [Display(Name = "Назва")]
        public string Name { get; set; }
        [Display(Name = "Інформація")]
        public string Info { get; set; }

        public virtual ICollection<DishIngredient> DishIngredient { get; set; }
    }
}
