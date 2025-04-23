namespace TechChallenge.DAO.Api.Entities
{
    public class CodigoDeArea : EntityBase
    {
        public string Regiao { get; set; } = null!;
        public virtual ICollection<Contato> Contatos { get; set; } = new List<Contato>();
    }
}
