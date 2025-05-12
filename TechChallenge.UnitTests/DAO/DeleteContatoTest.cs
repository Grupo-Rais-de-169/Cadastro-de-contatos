using AutoMapper;
using Moq;
using TechChallenge.DAO.Api.Controllers;
using TechChallenge.DAO.Api.Entities;
using TechChallenge.DAO.Api.Infra.Repository.Interfaces;

namespace TechChallenge.UnitTests.DAO
{
    public class DeleteContatoTest
    {
        private readonly Mock<IContatosRepository> _mockContatoRepository;
        private readonly Mock<ICodigoDeAreaRepository> _mockCodigoAreaRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly ContatoController _controller;

        public DeleteContatoTest()
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
        public async Task DeleteContato_ReturnsFailure_WhenContatoNotFound()
        {
            // Arrange
            int id = 1;
            _mockContatoRepository.Setup(repo => repo.GetById(id)).Returns((Contato)null);

            // Act
            var result = await _controller.DeleteContato(id);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("Contato não encontrado!", result.Message);
        }

        [Fact]
        public async Task DeleteContato_ReturnsSuccess_WhenContatoIsDeleted()
        {
            // Arrange
            int id = 1;
            var existingContato = new Contato();
            _mockContatoRepository.Setup(repo => repo.GetById(id)).Returns(existingContato);

            // Act
            var result = await _controller.DeleteContato(id);

            // Assert
            Assert.True(result.IsSuccess);
        }
    }
}
