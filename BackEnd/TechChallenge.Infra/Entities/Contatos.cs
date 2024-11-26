using System.Text.Json.Serialization;

namespace TechChallenge.Infra.Entities
{
    public class Contatos
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
        
        [JsonPropertyName("nome")]
        public string Nome { get; set; } = null!;
        
        [JsonPropertyName("email")]
        public string Email { get; set; } = null!;
        
        [JsonPropertyName("telefone")] 
        public string Telefone { get; set; } = null!;
        
        [JsonPropertyName("ddd")] 
        public virtual CodigoDeArea Ddd { get; set; } = null!;
    }
}
