namespace TechChallenge.DAO.Worker.Entities
{
    public class CodigoDeArea : EntityBase
    {
        public string Regiao { get; set; } = null!;
        public virtual ICollection<Contato> Contatos { get; set; } = new List<Contato>();
    }
}
