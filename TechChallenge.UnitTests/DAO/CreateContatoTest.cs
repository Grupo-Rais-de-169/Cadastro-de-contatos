using AutoMapper;
using Moq;
using TechChallenge.DAO.Api.Controllers;
using TechChallenge.DAO.Api.Entities;
using TechChallenge.DAO.Api.Infra.Repository.Interfaces;
using TechChallenge.DAO.Api.ViewModel;

namespace TechChallenge.UnitTests.DAO
{
    public class CreateContatoTest
    {
        private readonly Mock<IContatosRepository> _mockContatoRepository;
        private readonly Mock<ICodigoDeAreaRepository> _mockCodigoAreaRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly ContatoController _controller;

        public CreateContatoTest()
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
        public async Task CriaContato_ReturnsFailure_WhenDDDDoesNotExist()
        {
            // Arrange
            var contato = new ContatoInclusaoViewModel { IdDDD = 99 };
            _mockCodigoAreaRepository.Setup(repo => repo.GetById(contato.IdDDD)).Returns((CodigoDeArea)null);

            // Act
            var result = await _controller.CriaContato(contato);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("O DDD informado não existe.", result.Message);
        }

        [Fact]
        public async Task CriaContato_ReturnsSuccess_WhenContatoIsCreated()
        {
            // Arrange
            var contato = new ContatoInclusaoViewModel { IdDDD = 11 };
            var mappedContato = new Contato();
            _mockCodigoAreaRepository.Setup(repo => repo.GetById(contato.IdDDD)).Returns(new CodigoDeArea());
            _mockMapper.Setup(mapper => mapper.Map<Contato>(contato)).Returns(mappedContato);

            // Act
            var result = await _controller.CriaContato(contato);

            // Assert
            Assert.True(result.IsSuccess);
        }
    }
}
