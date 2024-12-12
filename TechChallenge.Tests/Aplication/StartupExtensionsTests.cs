using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TechChallenge.Api.Services;
using TechChallenge.Api;
using TechChallenge.Domain.Interfaces.Services;

namespace TechChallenge.Tests.Aplication
{
    public class StartupExtensionsTests
    {
        [Fact]
        public void ConfigureServices_ShouldRegisterDependencies()
        {
            var builder = WebApplication.CreateBuilder();
            builder.ConfigureServices();

            using var app = builder.Build();
            var services = app.Services;

            Assert.NotNull(services.GetService<IAuthService>());
            Assert.NotNull(services.GetService<ITokenServices>());
            Assert.NotNull(services.GetService<IMapper>());
        }

        [Fact]
        public void ConfigureMiddleware_ShouldConfigureApplication()
        {
            var builder = WebApplication.CreateBuilder();
            builder.ConfigureServices();

            using var app = builder.Build();
            app.ConfigureMiddleware();

            Assert.NotNull(app);
        }
    }

}

