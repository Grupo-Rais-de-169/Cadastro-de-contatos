using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using Xunit;
using TechChallenge.Domain.Interfaces.Repositories;
using TechChallenge.Domain.Interfaces.Services;
using TechChallenge.Domain.Model;
using TechChallenge.Domain.Model.ViewModel;
using TechChallenge.Domain.Utils;
using AutoMapper;
using TechChallenge.Domain.Services;
using TechChallenge.Domain;
using Microsoft.Extensions.Caching.Memory;

namespace TechChallenge.Tests.Domain.Services
{
    public class ContatoServiceTests
    {
        private readonly Mock<IContatosRepository> _mockContatosRepository;
        private readonly Mock<ICodigoDeAreaRepository> _mockCodigoAreaRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly IContatoService _contatoService;
        private readonly IMemoryCache _memoryCache;

        public ContatoServiceTests()
        {
            _mockContatosRepository = new Mock<IContatosRepository>();
            _mockCodigoAreaRepository = new Mock<ICodigoDeAreaRepository>();
            _memoryCache = new MemoryCache(new MemoryCacheOptions());
            _mockMapper = new Mock<IMapper>();
            _contatoService = new ContatoService(
                _mockMapper.Object,
                _mockContatosRepository.Object,
                _mockCodigoAreaRepository.Object,
                _memoryCache
                
            );
        }

        [Fact]
        public async Task GetAllDDD_ShouldReturnAll_WhenIdIsNull()
        {
            // Arrange
            var ddds = new List<CodigoDeArea> { new CodigoDeArea { Id = 1, Regiao = "Teste" } };
            _mockContatosRepository.Setup(repo => repo.GetAllDDD(null)).ReturnsAsync(ddds);

            // Act
            var result = await _contatoService.GetAllDDD();

            // Assert
            Assert.Equal(ddds, result);
        }

        [Fact]
        public async Task GetContatoByDDD_ShouldReturnMappedContatos_WhenDDDExists()
        {
            // Arrange
            var contatos = new List<Contato>
            {
                new Contato
                {
                    Id = 1,
                    Nome = "Teste",
                    IdDDD = 11,
                    Ddd = new CodigoDeArea
                    {
                        Id = 11,
                        Regiao = "minas"
                    }
                }
            };

            var contatoDtos = new List<ContatoDto> { new ContatoDto { Id = 1, Nome = "Teste" } };
            _mockContatosRepository.Setup(repo => repo.GetContatoByDDD(11)).ReturnsAsync(contatos);
            _mockMapper.Setup(mapper => mapper.Map<IEnumerable<ContatoDto>>(contatos)).Returns(contatoDtos);

            // Act
            var result = await _contatoService.GetContatoByDDD(11);

            // Assert
            Assert.Equal(contatoDtos, result.ToList());
        }

        // =================

        [Fact]
        public void GetAll_ShouldReturnContatosFromCache()
        {
            // Arrange
            var contatosCache = new List<Contato> { new Contato { Id = 1, Nome = "Teste" } };
            var contatoDtos = new List<ContatoDto> { new ContatoDto { Id = 1, Nome = "Teste" } };
            _memoryCache.Set("Contatos", contatosCache);
            _mockMapper.Setup(mapper => mapper.Map<List<ContatoDto>>(contatosCache))
                       .Returns(contatoDtos);

            // Act
            var result = _contatoService.GetAll();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(contatoDtos.Count, result.Count);
            Assert.Equal(contatoDtos[0].Nome, result[0].Nome); 
        }

        [Fact]
        public void GetAll_ShouldCallRepositoryIfCacheIsEmpty()
        {
            // Arrange
            var contatos = new List<Contato> { new Contato { Id = 1, Nome = "Teste" } };
            var contatoDtos = new List<ContatoDto> { new ContatoDto { Id = 1, Nome = "Teste" } };
            _memoryCache.Remove("Contatos");
            _mockContatosRepository.Setup(repo => repo.GetAll()).Returns(contatos);
            _mockMapper.Setup(mapper => mapper.Map<List<ContatoDto>>(contatos))
                       .Returns(contatoDtos);

            // Act
            var result = _contatoService.GetAll();

            // Assert
            Assert.NotNull(result); 
            Assert.Equal(contatoDtos.Count, result.Count); 
            Assert.Equal(contatoDtos[0].Nome, result[0].Nome); 

            _mockContatosRepository.Verify(repo => repo.GetAll(), Times.Once);
            _mockMapper.Verify(mapper => mapper.Map<List<ContatoDto>>(contatos), Times.Once);
            var cacheContatos = _memoryCache.Get<List<Contato>>("Contatos");
            Assert.Equal(contatos, cacheContatos);
        }

