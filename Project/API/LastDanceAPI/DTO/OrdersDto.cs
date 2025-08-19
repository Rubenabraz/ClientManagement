namespace LastDanceAPI.DTO
{
    public class OrdersDto
    {
        public int ordClientID { get; set; }
        public string ordName { get; set; } = null!;
        public decimal ordTotalAmount { get; set; }
        public string? ordDescription { get; set; }
    }

    public class OrdersUpdateDto : OrdersDto
    {
        public string ordStatus { get; set; } = "em processamento";
        public bool ordDelivered { get; set; } = false;
    }

    public class OrdersDeleteDto
    {
        public int ordID { get; set; }
        public string ordStatus { get; set; } = "removido";
    }
}
