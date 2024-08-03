using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookShoppingCartMvcUI.Models
{

    [Table("Order")]
    public class Order
    {
        public int Id { get; set; }
        [Required]
        public  Guid UserId { get; set; }
        public DateTime CreateDate { get; set; } = DateTime.UtcNow;
        [Required]
        public int OrderStatus { get; set; }
        public bool IsDeleted { get; set; } = false;
        public OrderStatus orderStatusId{ get; set; }
        public List<OrderDetail> OrderDetails{ get; set; }

    }
}
