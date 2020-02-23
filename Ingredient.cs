using System;
using System.Collections.Generic;

namespace lab1
{
    public partial class Ingredient
    {
        public Ingredient()
        {
            DishIngredient = new HashSet<DishIngredient>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Info { get; set; }

        public virtual ICollection<DishIngredient> DishIngredient { get; set; }
    }
}
