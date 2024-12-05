using System.Text.Json.Serialization;
using TechChallenge.Domain.Entities;

namespace TechChallenge.Domain
{
    public class Usuario: EntityBase
    {

        [JsonPropertyName("login")]
        public string Login { get; set; } = null!;

        [JsonPropertyName("senha")]
        public string Senha { get; set; } = null!;

        [JsonPropertyName("permissaoId")]
        public int PermissaoId { get; set; }
        public virtual Permissao Permissao { get; set; } = null!;
    }
}
