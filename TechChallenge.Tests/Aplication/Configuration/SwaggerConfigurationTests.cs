using Moq;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Mvc;
using TechChallenge.Api.Configuration;
using Xunit;
using Swashbuckle.AspNetCore.ReDoc;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.Swagger;

namespace TechChallenge.Tests.Aplication.Configuration
{
    public class SwaggerConfigurationTests
    {
        [Fact]
        public void AddSwaggerConfiguration_ShouldAddSwaggerGen()
        {
            // Arrange
            var services = new ServiceCollection();

            // Act
            SwaggerConfiguration.AddSwaggerConfiguration(services);

            // Assert
            var serviceProvider = services.BuildServiceProvider();
            Assert.NotNull(serviceProvider);
        }
    }
}
