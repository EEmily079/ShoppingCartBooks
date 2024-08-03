using Microsoft.Build.Framework;
using NuGet.Protocol.Core.Types;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookShoppingCartMvcUI.Models
{

    [Table("OrderDetail")]
    public class OrderDetail
    {
        public int Id { get; set; }
        [Required]
        public  int OrderId  { get; set; }
        [Required]
        public  int BookId  { get; set; }
        [Required]
        public int Quantity { get; set; }
        public double UniquePrice { get; set; }
        public Order Order { get; set; }
        public  Book Book { get; set; }


    }
}
