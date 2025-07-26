using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Dynamic;
using System.Text.Json;
using TechChallenge.Cadastro.Api.Controllers;
using TechChallenge.Cadastro.Api.Model;
using TechChallenge.Cadastro.Api.Services.Interfaces;
using TechChallenge.Cadastro.Api.Utils;
using TechChallenge.Core.ViewModels;

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
            var contato = new ContatoExclusaoViewModel()
            {
                Id = id
            };
            _contatoService.Setup(service => service.GetContatoById(id)).ReturnsAsync(new Contato());
            _contatoService.Setup(service => service.DeleteAsync(It.Is<ContatoExclusaoViewModel>(c => c.Id == id)))
            .ReturnsAsync(serviceResult);

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
            var serviceResult = new Result { Message = "Contato não encontrado!" };
            var contato = new ContatoExclusaoViewModel();

            _contatoService.Setup(service => service.GetContatoById(id)).ReturnsAsync((Contato?)null);
            _contatoService.Setup(service => service.DeleteAsync(contato)).ReturnsAsync(serviceResult);

            // Act
            var result = await _controller.DeleteContato(id) as NotFoundObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(404, result.StatusCode); // Not Found

            var json = JsonSerializer.Serialize(result.Value);
            dynamic? response = JsonSerializer.Deserialize<ExpandoObject>(json);
            Assert.Equal("Contato não encontrado!", response?.Message.GetString());
        }
    }
}
