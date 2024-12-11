using System.Text.Json.Serialization;
using TechChallenge.Domain.Entities;

namespace TechChallenge.Domain
{
    public class Usuario: EntityBase
    {
        public string Login { get; set; } = null!;
        public string Senha { get; set; } = null!;
        public int PermissaoId { get; set; }
        public virtual Permissao Permissao { get; set; } = null!;
    }
}
