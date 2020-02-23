using System;
using System.Collections.Generic;

namespace lab1
{
    public partial class Type
    {
        public Type()
        {
            Dish = new HashSet<Dish>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Info { get; set; }

        public virtual ICollection<Dish> Dish { get; set; }
    }
}
