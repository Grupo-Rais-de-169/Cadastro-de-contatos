using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace TechChallenge.Domain.Model.ViewModel
{
    public class ContatoAlteracaoViewModel
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Telefone { get; set; }
        public string Email { get; set; }
        [JsonPropertyName("DDD")]
        public int IdDDD { get; set; }
    }
}
