using System.Text.Json.Serialization;
using TechChallenge.Domain.Entities;

namespace TechChallenge.Domain
{
    public class Permissao: EntityBase
    {
        [JsonPropertyName("funcao")]
        public string Funcao { get; set; } = null!;
        [JsonIgnore]
        public virtual ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
    }
}
