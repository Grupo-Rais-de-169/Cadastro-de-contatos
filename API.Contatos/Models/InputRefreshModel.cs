using System.Text.Json.Serialization;

namespace API.Contatos.Models
{
    public class InputRefreshModel
    {
        public string token { get; set; } = null!;
        public string refreshToken { get; set; } = null!;

        [JsonIgnore]
        public string? create { get; set; }

        [JsonIgnore]
        public string? validate { get; set; }
    }
}
