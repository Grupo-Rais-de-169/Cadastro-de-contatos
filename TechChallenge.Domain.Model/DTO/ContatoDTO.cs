namespace TechChallenge.Domain.Model
{
    public class ContatoDto
    {
        public int Id { get; set; }
        public string Nome { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string Telefone { get; set; } = null!;
        public int DDD {  get; set; }
    }
}
