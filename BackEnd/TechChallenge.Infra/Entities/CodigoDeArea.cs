using System.Text.Json.Serialization;

namespace TechChallenge.Infra.Entities
{
    public class CodigoDeArea
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("ddd")]
        public int Ddd { get; set; }

        [JsonPropertyName("uf")]
        public string Uf { get; set; } = null!;
    }
}
