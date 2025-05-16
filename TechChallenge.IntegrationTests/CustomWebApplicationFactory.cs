using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using TechChallenge.Cadastro.Api;
using TechChallenge.Cadastro.Api.Services.Interfaces;

namespace TechChallenge.IntegrationTests
{
    public class CustomWebApplicationFactory : WebApplicationFactory<Program>
    {
        public Mock<IContatoService> ContatoServiceMock { get; } = new();

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                // Remove implementações reais, se existirem
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(IContatoService));

                if (descriptor != null)
                    services.Remove(descriptor);

                // Injeta o mock
                services.AddSingleton(ContatoServiceMock.Object);
            });
        }
    }
}
