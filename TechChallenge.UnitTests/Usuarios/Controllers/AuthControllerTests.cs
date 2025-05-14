using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Moq;
using System.Dynamic;
using System.Security.Claims;
using System.Text.Json;
using TechChallenge.Usuarios.Api.Controllers;
using TechChallenge.Usuarios.Api.Domain.Entities;
using TechChallenge.Usuarios.Api.Domain.Interfaces;
using TechChallenge.Usuarios.Api.ViewModels;

namespace TechChallenge.UnitTests.Usuarios.Controllers
{
    public class AuthControllerTests
    {
        private readonly Mock<ITokenServices> _mockTokenServices;
        private readonly Mock<ILogger<AuthController>> _mockLogger;
        private readonly Mock<IConfiguration> _mockConfiguration;
        private readonly AuthController _controller;

        public AuthControllerTests()
        {
            _mockTokenServices = new Mock<ITokenServices>();
            _mockLogger = new Mock<ILogger<AuthController>>();
            _mockConfiguration = new Mock<IConfiguration>();
            _mockConfiguration.Setup(config => config["Jwt:Key"]).Returns("FakeJwtKey");
            _mockConfiguration.Setup(config => config["Jwt:Issuer"]).Returns("FakeIssuer");
            _controller = new AuthController(_mockLogger.Object, _mockTokenServices.Object);
        }

        [Fact]
        public void Login_ShouldReturnBadRequest_WhenAuthRequestIsNull()
        {
            // Arrange
            AuthRequestModel? authRequest = null;

            // Act
            var result = _controller.Login(authRequest);

            // Assert
            Assert.IsType<BadRequestResult>(result.Result);
        }

        [Fact]
        public void Login_ShouldReturnUnauthorized_WhenUserNotFound()
        {
            // Arrange
            var authRequest = new AuthRequestModel { login = "user", password = "wrongpassword" };
            _mockTokenServices.Setup(s => s.GetUserByLoginAndPassword(authRequest.login, authRequest.password))
                .Returns((Usuario?)null);

            // Act
            var result = _controller.Login(authRequest);

            // Assert
            Assert.IsType<UnauthorizedResult>(result.Result);

            _mockLogger.Verify(x => x.Log(LogLevel.Error, It.IsAny<EventId>(), It.Is<It.IsAnyType>((v, t) =>
            (v.ToString() ?? string.Empty).Contains("Não autorizado")),
            null, It.IsAny<Func<It.IsAnyType, Exception?, string>>()), Times.Once);
        }

        [Fact]
        public void Login_ShouldReturnOk_WhenUserIsValid()
        {
            // Arrange
            var authRequest = new AuthRequestModel { login = "user", password = "password", permissao = "admin" };
            var user = new Usuario { Login = "user", Permissao = new Permissao() { Funcao = "admin" } };

            _mockTokenServices.Setup(s => s.GetUserByLoginAndPassword(authRequest.login, authRequest.password))
                .Returns(user);
            _mockTokenServices.Setup(s => s.GenerateToken(user)).Returns("fake-jwt-token");
            _mockTokenServices.Setup(s => s.GenerateRefreshToken()).Returns("fake-refresh-token");

            // Act
            var actionResult = _controller.Login(authRequest);
            var result = actionResult.Result as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);

            string json = JsonSerializer.Serialize(result.Value);

            dynamic? response = JsonSerializer.Deserialize<ExpandoObject>(json);

            var permissaoObject = (JsonElement)response?.Permissao;
            var funcaoValue = permissaoObject.GetProperty("funcao").GetString();

            // Assert
            Assert.NotNull(response);
            Assert.Equal("user", response?.Login.GetString());
            Assert.Equal("admin", funcaoValue);
            Assert.Equal("fake-jwt-token", response?.token.GetString());
            Assert.Equal("fake-refresh-token", response?.refreshToken.GetString());
        }

