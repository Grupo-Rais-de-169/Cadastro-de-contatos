using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace API.Contatos.Models
{
    public class AuthValidateModel
    {
        public AuthValidateModel()
        {

        }
        [Required]
        public string login { get; set; } = null!;
        [Required]
        public string password { get; set; } = null!;
        [JsonIgnore]
        public string? permissao { get; set; } = null!;


    }
}
