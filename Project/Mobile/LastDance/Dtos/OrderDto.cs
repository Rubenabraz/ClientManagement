namespace LastDance.DTOs
{
    public class OrderDto
    {
        public int ordId { get; set; }
        public int ordClientID { get; set; }
        public string ordName { get; set; } = null!;
        public string ordStatus { get; set; }
        public decimal ordTotalAmount { get; set; }
        public string? ordDescription { get; set; }
        public bool ordDelivered{get; set; }
        public DateTime ordCreatedDate { get; set; }
        public string ordCreatedUser { get; set; }
        public DateTime? ordUpdatedDate { get; set; }
        public string ordUpdatedUser { get; set; }


        // Propriedade "derivada"

        public string ClientName { get; set; } = string.Empty;

    }

    public class OrderUpdateDto : OrderDto
    {
        public string ordStatus { get; set; } = "em processamento";
        public bool ordDelivered { get; set; } = false;
    }

    public class OrderDeleteDto
    {
        public int ordID { get; set; }
        public string ordStatus { get; set; } = "removido";
    }
}
