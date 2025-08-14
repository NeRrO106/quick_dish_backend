using System.ComponentModel.DataAnnotations.Schema;

namespace QUickDish.API.Models
{
    public class Order
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public int? CourierId { get; set; }

        public string Address { get; set; } = string.Empty;
        public int PhoneNumber { get; set; }
        public decimal TotalAmount { get; set; }
        public string Status { get; set; } = "Pending";

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public string Notes { get; set; } = string.Empty;
        public string PaymentMethod { get; set; } = string.Empty;

        [NotMapped]
        public string CourierName { get; set; } = string.Empty;

        [NotMapped]
        public string UserName { get; set; } = string.Empty;

        public ICollection<OrderItem> Items { get; set; } = new List<OrderItem>();
    }
}
