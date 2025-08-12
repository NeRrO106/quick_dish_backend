using System.ComponentModel.DataAnnotations.Schema;

namespace QUickDish.API.Models
{
    public class OrderItem
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice => UnitPrice * Quantity;

        [NotMapped]
        public string ProductName { get; set; } = string.Empty;

        [NotMapped]
        public string ProductDescription { get; set; } = string.Empty;

    }
}
