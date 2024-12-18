﻿using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace TechChallenge.Domain.Model.ViewModel
{
    public class ContatoAlteracaoViewModel
    {
        [Required(ErrorMessage = "O Id é obrigatório.")]
        public int Id { get; set; }
        [Required(ErrorMessage = "O nome é obrigatório.")]
        public string Nome { get; set; } = null!;
        [Required(ErrorMessage = "O telefone é obrigatório.")]
        public string Telefone { get; set; } = null!;
        [Required(ErrorMessage = "O email é obrigatório.")]
        [EmailAddress(ErrorMessage = "O email informado não é válido.")]
        public string Email { get; set; } = null!;
        [JsonIgnore]
        public int IdDDD { get; set; }

        [Required(ErrorMessage = "O DDD é obrigatório.")]
        [RegularExpression(@"^0?\d{2}$", ErrorMessage = "O DDD deve ter 2 ou 3 caracteres e, opcionalmente, começar com 0.")]
        public string DDD
        {
            get => IdDDD.ToString("D2"); // Retorna sempre como string com no mínimo 2 dígitos
            set
            {
                if (int.TryParse(value, out var parsedDDD))
                {
                    IdDDD = parsedDDD;
                }
                else
                {
                    throw new ValidationException("O DDD deve ser numérico.");
                }
            }
        }
    }
}
