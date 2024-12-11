using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechChallenge.Domain;
using TechChallenge.Infra.Context;
using TechChallenge.Infra.Repositories;

namespace TechChallenge.Tests.Infra.Repositories
{
    public class AuthRepositoriesTests
    {
        private readonly Mock<IDbContextFactory<MainContext>> _dbContextFactory;

        public AuthRepositoriesTests()
        {
            _dbContextFactory = new Mock<IDbContextFactory<MainContext>>();
        }
        private MainContext GetInMemoryContext()
        {
            var options = new DbContextOptionsBuilder<MainContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;
            var mockedDbContext = new MainContext(options, _dbContextFactory.Object);
            _dbContextFactory.Setup(f => f.CreateDbContext()).Returns(mockedDbContext);
            return mockedDbContext;
        }

        [Fact]
        public async Task GetConfirmLoginAndPassword_ShouldReturnUser_WhenUserExists()
        {
            // Arrange
            using var context = GetInMemoryContext();
            var expectedUser = new Usuario { Login = "test_user", Senha = "test_password", Permissao = new Permissao { Funcao = "adminTest", Id = 1} }; 
            context.Usuarios.Add(expectedUser);
            context.SaveChanges();

            var repository = new AuthRepositories(context);

            // Act
            var result = await repository.GetConfirmLoginAndPassword("test_user");

            // Assert
            Assert.NotNull(result);
            Assert.Equal("test_user", result.Login);
            Assert.Equal("test_password",result.Senha);
        }

        [Fact]
        public async Task GetConfirmLoginAndPassword_ShouldReturnNull_WhenUserDoesNotExist()
        {
            // Arrange
            using var context = GetInMemoryContext();
            var repository = new AuthRepositories(context);

            // Act
            var result = await repository.GetConfirmLoginAndPassword("nonexistent_user");

            // Assert
            Assert.Null(result);
        }
    }
}
