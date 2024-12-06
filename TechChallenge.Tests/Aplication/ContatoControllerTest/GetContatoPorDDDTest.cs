using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moq;
using TechChallenge.Api.Controllers;
using System.Dynamic;
using System.Text.Json;
using TechChallenge.Domain.Model;
using TechChallenge.Domain.Interfaces.Services;

namespace TechChallenge.Tests.Aplication.ContatoControllerTest
{
    public class GetContatoPorDDDTest
    {
        private readonly Mock<IContatoService> _contatoService;
        private readonly ContatoController _controller;
        private readonly Mock<IConfiguration> _mockConfiguration;

        public GetContatoPorDDDTest()
        {
            _contatoService = new Mock<IContatoService>();
            _mockConfiguration = new Mock<IConfiguration>();
            _mockConfiguration.Setup(config => config["Jwt:Key"]).Returns("FakeJwtKey");
            _mockConfiguration.Setup(config => config["Jwt:Issuer"]).Returns("FakeIssuer");
            _controller = new ContatoController(_contatoService.Object);
        }

        [Fact]
        public async Task GetContatoPorDDD_ContatosEncontrados_RetornaOk()
        {
            // Arrange
            int ddd = 11;
            var contatos = new List<ContatoDto>
            {
                new ContatoDto { Nome = "Contato 1", Telefone = "11123456789" },
                new ContatoDto { Nome = "Contato 2", Telefone = "11987654321" }
            };

            _contatoService.Setup(service => service.GetContatoByDDD(ddd)).ReturnsAsync(contatos);

            // Act
            var result = await _controller.GetContatoPorDDD(ddd) as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            Assert.Equal(contatos, result.Value);
        }

        [Fact]
        public async Task GetContatoPorDDD_NenhumContatoEncontrado_RetornaNotFound()
        {
            // Arrange
            int ddd = 99;
            var contatos = new List<ContatoDto>();

            _contatoService.Setup(service => service.GetContatoByDDD(ddd)).ReturnsAsync(contatos);

            // Act
            var resultPadrao = new NotFoundObjectResult(new { message = "Erro de validação" });
            NotFoundObjectResult result = await _controller.GetContatoPorDDD(ddd) as NotFoundObjectResult?? resultPadrao;
            var json = JsonSerializer.Serialize(result.Value);
            dynamic? response = JsonSerializer.Deserialize<ExpandoObject>(json);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(404, result.StatusCode);
            Assert.Equal("Não foi encontrado contatos com o DDD informado", response?.message.GetString());
        }
    }
}
