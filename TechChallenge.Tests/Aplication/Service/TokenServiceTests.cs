//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Security.Claims;
//using System.Text;
//using Microsoft.Extensions.Configuration;
//using Microsoft.IdentityModel.Tokens;
//using Moq;
//using TechChallenge.Api.Services;
//using TechChallenge.Domain;
//using TechChallenge.Domain.Interfaces.Repositories;
//using TechChallenge.Domain.Interfaces.Services;
//using Xunit;

//public class TokenServicesTests
//{
//    private readonly Mock<IAuthRepositories> _mockAuthRepositories;
//    private readonly Mock<IPasswordService> _mockPasswordService;
//    private readonly IConfiguration _mockConfig;
//    private readonly TokenServices _tokenServices;

//    public TokenServicesTests()
//    {
//        _mockAuthRepositories = new Mock<IAuthRepositories>();
//        _mockPasswordService = new Mock<IPasswordService>();

//        var inMemorySettings = new Dictionary<string, string>
//        {
//            {"Jwt:Key", "SuperSecretKeyForJwtToken"}
//        };
//        _mockConfig = new ConfigurationBuilder()
//            .AddInMemoryCollection(inMemorySettings)
//            .Build();

//        _tokenServices = new TokenServices(_mockConfig, _mockAuthRepositories.Object, _mockPasswordService.Object);
//    }

//    [Fact]
//    public void GenerateToken_ValidUser_ReturnsToken()
//    {
//        // Arrange
//        var user = new Usuario
//        {
//            Login = "testuser",
//            Permissao = new Permissao { Funcao = "admin" }
//        };

//        // Act
//        var token = _tokenServices.GenerateToken(user);

//        // Assert
//        Assert.False(string.IsNullOrEmpty(token));
//    }

//    [Fact]
//    public void GenerateRefreshToken_ReturnsNonEmptyString()
//    {
//        // Act
//        var refreshToken = _tokenServices.GenerateRefreshToken();

//        // Assert
//        Assert.False(string.IsNullOrEmpty(refreshToken));
//    }

//    [Fact]
//    public void GetPrincipalFromExpiredToken_ValidToken_ReturnsClaimsPrincipal()
//    {
//        // Arrange
//        var claims = new List<Claim>
//        {
//            new Claim(ClaimTypes.Name, "testuser")
//        };
//        var token = _tokenServices.GenerateToken(claims);

//        // Act
//        var principal = _tokenServices.GetPrincipalFromExpiredToken(token);

//        // Assert
//        Assert.NotNull(principal);
//        Assert.Equal("testuser", principal.Identity.Name);
//    }

//    [Fact]
//    public void SaveRefreshToken_StoresTokenCorrectly()
//    {
//        // Arrange
//        var username = "testuser";
//        var refreshToken = "sample-refresh-token";

//        // Act
//        _tokenServices.SaveRefreshToken(username, refreshToken);
//        var retrievedToken = _tokenServices.GetRefreshToken(username);

//        // Assert
//        Assert.Equal(refreshToken, retrievedToken);
//    }

//    [Fact]
//    public void DeleteRefreshToken_RemovesTokenCorrectly()
//    {
//        // Arrange
//        var username = "testuser";
//        var refreshToken = "sample-refresh-token";
//        _tokenServices.SaveRefreshToken(username, refreshToken);

//        // Act
//        _tokenServices.DeleteRefreshToken(username, refreshToken);
//        var retrievedToken = _tokenServices.GetRefreshToken(username);

//        // Assert
//        Assert.Null(retrievedToken);
//    }

//    [Fact]
//    public void GetUserByLoginAndPassword_ValidCredentials_ReturnsUser()
//    {
//        // Arrange
//        var login = "testuser";
//        var password = "password123";
//        var hashedPassword = "hashedPassword123";
//        var user = new Usuario { Login = login, Senha = hashedPassword };

//        _mockAuthRepositories
//            .Setup(repo => repo.GetConfirmLoginAndPassword(login))
//            .ReturnsAsync(user);

//        _mockPasswordService
//            .Setup(service => service.VerificarSenha(password, hashedPassword))
//            .Returns(true);

//        // Act
//        var retrievedUser = _tokenServices.GetUserByLoginAndPassword(login, password);

//        // Assert
//        Assert.NotNull(retrievedUser);
//        Assert.Equal(login, retrievedUser.Login);
//    }

//    [Fact]
//    public void GetUserByLoginAndPassword_InvalidPassword_ReturnsNull()
//    {
//        // Arrange
//        var login = "testuser";
//        var password = "wrongpassword";
//        var hashedPassword = "hashedPassword123";
//        var user = new Usuario { Login = login, Senha = hashedPassword };

//        _mockAuthRepositories
//            .Setup(repo => repo.GetConfirmLoginAndPassword(login))
//            .ReturnsAsync(user);

//        _mockPasswordService
//            .Setup(service => service.VerificarSenha(password, hashedPassword))
//            .Returns(false);

//        // Act
//        var retrievedUser = _tokenServices.GetUserByLoginAndPassword(login, password);

//        // Assert
//        Assert.Null(retrievedUser);
//    }
//}
