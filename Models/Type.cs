using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace lab1
{
    public partial class Type
    {
        public Type()
        {
            Dish = new HashSet<Dish>();
        }

        public int Id { get; set; }
        [Required(ErrorMessage = "Поле не повинно бути порожнім")]
        [Display(Name="Тип")]
        public string Name { get; set; }
        [Display(Name="Інформація про тип")]
        public string Info { get; set; }

        public virtual ICollection<Dish> Dish { get; set; }
    }
}
