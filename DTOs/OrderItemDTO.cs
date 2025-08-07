namespace QUickDish.API.DTOs
{
    public class OrderItemDTO
    {
        public string? ProductName { get; set; }
        public string? ProductDescription { get; set; }

        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
