using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using TechChallenge.Api.Controllers;
using TechChallenge.Api.Services;
using TechChallenge.Domain.Models;
using System.Dynamic;
using System.Text.Json;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;

namespace TechChallenge.Tests.Aplication.AuthControllerTest
{
    public class RefreshTokenTest
    {
        private readonly Mock<ITokenServices> _mockTokenServices;
        private readonly Mock<ILogger<AuthController>> _mockLogger;
        private readonly AuthController _controller;
        private readonly Mock<IConfiguration> _mockConfiguration;

        public RefreshTokenTest()
        {
            _mockTokenServices = new Mock<ITokenServices>();
            _mockLogger = new Mock<ILogger<AuthController>>();
            _mockConfiguration = new Mock<IConfiguration>();

            _mockConfiguration.Setup(config => config["Jwt:Key"]).Returns("FakeJwtKey");
            _mockConfiguration.Setup(config => config["Jwt:Issuer"]).Returns("FakeIssuer");

            _controller = new AuthController(/*_mockConfiguration.Object,*/ _mockLogger.Object, _mockTokenServices.Object);
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
