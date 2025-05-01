using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Dynamic;
using System.Text.Json;
using TechChallenge.Cadastro.Api.Controllers;
using TechChallenge.Cadastro.Api.Services.Interfaces;
using TechChallenge.Cadastro.Api.Utils;
using TechChallenge.Cadastro.Api.ViewModel;

namespace TechChallenge.UnitTest
{
    public class AlterarContatoTest
    {
        private readonly Mock<IContatoService> _contatoService;
        private readonly ContatoController _controller;

        public AlterarContatoTest()
        {
            _contatoService = new Mock<IContatoService>();
            _controller = new ContatoController(_contatoService.Object);
        }

        [Fact]
        public async Task AlteraContato_DadosValidos_RetornaNoContent()
        {
            // Arrange
            var contato = new ContatoAlteracaoViewModel
            {
                Id = 1,
                Nome = "Contato Atualizado",
                Telefone = "11988888888",
                IdDDD = 11
            };

            var serviceResult = new Result { IsSuccess = true };
            _contatoService.Setup(service => service.UpdateAsync(contato)).ReturnsAsync(serviceResult);

            // Act
            var result = await _controller.AlteraContato(contato) as NoContentResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(204, result.StatusCode); // No Content
        }

        [Fact]
        public async Task AlteraContato_DadosInvalidos_RetornaBadRequest()
        {
            // Arrange
            _controller.ModelState.AddModelError("Nome", "O campo Nome é obrigatório.");
            var contato = new ContatoAlteracaoViewModel
            {
                Id = 1,
                Nome = "",
                Telefone = "11988888888",
                IdDDD = 11
            };

            // Act
            var result = await _controller.AlteraContato(contato) as BadRequestObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(400, result.StatusCode); // Bad Request
            Assert.IsType<SerializableError>(result.Value);
        }

        [Fact]
        public async Task AlteraContato_ContatoNaoEncontrado_RetornaNotFound()
        {
            // Arrange
            var contato = new ContatoAlteracaoViewModel
            {
                Id = 99, // ID inexistente
                Nome = "Contato Não Encontrado",
                Telefone = "11999999999",
                IdDDD = 99
            };

            var serviceResult = new Result { Message = "Contato não encontrado" };
            _contatoService.Setup(service => service.UpdateAsync(contato)).ReturnsAsync(serviceResult);

            // Act
            var result = await _controller.AlteraContato(contato) as NotFoundObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(404, result.StatusCode); // Not Found

            var json = JsonSerializer.Serialize(result.Value);
            dynamic? response = JsonSerializer.Deserialize<ExpandoObject>(json);
            Assert.Equal("Contato não encontrado", response?.message.GetString());
        }
    }
}
