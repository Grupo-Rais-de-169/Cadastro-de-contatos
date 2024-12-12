using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechChallenge.Domain.Services;
using Xunit;

namespace TechChallenge.Tests.Domain.Services
{
    public class PasswordServiceTests
    {
        private readonly PasswordService _passwordService;

        public PasswordServiceTests()
        {
            _passwordService = new PasswordService();
        }

        [Fact]
        public void GerarHash_ShouldGenerateValidHash()
        {
            // Arrange
            var senha = "Teste@123";

            // Act
            var hash = _passwordService.GerarHash(senha);

            // Assert
            Assert.NotNull(hash);
            Assert.NotEmpty(hash);
            Assert.NotEqual(senha, hash);
        }

        [Fact]
        public void VerificarSenha_ShouldReturnTrue_WhenPasswordMatchesHash()
        {
            // Arrange
            var senha = "Teste@123";
            var hash = _passwordService.GerarHash(senha);

            // Act
            var result = _passwordService.VerificarSenha(senha, hash);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void VerificarSenha_ShouldReturnFalse_WhenPasswordDoesNotMatchHash()
        {
            // Arrange
            var senha = "Teste@123";
            var senhaErrada = "Errado@123";
            var hash = _passwordService.GerarHash(senha);

            // Act
            var result = _passwordService.VerificarSenha(senhaErrada, hash);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void GerarHash_ShouldGenerateDifferentHashesForSamePassword()
        {
            // Arrange
            var senha = "Teste@123";

            // Act
            var hash1 = _passwordService.GerarHash(senha);
            var hash2 = _passwordService.GerarHash(senha);

            // Assert
            Assert.NotEqual(hash1, hash2); // Salt ensures different hashes for the same password
        }

        [Fact]
        public void VerificarSenha_ShouldReturnFalse_WhenHashIsTampered()
        {
            // Arrange
            var senha = "Teste@123";
            var hash = _passwordService.GerarHash(senha);
            var hashAlterado = hash.Substring(0, hash.Length - 1) + "0"; // Simula uma adulteração no hash

            // Act
            var result = _passwordService.VerificarSenha(senha, hashAlterado);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void VerificarSenha_ShouldThrowException_WhenHashIsInvalid()
        {
            // Arrange
            var senha = "Teste@123";
            var hashInvalido = "HashInvalido";

            // Act & Assert
            var exception = Record.Exception(() => _passwordService.VerificarSenha(senha, hashInvalido));

            Assert.NotNull(exception);
            Assert.IsType<BCrypt.Net.SaltParseException>(exception);
        }
    }
}