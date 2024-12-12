using AutoMapper;
using Microsoft.Extensions.Caching.Memory;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechChallenge.Domain.Interfaces.Repositories;
using TechChallenge.Domain.Interfaces.Services;
using TechChallenge.Domain.Model.DTO;
using TechChallenge.Domain.Services;
using TechChallenge.Domain;
using Microsoft.EntityFrameworkCore;
using TechChallenge.Domain.Model.ViewModel;

namespace TechChallenge.Tests.Domain.Services
{
    public class UsuarioServiceTests
    {
        private readonly Mock<IUsuarioRepository> _mockUsuarioRepository;
        private readonly Mock<IMemoryCache> _mockCache;
        private readonly Mock<IPasswordService> _mockPasswordService;
        private readonly Mock<IPermissaoRepository> _mockPermissaoRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly UsuarioService _usuarioService;
        private readonly Mock<IUsuarioService> _mockUsuarioService;

        public UsuarioServiceTests()
        {
            _mockUsuarioRepository = new Mock<IUsuarioRepository>();
            _mockCache = new Mock<IMemoryCache>();
            _mockPasswordService = new Mock<IPasswordService>();
            _mockPermissaoRepository = new Mock<IPermissaoRepository>();
            _mockMapper = new Mock<IMapper>();
            _mockUsuarioService = new Mock<IUsuarioService>();


            _usuarioService = new UsuarioService(
                _mockUsuarioRepository.Object,
                _mockMapper.Object,
                _mockCache.Object,
                _mockPasswordService.Object,
                _mockPermissaoRepository.Object
            );
        }

        [Fact]
        public async Task AddAsync_ShouldReturnFailure_WhenPermissaoDoesNotExist()
        {
            // Arrange
            var usuarioViewModel = new UsuarioInclusaoViewModel { Login = "user1", Senha = "password", PermissaoId = 999 };
            _mockPermissaoRepository.Setup(p => p.GetById(999)).Returns((Permissao)null);

            // Act
            var result = await _usuarioService.AddAsync(usuarioViewModel);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("O ID da permissão informado não existe.", result.Message);
        }

        [Fact]
        public async Task AddAsync_ShouldAddUsuario_WhenDataIsValid()
        {
            // Arrange
            var usuarioViewModel = new UsuarioInclusaoViewModel { Login = "user1", Senha = "password", PermissaoId = 1 };
            _mockPermissaoRepository.Setup(p => p.GetById(1)).Returns(new Permissao { Id = 1 });
            _mockUsuarioRepository.Setup(r => r.AddAsync(It.IsAny<Usuario>())).Returns(Task.CompletedTask);
            _mockPasswordService.Setup(p => p.GerarHash("password")).Returns("hashedpassword");

            // Act
            var result = await _usuarioService.AddAsync(usuarioViewModel);

            // Assert
            Assert.True(result.IsSuccess);
        }

        [Fact]
        public void Update_ShouldReturnFailure_WhenUsuarioNotFound()
        {
            // Arrange
            var usuarioAlteracaoViewModel = new UsuarioAlteracaoViewModel { Id = 1, Login = "user1", Senha = "newpassword", PermissaoId = 1 };
            _mockUsuarioRepository.Setup(r => r.GetById(1)).Returns((Usuario)null);

            // Act
            var result = _usuarioService.Update(usuarioAlteracaoViewModel);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("Usuário não encontrado!", result.Message);
        }

        [Fact]
        public void Delete_ShouldReturnFailure_WhenUsuarioNotFound()
        {
            // Arrange
            _mockUsuarioRepository.Setup(r => r.GetById(1)).Returns((Usuario)null);

            // Act
            var result = _usuarioService.Delete(1);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("Usuário não encontrado!", result.Message);
        }

        [Fact]
        public void Delete_ShouldDeleteUsuario_WhenUsuarioExists()
        {
            // Arrange
            var usuario = new Usuario { Id = 1, Login = "user1" };
            _mockUsuarioRepository.Setup(r => r.GetById(1)).Returns(usuario);
            _mockUsuarioRepository.Setup(r => r.Delete(1)).Verifiable();

            // Act
            var result = _usuarioService.Delete(1);

            // Assert
            Assert.True(result.IsSuccess);
            _mockUsuarioRepository.Verify(r => r.Delete(1), Times.Once);
        }

    }
}
