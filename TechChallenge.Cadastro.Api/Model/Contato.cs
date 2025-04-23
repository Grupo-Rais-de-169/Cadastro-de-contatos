namespace TechChallenge.Cadastro.Api.Model
{
    public class Contato
    {
        public int Id { get; set; }
        public string Nome { get; set; } = null!;
        
        public string Email { get; set; } = null!;
        
        public string Telefone { get; set; } = null!;
        public int IdDDD { get; set; }

        public virtual CodigoDeArea Ddd { get; set; } = null!;
    }
}
