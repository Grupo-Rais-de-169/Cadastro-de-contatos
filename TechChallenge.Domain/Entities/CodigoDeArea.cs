using System.Text.Json.Serialization;
using TechChallenge.Domain.Entities;

namespace TechChallenge.Domain
{
    public class CodigoDeArea: EntityBase
    {
        public string Regiao { get; set; } = null!;
        public virtual ICollection<Contato> Contatos { get; set; }
    }
}
