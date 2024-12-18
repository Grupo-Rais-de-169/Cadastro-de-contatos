﻿using System.Security.Claims;
using Microsoft.Extensions.Configuration;
using Moq;
using TechChallenge.Api.Services;
using TechChallenge.Domain;
using TechChallenge.Domain.Interfaces.Repositories;
using TechChallenge.Domain.Interfaces.Services;

namespace TechChallenge.Tests.Aplication.Service
{
    public class TokenServicesTests
    {
        private readonly Mock<IAuthRepositories> _mockAuthRepositories;
        private readonly Mock<IPasswordService> _mockPasswordService;
        private readonly IConfiguration _mockConfig;
        private readonly TokenServices _tokenServices;
        private const string _secureKey = "YourSuperSecureTokenKeyThatIsAtLeast32Chars!";

        public TokenServicesTests()
        {
            _mockAuthRepositories = new Mock<IAuthRepositories>();
            _mockPasswordService = new Mock<IPasswordService>();

            var inMemorySettings = new Dictionary<string, string>
        {
            {"Jwt:Key", "SuperSecretKeyForJwtToken"}
        };
            _mockConfig = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();

            _tokenServices = new TokenServices(_mockConfig, _mockAuthRepositories.Object, _mockPasswordService.Object);
        }

        [Fact]
        public void GenerateToken_ValidUser_ReturnsToken()
        {
            // Arrange
            var user = new Usuario
            {
                Login = "testuser",
                Permissao = new Permissao { Funcao = "admin" },
                Id = 1,
                PermissaoId = 1,
                Senha = "senhaTeste123456789senhaTeste123456789senhaTeste123456789senhaTeste123456789"
            };

            // Ajuste a chave diretamente no serviço antes do teste
            typeof(TokenServices)
                .GetField("_token", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?
                .SetValue(_tokenServices, _secureKey);

            // Act
            var token = _tokenServices.GenerateToken(user);

            // Assert
            Assert.False(string.IsNullOrEmpty(token));
        }

        [Fact]
        public void GenerateRefreshToken_ReturnsNonEmptyString()
        {
            // Act
            var refreshToken = _tokenServices.GenerateRefreshToken();

            // Assert
            Assert.False(string.IsNullOrEmpty(refreshToken));
        }

        [Fact]
        public void GetPrincipalFromExpiredToken_ValidToken_ReturnsClaimsPrincipal()
        {
            // Arrange
            typeof(TokenServices)
                .GetField("_token", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?
                .SetValue(_tokenServices, _secureKey);

            var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, "testuser")
         };

            var token = _tokenServices.GenerateToken(claims);

            // Act
            var principal = _tokenServices.GetPrincipalFromExpiredToken(token);

            // Assert
            Assert.NotNull(principal);
            Assert.Equal("testuser", principal.Identity.Name);
        }

        [Fact]
        public void SaveRefreshToken_StoresTokenCorrectly()
        {
            // Arrange
            var username = "testuser";
            var refreshToken = "sample-refresh-token";

            // Act
            _tokenServices.SaveRefreshToken(username, refreshToken);
            var retrievedToken = _tokenServices.GetRefreshToken(username);

            // Assert
            Assert.Equal(refreshToken, retrievedToken);
        }

        [Fact]
        public void DeleteRefreshToken_RemovesTokenCorrectly()
        {
            // Arrange
            var username = "testuser";
            var refreshToken = "sample-refresh-token";
            _tokenServices.SaveRefreshToken(username, refreshToken);

            // Act
            _tokenServices.DeleteRefreshToken(username, refreshToken);
            var retrievedToken = _tokenServices.GetRefreshToken(username);

            // Assert
            Assert.Null(retrievedToken);
        }

        [Fact]
        public void GetUserByLoginAndPassword_ValidCredentials_ReturnsUser()
        {
            // Arrange
            var login = "testuser";
            var password = "password123";
            var hashedPassword = "hashedPassword123";
            var user = new Usuario { Login = login, Senha = hashedPassword };

            _mockAuthRepositories
                .Setup(repo => repo.GetConfirmLoginAndPassword(login))
                .ReturnsAsync(user);

            _mockPasswordService
                .Setup(service => service.VerificarSenha(password, hashedPassword))
                .Returns(true);

            // Act
            var retrievedUser = _tokenServices.GetUserByLoginAndPassword(login, password);

            // Assert
            Assert.NotNull(retrievedUser);
            Assert.Equal(login, retrievedUser.Login);
        }

        [Fact]
        public void GetUserByLoginAndPassword_InvalidPassword_ReturnsNull()
        {
            // Arrange
            var login = "testuser";
            var password = "wrongpassword";
            var hashedPassword = "hashedPassword123";
            var user = new Usuario { Login = login, Senha = hashedPassword };

            _mockAuthRepositories
                .Setup(repo => repo.GetConfirmLoginAndPassword(login))
                .ReturnsAsync(user);

            _mockPasswordService
                .Setup(service => service.VerificarSenha(password, hashedPassword))
                .Returns(false);

            // Act
            var retrievedUser = _tokenServices.GetUserByLoginAndPassword(login, password);

            // Assert
            Assert.Null(retrievedUser);
        }
    }
}
