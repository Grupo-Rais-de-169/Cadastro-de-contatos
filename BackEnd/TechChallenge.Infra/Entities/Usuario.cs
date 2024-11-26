using System.Text.Json.Serialization;

namespace TechChallenge.Infra.Entities
{
    public class Usuario
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("login")]
        public string Login { get; set; } = null!;

        [JsonPropertyName("senha")]
        public string Senha { get; set; } = null!;

        [JsonPropertyName("permissaoId")]
        public int PermissaoId { get; set; }
        public virtual Permissao Permissao { get; set; } = null!;
    }
}
