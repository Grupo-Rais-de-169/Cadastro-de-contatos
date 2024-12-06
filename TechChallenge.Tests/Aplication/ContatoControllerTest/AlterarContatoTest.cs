using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Dynamic;
using System.Text.Json;
using TechChallenge.Api.Controllers;
using TechChallenge.Domain.Interfaces.Services;
using TechChallenge.Domain.Model.ViewModel;
using TechChallenge.Domain.Utils;

namespace TechChallenge.Tests.Aplication.ContatoControllerTest
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
        public void AlteraContato_DadosValidos_RetornaNoContent()
        {
            // Arrange
            var contato = new ContatoAlteracaoViewModel
            {
                Id = 1,
                Nome = "Contato Atualizado",
                Telefone = "11988888888",
                IdDDD = 11
            };

            var serviceResult = Result.Success();
            _contatoService.Setup(service => service.Update(contato)).Returns(serviceResult);

            // Act
            var result = _controller.AlteraContato(contato) as NoContentResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(204, result.StatusCode); // No Content
        }

        [Fact]
        public void AlteraContato_DadosInvalidos_RetornaBadRequest()
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
            var result = _controller.AlteraContato(contato) as BadRequestObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(400, result.StatusCode); // Bad Request
            Assert.IsType<SerializableError>(result.Value);
        }

        [Fact]
        public void AlteraContato_ContatoNaoEncontrado_RetornaNotFound()
        {
            // Arrange
            var contato = new ContatoAlteracaoViewModel
            {
                Id = 99, // ID inexistente
                Nome = "Contato Não Encontrado",
                Telefone = "11999999999",
                IdDDD = 99
            };

            var serviceResult = Result.Failure("Contato não encontrado");
            _contatoService.Setup(service => service.Update(contato)).Returns(serviceResult);

            // Act
            var result = _controller.AlteraContato(contato) as NotFoundObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(404, result.StatusCode); // Not Found

            var json = JsonSerializer.Serialize(result.Value);
            dynamic response = JsonSerializer.Deserialize<ExpandoObject>(json);
            Assert.Equal("Contato não encontrado", response.message.GetString());
        }
    }
}
