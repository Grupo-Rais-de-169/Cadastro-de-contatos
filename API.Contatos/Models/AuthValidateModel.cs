using System.ComponentModel.DataAnnotations;

namespace API.Contatos.Models
{
    public class AuthValidateModel
    {
        public AuthValidateModel()
        {

        }
        public AuthValidateModel(string login, string senha)
        {

        }

        [Required]
        public string login { get; set; } = null!;
        [Required]
        public string senha { get; set; } = null!;


    }
}
