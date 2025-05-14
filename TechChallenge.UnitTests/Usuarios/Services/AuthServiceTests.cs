using Moq;
using TechChallenge.Usuarios.Api.Application;
using TechChallenge.Usuarios.Api.Domain.Entities;
using TechChallenge.Usuarios.Api.Domain.Interfaces;

namespace TechChallenge.UnitTests.Usuarios.Services
{
    public class AuthServiceTests
    {
        private readonly Mock<IAuthRepositories> _mockAuthRepositories;
        private readonly IAuthService _authService;

        public AuthServiceTests()
        {
            _mockAuthRepositories = new Mock<IAuthRepositories>();
            _authService = new AuthService(_mockAuthRepositories.Object);
        }

        [Fact]
        public async Task GetConfirmLoginAndPassword_ShouldReturnUser_WhenCredentialsAreCorrect()
        {
            // Arrange
            var username = "validUser";
            var password = "validPassword";
            var expectedUser = new Usuario
            {
                Id = 1,
                Login = username,
                Senha = password,
                PermissaoId = 1
            };

            _mockAuthRepositories
                .Setup(repo => repo.GetConfirmLoginAndPassword(username))
                .ReturnsAsync(expectedUser);

            // Act
            var result = await _authService.GetConfirmLoginAndPassword(username, password);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedUser.Id, result?.Id);
            Assert.Equal(expectedUser.Login, result?.Login);
            _mockAuthRepositories.Verify(repo => repo.GetConfirmLoginAndPassword(username), Times.Once);
        }

        [Fact]
        public async Task GetConfirmLoginAndPassword_ShouldReturnNull_WhenCredentialsAreIncorrect()
        {
            // Arrange
            var username = "invalidUser";
            var password = "invalidPassword";

            _mockAuthRepositories
                .Setup(repo => repo.GetConfirmLoginAndPassword(username))
                .ReturnsAsync((Usuario?)null);

            // Act
            var result = await _authService.GetConfirmLoginAndPassword(username, password);

            // Assert
            Assert.Null(result);
            _mockAuthRepositories.Verify(repo => repo.GetConfirmLoginAndPassword(username), Times.Once);
        }

        [Fact]
        public async Task GetConfirmLoginAndPassword_ShouldCallRepositoryWithCorrectParameters()
        {
            // Arrange
            var username = "testUser";
            var password = "testPassword";

            // Act
            await _authService.GetConfirmLoginAndPassword(username, password);

            // Assert
            _mockAuthRepositories.Verify(repo => repo.GetConfirmLoginAndPassword(username), Times.Once);
        }
    }
}