        [Fact]
        public void Login_ShouldReturnBadRequest_OnException()
        {
            // Arrange
            var authRequest = new AuthRequestModel { login = "user", password = "password" };
            _mockTokenServices.Setup(s => s.GetUserByLoginAndPassword(It.IsAny<string>(), It.IsAny<string>()))
                .Throws(new Exception("Simulated exception"));

            // Act
            var result = _controller.Login(authRequest);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result.Result);
            _mockLogger.Verify(x =>
            x.Log(LogLevel.Error, It.IsAny<EventId>(),
            It.Is<It.IsAnyType>((v, t) =>
            (v.ToString() ?? string.Empty).Contains("Erro ao efetuar o Login: Simulated exception")),
            null,
            It.IsAny<Func<It.IsAnyType, Exception?, string>>()), Times.Once);
        }

        [Fact]
        public void Refresh_ValidTokens_ReturnsNewTokens()
        {
            // Arrange
            var input = new RefreshRequestModel
            {
                token = "expiredToken",
                refreshToken = "validRefreshToken",
                create = "teste",
                validate = "teste"
            };

            var claims = new List<Claim> { new Claim(ClaimTypes.Name, "testuser") };
            var principal = new ClaimsPrincipal(new ClaimsIdentity(claims));

            _mockTokenServices.Setup(s => s.GetPrincipalFromExpiredToken(input.token)).Returns(principal);
            _mockTokenServices.Setup(s => s.GetRefreshToken("testuser")).Returns("validRefreshToken");
            _mockTokenServices.Setup(s => s.GenerateToken(It.IsAny<IEnumerable<Claim>>())).Returns("newJwtToken");
            _mockTokenServices.Setup(s => s.GenerateRefreshToken()).Returns("newRefreshToken");

            // Act
            var actionResult = _controller.Refresh(input);

            var result = actionResult.Value;

            var json = JsonSerializer.Serialize(result);
            dynamic response = JsonSerializer.Deserialize<ExpandoObject>(json);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("newJwtToken", response.token.GetString());
            Assert.Equal("newRefreshToken", response.refreshToken.GetString());
            _mockTokenServices.Verify(s => s.DeleteRefreshToken("testuser", "validRefreshToken"), Times.Once);
            _mockTokenServices.Verify(s => s.SaveRefreshToken("testuser", "newRefreshToken"), Times.Once);
        }

        [Fact]
        public void Refresh_InvalidRefreshToken_ReturnsBadRequest()
        {
            // Arrange
            var input = new RefreshRequestModel
            {
                token = "expiredToken",
                refreshToken = "invalidRefreshToken"
            };

            var claims = new List<Claim> { new Claim(ClaimTypes.Name, "testuser") };
            var principal = new ClaimsPrincipal(new ClaimsIdentity(claims));

            _mockTokenServices.Setup(s => s.GetPrincipalFromExpiredToken(input.token)).Returns(principal);
            _mockTokenServices.Setup(s => s.GetRefreshToken("testuser")).Returns("validRefreshToken");

            // Act
            var result = _controller.Refresh(input).Result as BadRequestObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(400, result.StatusCode);
            Assert.Equal("Inválido Refresh", result.Value);

            _mockLogger.Verify(x =>
            x.Log(LogLevel.Error, It.IsAny<EventId>(),
            It.Is<It.IsAnyType>((v, t) =>
            (v.ToString() ?? string.Empty).Contains("Erro ao efetuar o Refresh: Inválido Refresh")),
            null,
            It.IsAny<Func<It.IsAnyType, Exception?, string>>()), Times.Once);
        }

        [Fact]
        public void Refresh_InvalidExpiredToken_ReturnsBadRequest()
        {
            // Arrange
            var input = new RefreshRequestModel
            {
                token = "invalidExpiredToken",
                refreshToken = "validRefreshToken"
            };

            _mockTokenServices.Setup(s => s.GetPrincipalFromExpiredToken(input.token))
                              .Throws(new SecurityTokenException("Invalid token"));

            // Act
            var result = _controller.Refresh(input).Result as BadRequestObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(400, result.StatusCode);
            Assert.Equal("Invalid token", result.Value);

            _mockLogger.Verify(x =>
            x.Log(LogLevel.Error, It.IsAny<EventId>(),
            It.Is<It.IsAnyType>((v, t) =>
            (v.ToString() ?? string.Empty).Contains("Erro ao efetuar o Refresh")),
            null,
            It.IsAny<Func<It.IsAnyType, Exception?, string>>()), Times.Once);

        }

        [Fact]
        public void Refresh_TokenServiceThrowsException_ReturnsBadRequest()
        {
            // Arrange
            var input = new RefreshRequestModel
            {
                token = "expiredToken",
                refreshToken = "validRefreshToken"
            };

            var claims = new List<Claim> { new Claim(ClaimTypes.Name, "testuser") };
            var principal = new ClaimsPrincipal(new ClaimsIdentity(claims));

            _mockTokenServices.Setup(s => s.GetPrincipalFromExpiredToken(input.token)).Returns(principal);
            _mockTokenServices.Setup(s => s.GetRefreshToken("testuser")).Throws(new Exception("Unexpected error"));

            // Act
            var result = _controller.Refresh(input).Result as BadRequestObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(400, result.StatusCode);
            Assert.Equal("Unexpected error", result.Value);

            _mockLogger.Verify(x =>
            x.Log(LogLevel.Error, It.IsAny<EventId>(),
            It.Is<It.IsAnyType>((v, t) =>
            (v.ToString() ?? string.Empty).Contains("Erro ao efetuar o Refresh")),
            null,
            It.IsAny<Func<It.IsAnyType, Exception?, string>>()), Times.Once);
        }

        [Fact]
        public void Refresh_NonExistentUser_ReturnsBadRequest()
        {
            // Arrange
            var input = new RefreshRequestModel
            {
                token = "expiredToken",
                refreshToken = "validRefreshToken"
            };

            var claims = new List<Claim> { new Claim(ClaimTypes.Name, "nonexistentuser") };
            var principal = new ClaimsPrincipal(new ClaimsIdentity(claims));

            _mockTokenServices.Setup(s => s.GetPrincipalFromExpiredToken(input.token)).Returns(principal);
            _mockTokenServices.Setup(s => s.GetRefreshToken("nonexistentuser")).Returns(string.Empty);

            // Act
            var result = _controller.Refresh(input).Result as BadRequestObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(400, result.StatusCode);
            Assert.Equal("Inválido Refresh", result.Value);

            _mockLogger.Verify(x =>
            x.Log(LogLevel.Error, It.IsAny<EventId>(),
            It.Is<It.IsAnyType>((v, t) =>
            (v.ToString() ?? string.Empty).Contains("Erro ao efetuar o Refresh")),
            null,
            It.IsAny<Func<It.IsAnyType, Exception?, string>>()), Times.Once);
        }
    }
}