        [Fact]
        public void GetAll_ShouldThrowExceptionIfRepositoryFails()
        {
            // Arrange
            _memoryCache.Remove("Contatos");
            _mockContatosRepository.Setup(repo => repo.GetAll()).Throws(new Exception("Repository error"));

            // Act & Assert
            var exception = Assert.Throws<Exception>(() => _contatoService.GetAll());
            Assert.Equal("Repository error", exception.Message);
            _mockContatosRepository.Verify(repo => repo.GetAll(), Times.Once);
            Assert.Null(_memoryCache.Get<List<Contato>>("Contatos"));
        }





        // =========================================================================================================================================================================

        /*[Fact]
        public void GetAll_ShouldReturnMappedContatos()
        {
            // Arrange
            var contatos = new List<Contato> { new Contato { Id = 1, Nome = "Teste" } };
            var contatoDtos = new List<ContatoDto> { new ContatoDto { Id = 1, Nome = "Teste" } };
            _mockContatosRepository.Setup(repo => repo.GetAll()).Returns(contatos);
            _mockMapper.Setup(mapper => mapper.Map<List<ContatoDto>>(contatos)).Returns(contatoDtos);

            // Act
            var result = _contatoService.GetAll();

            // Assert
            Assert.NotNull(result);
            //Assert.Equal(contatoDtos, result);
            Assert.Equal(contatoDtos.Count, result.Count);
            for (int i = 0; i < contatoDtos.Count; i++)
            {
                Assert.Equal(contatoDtos[i].Id, result[i].Id);
                Assert.Equal(contatoDtos[i].Nome, result[i].Nome);
            }
            _mockContatosRepository.Verify(repo => repo.GetAll(), Times.Once);
            _mockMapper.Verify(mapper => mapper.Map<List<ContatoDto>>(contatos), Times.Once);
        }*/

        // ==============================================================================================================================================================================

        [Fact]
        public async Task GetByIdAsync_ShouldReturnMappedContato_WhenIdExists()
        {
            // Arrange
            var contato = new Contato { Id = 1, Nome = "Teste" };
            var contatoDto = new ContatoDto { Id = 1, Nome = "Teste" };
            _mockContatosRepository.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(contato);
            _mockMapper.Setup(mapper => mapper.Map<ContatoDto>(contato)).Returns(contatoDto);

            // Act
            var result = await _contatoService.GetByIdAsync(1);

            // Assert
            Assert.Equal(contatoDto, result);
        }

        [Fact]
        public async Task AddAsync_ShouldReturnFailure_WhenDDDDoesNotExist()
        {
            // Arrange
            var contatoInclusao = new ContatoInclusaoViewModel { IdDDD = 99 };
            _mockCodigoAreaRepository.Setup(repo => repo.GetById(99)).Returns((CodigoDeArea)null);

            // Act
            var result = await _contatoService.AddAsync(contatoInclusao);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("O DDD informado não existe.", result.Message);
        }

        [Fact]
        public void Update_ShouldReturnFailure_WhenContatoDoesNotExist()
        {
            // Arrange
            var contatoAlteracao = new ContatoAlteracaoViewModel { Id = 1, Nome = "Novo Nome" };
            _mockContatosRepository.Setup(repo => repo.GetById(1)).Returns((Contato)null);

            // Act
            var result = _contatoService.Update(contatoAlteracao);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("Contato não encontrado!", result.Message);
        }

        [Fact]
        public void Delete_ShouldReturnFailure_WhenContatoDoesNotExist()
        {
            // Arrange
            _mockContatosRepository.Setup(repo => repo.GetById(1)).Returns((Contato)null);

            // Act
            var result = _contatoService.Delete(1);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("Contato não encontrado!", result.Message);
        }
    }
}
