namespace QUickDish.API.DTOs
{
    public class OrderUpdateRequest
    {
        public int? CourierID { get; set; }
        public string? Status { get; set; }
    }
}
