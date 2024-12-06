using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using TechChallenge.Api.Controllers;
using System.Dynamic;
using System.Text.Json;
using TechChallenge.Domain.Interfaces.Services;
using TechChallenge.Domain.Model.ViewModel;
using TechChallenge.Domain.Utils;
using System.ComponentModel.DataAnnotations;

namespace TechChallenge.Tests.Aplication.ContatoControllerTest
{
    public class CreateContatoTest
    {
        private readonly Mock<IContatoService> _contatoService;
        private readonly ContatoController _controller;
        private readonly Mock<IConfiguration> _mockConfiguration;

        public CreateContatoTest()
        {
            _contatoService = new Mock<IContatoService>();
            _mockConfiguration = new Mock<IConfiguration>();
            _mockConfiguration.Setup(config => config["Jwt:Key"]).Returns("FakeJwtKey");
            _mockConfiguration.Setup(config => config["Jwt:Issuer"]).Returns("FakeIssuer");
            _controller = new ContatoController(_contatoService.Object);
        }

        [Fact]
        public async Task CriaContato_DadosValidos_RetornaCreated()
        {
            // Arrange
            var contato = new ContatoInclusaoViewModel
            {
                Nome = "Contato Teste",
                Telefone = "11999999999",
                IdDDD = 11
            };

            var serviceResult = Result.Success();

            _contatoService.Setup(service => service.AddAsync(contato)).ReturnsAsync(serviceResult);

            // Act
            var result = await _controller.CriaContato(contato) as CreatedAtActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(201, result.StatusCode);
            Assert.Equal(contato, result.Value);
            Assert.Equal(nameof(_controller.GetContatoPorDDD), result.ActionName);
        }

        [Theory]
        [InlineData("", "11999999999", "teste@teste.com", "11", "O nome é obrigatório.")]
        [InlineData("João", "", "teste@teste.com", "11", "O telefone é obrigatório.")]
        [InlineData("João", "11999999999", "", "11", "O email é obrigatório.")]
        [InlineData("João", "11999999999", "emailinvalido", "11", "O email informado não é válido.")]
        public async Task CriaContato_DadosInvalidos_RetornaBadRequest(string nome, string telefone, string email, string ddd, string mensagemEsperada)
        {
            // Arrange
            var contato = new ContatoInclusaoViewModel
            {
                Nome = nome,
                Telefone = telefone,
                Email = email,
                DDD = ddd
            };

            var validationContext = new ValidationContext(contato);
            var validationResults = new List<ValidationResult>();
            Validator.TryValidateObject(contato, validationContext, validationResults, true);

            foreach (var validationResult in validationResults)
            {
                if (!string.IsNullOrEmpty(validationResult.ErrorMessage))
                    _controller.ModelState.AddModelError(validationResult.MemberNames.First(), validationResult.ErrorMessage);
            }

            // Act
            var result = await _controller.CriaContato(contato) as BadRequestObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(400, result.StatusCode);
            Assert.IsType<SerializableError>(result.Value);

            var serializableError = result.Value as SerializableError;
            var errorMessages = serializableError?.Values.SelectMany(v => v as string[] ?? Array.Empty<string>()).ToList();
            Assert.Contains(mensagemEsperada, errorMessages?.FirstOrDefault());
        }

        [Fact]
        public async Task CriaContato_FalhaAoAdicionarContato_RetornaNotFound()
        {
            //Arrange
            var contato = new ContatoInclusaoViewModel
            {
                Nome = "Contato Teste",
                Telefone = "11999999999",
                IdDDD = 11
            };

            var serviceResult = Result.Failure("O DDD informado não existe.");

            _contatoService.Setup(service => service.AddAsync(contato)).ReturnsAsync(serviceResult);

            // Act
            var result = await _controller.CriaContato(contato) as NotFoundObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(404, result.StatusCode);

            var json = JsonSerializer.Serialize(result.Value);
            dynamic? response = JsonSerializer.Deserialize<ExpandoObject>(json);
            Assert.Equal("O DDD informado não existe.", response?.message.GetString());
        }
    }
}
