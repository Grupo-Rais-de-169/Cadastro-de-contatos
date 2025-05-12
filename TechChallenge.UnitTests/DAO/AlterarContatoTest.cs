

using AutoMapper;
using Moq;
using TechChallenge.DAO.Api.Controllers;
using TechChallenge.DAO.Api.Entities;
using TechChallenge.DAO.Api.Infra.Repository.Interfaces;
using TechChallenge.DAO.Api.ViewModel;

namespace TechChallenge.UnitTests.DAO
{
    public class AlterarContatoTest
    {
        private readonly Mock<IContatosRepository> _mockContatoRepository;
        private readonly Mock<ICodigoDeAreaRepository> _mockCodigoAreaRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly ContatoController _controller;

        public AlterarContatoTest()
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
        public async Task AlteraContato_ReturnsFailure_WhenContatoNotFound()
        {
            // Arrange
            var contatoModel = new ContatoAlteracaoViewModel { Id = 1 };
            _mockContatoRepository.Setup(repo => repo.GetById(contatoModel.Id)).Returns((Contato)null);

            // Act
            var result = await _controller.AlteraContato(contatoModel);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("Contato não encontrado!", result.Message);
        }

        [Fact]
        public async Task AlteraContato_ReturnsSuccess_WhenContatoIsUpdated()
        {
            // Arrange
            var contatoModel = new ContatoAlteracaoViewModel { Id = 1, IdDDD = 11 };
            var existingContato = new Contato();
            _mockContatoRepository.Setup(repo => repo.GetById(contatoModel.Id)).Returns(existingContato);
            _mockCodigoAreaRepository.Setup(repo => repo.GetById(contatoModel.IdDDD)).Returns(new CodigoDeArea());

            // Act
            var result = await _controller.AlteraContato(contatoModel);

            // Assert
            Assert.True(result.IsSuccess);
        }
    }
}
