using System.Text.Json.Serialization;

namespace TechChallenge.Domain.Models 
{ 
    public class RefreshRequestModel
    {
        public string token { get; set; } = null!;
        public string refreshToken { get; set; } = null!;

        [JsonIgnore]
        public string? create { get; set; }

        [JsonIgnore]
        public string? validate { get; set; }
    }
}
