﻿using TechChallenge.Domain.Interfaces.Services;

namespace TechChallenge.Domain.Services
{
    public class PasswordService: IPasswordService
    {
        public string GerarHash(string senha)
        {
            return BCrypt.Net.BCrypt.HashPassword(senha);
        }

        public bool VerificarSenha(string senhaDigitada, string hashSalvo)
        {
            return BCrypt.Net.BCrypt.Verify(senhaDigitada, hashSalvo);
        }
    }
}
