using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using TechChallenge.Cadastro.Api;
using TechChallenge.Cadastro.Api.Services.Interfaces;
using TechChallenge.DAO.Api.Infra.Repository.Interfaces;

namespace TechChallenge.IntegrationTests
{
    public class CustomWebApplicationFactory : WebApplicationFactory<Program>
    {
        public Mock<IContatoService> ContatoServiceMock { get; } = new();
        public Mock<IContatosRepository> ContatosRepositoryMock { get; } = new();
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseSetting("https_port", "0"); // Isso garante que ele use uma porta dinâmica
            builder.UseEnvironment("Testing");
            builder.UseSetting("urls", "http://127.0.0.1:0");
            builder.ConfigureLogging(logging =>
            {
                logging.ClearProviders();
            });

            builder.ConfigureServices(services =>
            {
                
                var contatoServiceDescriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(IContatoService));
                if (contatoServiceDescriptor != null)
                    services.Remove(contatoServiceDescriptor);

                var contatosRepoDescriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(IContatosRepository));
                if (contatosRepoDescriptor != null)
                    services.Remove(contatosRepoDescriptor);

                services.AddSingleton(ContatoServiceMock.Object);
                services.AddSingleton(ContatosRepositoryMock.Object);
            });
        }
    }
}
