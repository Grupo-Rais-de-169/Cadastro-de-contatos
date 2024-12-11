using System.ComponentModel.DataAnnotations;

namespace TechChallenge.Domain.Model.ViewModel
{
    public class UsuarioAlteracaoViewModel
    {
        [Required(ErrorMessage = "O Id é obrigatório.")]
        public int Id { get; set; }
        
        [Required(ErrorMessage = "O Login é obrigatório.")]
        public string Login { get; set; } = null!;
        
        [Required(ErrorMessage = "A senha é obrigatória.")]
        [RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,10}$",
        ErrorMessage = "A senha deve ter entre 8 e 10 caracteres, incluindo uma letra, um número e um caractere especial.")]
        public string Senha { get; set; } = null!;
        
        [Required(ErrorMessage = "O PermissaoId é obrigatório.")]
        public int PermissaoId { get; set; }
    }
}
