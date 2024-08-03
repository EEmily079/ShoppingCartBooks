﻿using Microsoft.Build.Framework;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookShoppingCartMvcUI.Models
{
    [Table("ShoppingCart")]
    public class ShoppingCart 
    {
        public int Id { get; set; }

        [Required]
        public int CartId { get; set; }
        [Required]
        public int UserId { get; set;}

        public bool IsDeleted { get; set; } = false;

    }
}
