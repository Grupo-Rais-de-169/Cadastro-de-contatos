namespace TechChallenge.DAO.Api.Entities
{
    public class Contato : EntityBase
    {

        public string Nome { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string Telefone { get; set; } = null!;
        public int IdDDD { get; set; }

        public virtual CodigoDeArea Ddd { get; set; } = null!;
    }
}
