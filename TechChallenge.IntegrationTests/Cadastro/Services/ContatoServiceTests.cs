using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Moq;
using Moq.Protected;
using Newtonsoft.Json;
using System.Net;
using System.Text;
using TechChallenge.Cadastro.Api.Configuration;
using TechChallenge.Cadastro.Api.Model;
using TechChallenge.Cadastro.Api.Services;
using TechChallenge.Cadastro.Api.Utils;
using TechChallenge.Cadastro.Api.ViewModel;

namespace TechChallenge.IntegrationTests.Cadastro.Services
{
    public class ContatoServiceTests
    {
        private readonly Mock<HttpClient> _httpClientMock;
        private readonly string _baseUrl = "http://mocked-url/";

        public ContatoServiceTests()
        {
            _httpClientMock = new Mock<HttpClient>();
            var configMock = new Mock<IOptions<MicroservicoConfig>>();
            configMock.Setup(c => c.Value).Returns(new MicroservicoConfig { DAO = _baseUrl });
        }



        [Fact]
        public async Task GetContatoByDDD_QuandoDDDExiste_DeveRetornarContatos()
        {
            // Arrange
            int ddd = 11;
            var expectedContatos = new List<Contato> { new Contato { Id = 1, Nome = "John Doe" } };

            var json = JsonConvert.SerializeObject(expectedContatos);

            var responseMessage = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            };

            var httpClient = CreateMockHttpClient($"GetContatoPorDDD/{ddd}", responseMessage);

            var config = Options.Create(new MicroservicoConfig { DAO = "http://localhost/" });
            var cache = new MemoryCache(new MemoryCacheOptions());

            var service = new ContatoService(httpClient, config, cache);

            // Act
            var result = await service.GetContatoByDDD(ddd);

            // Assert
            Assert.NotNull(result);
            var contato = result.FirstOrDefault();
            Assert.NotNull(contato);
            Assert.Equal("John Doe", contato.Nome);
        }

        [Fact]
        public async Task GetAllAsync_QuandoChamadoMultiplasVezes_DeveRetornarContatosDoCache()
        {
            // Arrange
            var expectedContatos = new List<Contato> { new Contato { Id = 1, Nome = "Maria" } };

            var json = JsonConvert.SerializeObject(expectedContatos);

            var responseMessage = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            };

            var httpClient = CreateMockHttpClient("GetAllContatos/", responseMessage);

            var config = Options.Create(new MicroservicoConfig { DAO = "http://localhost/" });
            var memoryCache = new MemoryCache(new MemoryCacheOptions());

            var service = new ContatoService(httpClient, config, memoryCache);

            // Act 1: Primeira chamada popula o cache
            var primeiraChamada = await service.GetAllAsync();

            // Act 2: Segunda chamada deve usar o cache (sem chamar o HttpClient novamente)
            var segundaChamada = await service.GetAllAsync();

            // Assert
            Assert.NotNull(primeiraChamada);
            Assert.Single(primeiraChamada);
            Assert.Equal("Maria", primeiraChamada.First().Nome);

            Assert.NotNull(segundaChamada);
            Assert.Single(segundaChamada);
            Assert.Equal("Maria", segundaChamada.First().Nome);
        }


        [Fact]
        public async Task AddAsync_QuandoContatoValido_DeveRetornarSucesso()
        {
            // Arrange
            var contato = new ContatoInclusaoViewModel
            {
                Nome = "Lucas",
                IdDDD = 31,
                Telefone = "98888-1234"
            };

            var jsonResult = JsonConvert.SerializeObject(new Result
            {
                IsSuccess = true,
                Message = "Contato adicionado com sucesso"
            });

            var responseMessage = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(jsonResult, Encoding.UTF8, "application/json")
            };

            var httpClient = CreateMockHttpClient("CadastraContato/", responseMessage);

            var config = Options.Create(new MicroservicoConfig { DAO = "http://localhost/" });
            var memoryCache = new MemoryCache(new MemoryCacheOptions());

            var service = new ContatoService(httpClient, config, memoryCache);

            // Act
            var result = await service.AddAsync(contato);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.IsSuccess);
            Assert.Equal("Contato adicionado com sucesso", result.Message);
        }

        [Fact]
        public async Task UpdateAsync_QuandoContatoExiste_DeveRetornarSucesso()
        {
            // Arrange
            var contato = new ContatoAlteracaoViewModel
            {
                Id = 1,
                Nome = "Carlos",
                IdDDD = 85,
                Telefone = "97777-1234"
            };

            var expectedResult = new Result
            {
                IsSuccess = true,
                Message = "Contato atualizado com sucesso"
            };

            var jsonResult = JsonConvert.SerializeObject(expectedResult);

            var responseMessage = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(jsonResult, Encoding.UTF8, "application/json")
            };

            var httpClient = CreateMockHttpClient("AtualizaContato/", responseMessage);

            var config = Options.Create(new MicroservicoConfig { DAO = "http://localhost/" });
            var memoryCache = new MemoryCache(new MemoryCacheOptions());

            var service = new ContatoService(httpClient, config, memoryCache);

            // Act
            var result = await service.UpdateAsync(contato);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.IsSuccess);
            Assert.Equal("Contato atualizado com sucesso", result.Message);
        }

        [Fact]
        public async Task DeleteAsync_QuandoContatoExiste_DeveRetornarSucesso()
        {
            // Arrange
            int contatoId = 1;

            var expectedResult = new Result
            {
                IsSuccess = true,
                Message = "Contato removido com sucesso"
            };

            var jsonResult = JsonConvert.SerializeObject(expectedResult);

            var responseMessage = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(jsonResult, Encoding.UTF8, "application/json")
            };

            var httpClient = CreateMockHttpClient($"DeletaContato/{contatoId}", responseMessage);

            var config = Options.Create(new MicroservicoConfig { DAO = "http://localhost/" });
            var memoryCache = new MemoryCache(new MemoryCacheOptions());

            var service = new ContatoService(httpClient, config, memoryCache);

            // Act
            var result = await service.DeleteAsync(contatoId);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.IsSuccess);
            Assert.Equal("Contato removido com sucesso", result.Message);
        }

        [Fact]
        public void DeletaCache_QuandoExecutado_DeveRemoverContatosDoCache()
        {
            // Arrange
            var cacheMock = new Mock<IMemoryCache>();
            var service = new ContatoService(_httpClientMock.Object, Options.Create(new MicroservicoConfig { DAO = _baseUrl }), cacheMock.Object);

            // Act
            service.DeletaCache();

            // Assert
            cacheMock.Verify(c => c.Remove("Contatos"), Times.Once);
            cacheMock.Verify(c => c.Remove("ContatosAsync"), Times.Once);
        }

        public static HttpClient CreateMockHttpClient(string expectedUrl, HttpResponseMessage responseMessage)
        {
            var handlerMock = new Mock<HttpMessageHandler>();

            handlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(req => req.RequestUri.ToString().Contains(expectedUrl)),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(responseMessage);

            return new HttpClient(handlerMock.Object);
        }
    }
}