using System.Text.Json.Serialization;

namespace TechChallenge.DAO.Api.Entities
{
    public class Permissao: EntityBase
    {
        [JsonPropertyName("funcao")]
        public string Funcao { get; set; } = null!;
        [JsonIgnore]
        public virtual ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
    }
}
