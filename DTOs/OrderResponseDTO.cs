namespace QUickDish.API.DTOs
{
    public class OrderResponseDTO
    {
        public int Id { get; set; }
        public string? UserName { get; set; }
        public string? CourierName { get; set; }
        public string Address { get; set; }
        public decimal TotalAmount { get; set; }
        public string Status { get; set; }

        public List<OrderItemDTO> Items { get; set; }
    }
}
