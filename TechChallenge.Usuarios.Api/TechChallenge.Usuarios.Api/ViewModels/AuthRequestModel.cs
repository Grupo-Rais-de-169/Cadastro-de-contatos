using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace TechChallenge.Usuarios.Api.ViewModels
{
    public class AuthRequestModel
    {
        public AuthRequestModel()
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
