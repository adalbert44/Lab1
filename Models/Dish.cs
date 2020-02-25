using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace lab1
{
    public partial class Dish
    {
        public Dish()
        {
            DishIngredient = new HashSet<DishIngredient>();
            OrderDish = new HashSet<OrderDish>();
        }

        public int Id { get; set; }
        [Display(Name = "Ресторан")]
        public int RestaurantId { get; set; }
        [Required(ErrorMessage = "Поле не повинно бути порожнім")]
        [Display(Name = "Назва")]
        public string Name { get; set; }
        [Display(Name = "Тип")]
        public int TypeId { get; set; }
        [Display(Name = "Рецепт")]
        public string Recipe { get; set; }
        [Required(ErrorMessage = "Поле не повинно бути порожнім")]
        [Range(0, int.MaxValue, ErrorMessage = "Введіть валідне число")]
        [Display(Name = "Калорії")]
        public int? Calories { get; set; }
        [Required(ErrorMessage = "Поле не повинно бути порожнім")]
        [Range(0, int.MaxValue, ErrorMessage = "Введіть валідне число")]
        [Display(Name = "Ціна")]
        public int Cost { get; set; }

        [Display(Name = "Ресторан")]
        public virtual Restaurant Restaurant { get; set; }
        [Display(Name = "Тип")]
        public virtual Type Type { get; set; }
        public virtual ICollection<DishIngredient> DishIngredient { get; set; }
        public virtual ICollection<OrderDish> OrderDish { get; set; }
    }
}
