using Xunit;
using AutoMapper;
using TechChallenge.Domain.Config;
using TechChallenge.Domain.Model.DTO;
using TechChallenge.Domain.Model.ViewModel;
using TechChallenge.Domain.Model;
using TechChallenge.Domain;

namespace TechChallenge.Tests.Aplication.Configuration
{
    public class MappingConfigTests
    {

        [Theory]
        [InlineData(typeof(Contato), typeof(ContatoDto))]
        [InlineData(typeof(ContatoInclusaoViewModel), typeof(Contato))]
        [InlineData(typeof(Usuario), typeof(UsuarioDTO))]
        [InlineData(typeof(Usuario), typeof(UsuarioInclusaoViewModel))]
        public void RegisterMaps_ShouldMapCorrectly(Type sourceType, Type destinationType)
        {
            // Arrange
            var mappingConfig = MappingConfig.RegisterMaps();
            var mapper = mappingConfig.CreateMapper();

            // Act
            var instance = Activator.CreateInstance(sourceType); // Cria uma instância do tipo fonte
            var mappedInstance = mapper.Map(instance, sourceType, destinationType); // Realiza o mapeamento

            // Assert
            Assert.NotNull(mappedInstance); // Garante que a instância mapeada não é nula
            Assert.IsType(destinationType, mappedInstance); // Verifica se o tipo da instância mapeada é o esperado
        }
    }
}