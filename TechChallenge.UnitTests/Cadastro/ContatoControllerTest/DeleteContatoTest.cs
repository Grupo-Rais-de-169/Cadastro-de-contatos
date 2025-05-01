using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Dynamic;
using System.Text.Json;
using TechChallenge.Cadastro.Api.Controllers;
using TechChallenge.Cadastro.Api.Services.Interfaces;
using TechChallenge.Cadastro.Api.Utils;

namespace TechChallenge.UnitTest
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
        public async Task DeleteContato_IdValido_RetornaNoContent()
        {
            // Arrange
            int id = 1; // ID válido
            var serviceResult = new Result { IsSuccess = true };
            _contatoService.Setup(service => service.DeleteAsync(id)).ReturnsAsync(serviceResult);

            // Act
            var result = await _controller.DeleteContato(id) as NoContentResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(204, result.StatusCode); // No Content
        }

        [Theory]
        [InlineData(-1)] 
        [InlineData(0)]  
        [InlineData(99)] 
        public async Task DeleteContato_ContatoNaoEncontrado_RetornaNotFound(int id)
        {
            // Arrange
            //int id = 99; // ID inexistente
            var serviceResult = new Result { Message = "Contato não encontrado" };

            _contatoService.Setup(service => service.DeleteAsync(id)).ReturnsAsync(serviceResult);

            // Act
            var result = await _controller.DeleteContato(id) as NotFoundObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(404, result.StatusCode); // Not Found

            var json = JsonSerializer.Serialize(result.Value);
            dynamic? response = JsonSerializer.Deserialize<ExpandoObject>(json);
            Assert.Equal("Contato não encontrado", response?.message.GetString());
        }
    }
}
