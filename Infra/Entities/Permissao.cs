﻿using System.Text.Json.Serialization;

namespace Infra.Entities
{
    public class Permissao
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("funcao")]
        public string Funcao { get; set; } = null!;
    }
}
