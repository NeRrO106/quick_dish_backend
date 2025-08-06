namespace QUickDish.API.Models
{
    public class Order
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int CourierId { get; set; }
        public string Address { get; set; }
        public int PhoneNumber { get; set; }
        public string Email { get; set; }
        public decimal TotalAmount { get; set; }
        public string Status { get; set; } = "Pending";

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public string Notes { get; set; }
        public string PaymentMethod { get; set; }

        public ICollection<OrderItem> Items { get; set; } = new List<OrderItem>();
    }
}
