using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Moq;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using TechChallenge.Cadastro.Api.Producers.Contato;
using TechChallenge.Core.ViewModels;
using TechChallenge.DAO.Api;
using TechChallenge.DAO.Api.Entities;
using TechChallenge.DAO.Api.Infra.Repository.Interfaces;
using TechChallenge.DAO.Api.Utils;
namespace TechChallenge.IntegrationTests.DAO.Controller
{
    public class ContatoControllerTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly HttpClient _client;
        private readonly CustomWebApplicationFactory _factory;

        public ContatoControllerTests(CustomWebApplicationFactory factory)
        {
            _factory = factory;
            _client = _factory.CreateClient();

            var configuration = JwtTokenGenerator.LoadConfiguration();
            var token = JwtTokenGenerator.GerarJwt(configuration);
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        [Fact]
        public async Task GetAllContatos_DeveRetornar200()
        {
            var response = await _client.GetAsync("/api/Contato/GetAllContatos");

            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task GetContatoPorDDD_NaoEncontrado_DeveRetornar404()
        {
            var mockRepo = new Mock<IContatosRepository>();

            mockRepo.Setup(r => r.GetContatoByDDD(999))
                .ReturnsAsync(new List<Contato>());

            var response = await _client.GetAsync("/api/Contato/GetContatoPorDDD/999");            

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
            var body = await response.Content.ReadAsStringAsync();
            body.Should().Contain("DDD informado");
        }

        //[Fact]
        //public async Task CadastraContato_ComDDDInvalido_DeveRetornarFalha()
        //{
        //    var contato = new ContatoInclusaoViewModel
        //    {
        //        Nome = "João",
        //        Email = "joao@email.com",
        //        Telefone = "11999999999",
        //        IdDDD = 000 // inválido
        //    };

        //    var content = new StringContent(JsonConvert.SerializeObject(contato), Encoding.UTF8, "application/json");

        //    var response = await _client.PostAsync("/api/Contato/CadastraContato", content);
        //    var json = await response.Content.ReadAsStringAsync();
        //    response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        //    var result = JsonConvert.DeserializeObject<Result>(await response.Content.ReadAsStringAsync());
        //    result.IsSuccess.Should().BeFalse();
        //    result.Message.Should().Contain("O DDD informado não existe.");
        //}

        //[Fact]
        //public async Task AtualizaContato_ContatoInexistente_DeveRetornarFalha()
        //{
        //    var contato = new ContatoAlteracaoViewModel
        //    {
        //        Id = 9999, // não existe
        //        Nome = "Novo Nome",
        //        Email = "novo@email.com",
        //        Telefone = "1122223333",
        //        IdDDD = 11
        //    };

        //    var producerMock = new Mock<IContatoProducer>();
        //    producerMock
        //        .Setup(p => p.ExecuteAsync(It.IsAny<ContatoAlteracaoViewModel>()))
        //        .Returns(Task.CompletedTask);

        //    var content = new StringContent(JsonConvert.SerializeObject(contato), Encoding.UTF8, "application/json");

        //    var response = await _client.PutAsync("/api/Contato/AtualizaContato", content);

        //    response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        //    var result = JsonConvert.DeserializeObject<Result>(await response.Content.ReadAsStringAsync());
        //    result.IsSuccess.Should().BeFalse();
        //    result.Message.Should().Contain("Contato não encontrado!");
        //}

        //[Fact]
        //public async Task DeleteContato_ContatoInexistente_DeveRetornarFalha()
        //{
        //    var response = await _client.DeleteAsync("/api/Contato/DeletaContato/9999");

        //    response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        //    var result = JsonConvert.DeserializeObject<Result>(await response.Content.ReadAsStringAsync());
        //    result.IsSuccess.Should().BeFalse();
        //    result.Message.Should().Contain("Contato não encontrado!");
        //}
    }
}