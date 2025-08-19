using System.Text.Json.Serialization;

namespace LastDanceAPI.Entities
{
    public class Orders
    {
        public int ordID { get; set; }
        public int ordClientID { get; set; }
        public string ordName { get; set; } = string.Empty;
        public string ordStatus { get; set; } = string.Empty;
        public bool ordIsDeleted { get; set; } = false;
        public decimal ordTotalAmount { get; set; } = 0;
        public string ordDescription { get; set; } = string.Empty;
        public bool ordDelivered { get; set; } = false;
        public string ordCreatedUser { get; set; } = string.Empty;
        public DateTime ordCreatedDate { get; set; } = DateTime.Now;
        public string ordUpdatedUser { get; set; } = string.Empty;
        public DateTime? ordUpdatedDate { get; set; }

        [JsonIgnore]
        public Clients? Client { get; set; } = null;
    }
}
