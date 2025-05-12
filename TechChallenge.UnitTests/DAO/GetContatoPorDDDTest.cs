using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using TechChallenge.DAO.Api.Controllers;
using TechChallenge.DAO.Api.Entities;
using TechChallenge.DAO.Api.Infra.Repository.Interfaces;

namespace TechChallenge.UnitTests.DAO
{
    public class GetContatoPorDDDTest
    {
        private readonly Mock<IContatosRepository> _mockContatoRepository;
        private readonly Mock<ICodigoDeAreaRepository> _mockCodigoAreaRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly ContatoController _controller;

        public GetContatoPorDDDTest()
        {
            _mockContatoRepository = new Mock<IContatosRepository>();
            _mockCodigoAreaRepository = new Mock<ICodigoDeAreaRepository>();
            _mockMapper = new Mock<IMapper>();
            _controller = new ContatoController(
                _mockContatoRepository.Object,
                _mockCodigoAreaRepository.Object,
                _mockMapper.Object
            );
        }

        [Fact]
        public async Task GetContatoAll_ReturnsOkResult_WithListOfContatos()
        {
            // Arrange
            var contatos = new List<Contato> { new Contato { Id = 1, Nome = "John Doe" } };
            _mockContatoRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(contatos);

            // Act
            var result = await _controller.GetContatoAll();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(contatos, okResult.Value);
        }

        [Fact]
        public async Task GetContatoPorDDD_ReturnsNotFound_WhenNoContatosFound()
        {
            // Arrange
            int ddd = 11;
            _mockContatoRepository.Setup(repo => repo.GetContatoByDDD(ddd)).ReturnsAsync(new List<Contato>());

            // Act
            var result = await _controller.GetContatoPorDDD(ddd);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            var value = notFoundResult.Value;

            var message = value.GetType().GetProperty("message")?.GetValue(value, null)?.ToString();

            Assert.Equal("Não foi encontrado contatos com o DDD informado", message);
        }

        [Fact]
        public async Task GetContatoPorDDD_ReturnsOkResult_WithContatos()
        {
            // Arrange
            int ddd = 11;
            var contatos = new List<Contato> { new Contato { Id = 1, Nome = "John Doe" } };
            _mockContatoRepository.Setup(repo => repo.GetContatoByDDD(ddd)).ReturnsAsync(contatos);

            // Act
            var result = await _controller.GetContatoPorDDD(ddd);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(contatos, okResult.Value);
        }
    }
}
