using System.Text.Json.Serialization;

namespace LastDanceAPI.Entities
{
    public class Clients
    {
        public int cltID { get; set; }
        public string cltName { get; set; } = string.Empty;
        public string cltSurname { get; set; } = string.Empty;
        public string cltFullName
        {
            get
            {
                return $"{cltName} {cltSurname}";
            }
        }
        public string cltEmail { get; set; } = string.Empty;
        public string cltPhoneNumber { get; set; } = string.Empty;
        public string cltGender { get; set; } = string.Empty;
        public bool cltActive { get; set; } = true;
        public string cltStatus { get; set; } = string.Empty;
        public string cltCreatedUser { get; set; } = string.Empty;
        public DateTime cltCreatedDate { get; set; } = DateTime.Now;
        public string cltUpdatedUser { get; set; } = string.Empty;
        public DateTime? cltUpdatedDate { get; set; }

        [JsonIgnore]
        public ICollection<Orders> Orders { get; set; } = new List<Orders>();
    }
}
