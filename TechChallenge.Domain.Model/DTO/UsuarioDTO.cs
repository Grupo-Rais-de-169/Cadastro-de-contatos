namespace TechChallenge.Domain.Model.DTO
{
    public class UsuarioDTO
    {
        public int Id { get; set; }
        public string Login { get; set; } = null!;
        public string Senha { get; set; } = null!;
        public int PermissaoId { get; set; }
    }
}
