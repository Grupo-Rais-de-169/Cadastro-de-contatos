using FluentAssertions;
using Moq;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using TechChallenge.Cadastro.Api.Model;
using TechChallenge.Cadastro.Api.Services.Interfaces;
using TechChallenge.Cadastro.Api.Utils;
using TechChallenge.Cadastro.Api.ViewModel;

namespace TechChallenge.IntegrationTests.Cadastro.Controller
{
    public class ContatoControllerTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly HttpClient _client;
        private readonly Mock<IContatoService> _contatoServiceMock;

        public ContatoControllerTests(CustomWebApplicationFactory factory)
        {
            _client = factory.CreateClient();
            _contatoServiceMock = factory.ContatoServiceMock;
            var configuration = JwtTokenGenerator.LoadConfiguration();
            var token = JwtTokenGenerator.GerarJwt(configuration);

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }


        [Fact]
        public async Task GetAllContatos_DeveRetornarOk()
        {
            _contatoServiceMock
                .Setup(service => service.GetAllAsync())
                .ReturnsAsync(new List<Contato>());

            var response = await _client.GetAsync("/api/Contato/GetAllContatos");

            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task GetContatoPorDDD_ComDDDInvalido_DeveRetornarNotFound()
        {
            _contatoServiceMock
                .Setup(service => service.GetContatoByDDD(It.IsAny<int>()))
                .ReturnsAsync(new List<Contato>()); // lista vazia simula DDD inválido

            var response = await _client.GetAsync("/api/Contato/GetContatoPorDDD/000");

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);

            var body = await response.Content.ReadAsStringAsync();
            body.Should().Contain("DDD");
        }

        [Fact]
        public async Task CriaContato_ComDDDInvalido_DeveRetornarNotFound()
        {
            var contato = new ContatoInclusaoViewModel
            {
                Nome = "Maria",
                Email = "maria@email.com",
                Telefone = "11912345678",
                IdDDD = 99 // inválido
            };

            _contatoServiceMock
                .Setup(service => service.AddAsync(It.IsAny<ContatoInclusaoViewModel>()))
                .ReturnsAsync(Result.Failure("DDD inválido"));

            var content = new StringContent(JsonConvert.SerializeObject(contato), Encoding.UTF8, "application/json");

            var response = await _client.PostAsync("/api/Contato", content);

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);

            var body = await response.Content.ReadAsStringAsync();
            body.Should().Contain("DDD");
        }

        [Fact]
        public async Task AlteraContato_ComContatoInvalido_DeveRetornarNotFound()
        {
            _contatoServiceMock
                .Setup(service => service.UpdateAsync(It.IsAny<ContatoAlteracaoViewModel>()))
                .ReturnsAsync(Result.Failure("Contato inválido"));

            var contato = new ContatoAlteracaoViewModel
            {
                Id = 999,
                Nome = "Teste",
                Email = "teste@email.com",
                Telefone = "11900000000",
                IdDDD = 000
            };

            var content = new StringContent(JsonConvert.SerializeObject(contato), Encoding.UTF8, "application/json");

            var response = await _client.PutAsync("/api/Contato", content);

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);

            var body = await response.Content.ReadAsStringAsync();
            body.Should().Contain("Contato");
        }

        [Fact]
        public async Task DeleteContato_ComIdInvalido_DeveRetornarNotFound()
        {
            _contatoServiceMock
                .Setup(service => service.DeleteAsync(It.IsAny<int>()))
                .ReturnsAsync(Result.Failure("Contato não encontrado!"));

            var response = await _client.DeleteAsync("/api/Contato/-1");

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);

            var body = await response.Content.ReadAsStringAsync();
            body.Should().Contain("Contato");
        }
    }
}

