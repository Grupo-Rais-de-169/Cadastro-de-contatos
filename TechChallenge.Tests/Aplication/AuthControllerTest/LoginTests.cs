using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using TechChallenge.Api.Controllers;
using TechChallenge.Api.Services;
using TechChallenge.Domain.Models;
using TechChallenge.Domain;
using System.Dynamic;
using System.Text.Json;

namespace TechChallenge.Tests.Aplication.AuthControllerTest
{
    public class LoginTests
    {
        private readonly Mock<ITokenServices> _mockTokenServices;
        private readonly Mock<ILogger<AuthController>> _mockLogger;
        private readonly Mock<IConfiguration> _mockConfiguration;
        private readonly AuthController _controller;

        public LoginTests()
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
            var authRequest = new AuthRequestModel { login = "user", password = "password",permissao = "admin" };
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
    }
}
