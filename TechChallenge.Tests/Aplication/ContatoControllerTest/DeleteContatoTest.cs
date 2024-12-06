using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Dynamic;
using System.Text.Json;
using TechChallenge.Api.Controllers;
using TechChallenge.Domain.Interfaces.Services;
using TechChallenge.Domain.Utils;

namespace TechChallenge.Tests.Aplication.ContatoControllerTest
{
    public class DeleteContatoTest
    {
        private readonly Mock<IContatoService> _contatoService;
        private readonly ContatoController _controller;

        public DeleteContatoTest()
        {
            _contatoService = new Mock<IContatoService>();
            _controller = new ContatoController(_contatoService.Object);
        }

        [Fact]
        public void DeleteContato_IdValido_RetornaNoContent()
        {
            // Arrange
            int id = 1; // ID válido
            var serviceResult = Result.Success();
            _contatoService.Setup(service => service.Delete(id)).Returns(serviceResult);

            // Act
            var result = _controller.DeleteContato(id) as NoContentResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(204, result.StatusCode); // No Content
        }

        [Theory]
        [InlineData(-1)] 
        [InlineData(0)]  
        [InlineData(99)] 
        public void DeleteContato_ContatoNaoEncontrado_RetornaNotFound(int id)
        {
            // Arrange
            //int id = 99; // ID inexistente
            var serviceResult = Result.Failure("Contato não encontrado");

            _contatoService.Setup(service => service.Delete(id)).Returns(serviceResult);

            // Act
            var result = _controller.DeleteContato(id) as NotFoundObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(404, result.StatusCode); // Not Found

            var json = JsonSerializer.Serialize(result.Value);
            dynamic response = JsonSerializer.Deserialize<ExpandoObject>(json);
            Assert.Equal("Contato não encontrado", response.message.GetString());
        }
    }
}
