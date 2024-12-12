using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Moq;
using System.Text;
using Xunit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Options;
using TechChallenge.Api.Configuration;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace TechChallenge.Tests.Aplication.Configuration
{
    public class JwtConfigurationTests
    {
        [Fact]
        public void AddJwtConfiguration_ShouldConfigureAuthenticationCorrectly()
        {
            // Arrange
            var inMemorySettings = new Dictionary<string, string>
            {
                { "Jwt:Key", "secretkey123456" },
                { "Jwt:Issuer", "TestIssuer" }
            };

            var builder = WebApplication.CreateBuilder(new string[0]);

            // Adiciona a configuração mockada corretamente
            builder.Configuration.AddInMemoryCollection(inMemorySettings);

            // Verificar se o valor da chave "Jwt:Issuer" está correto
            var issuer = builder.Configuration.GetSection("Jwt")["Issuer"];
            Assert.NotNull(issuer);  // A chave não pode ser null
            Assert.Equal("TestIssuer", issuer);  // Verificando se a chave foi lida corretamente

            // Chama o método de configuração do JWT
            builder.AddJwtConfiguration();

            // Act
            var serviceProvider = builder.Services.BuildServiceProvider();

            // Verifica a configuração do JwtBearerOptions
            var options = serviceProvider.GetService<IOptions<JwtBearerOptions>>();
            Assert.NotNull(options);

            var jwtBearerOptions = options.Value;

            // Verificar se as propriedades do TokenValidationParameters foram configuradas corretamente
            Assert.NotNull(jwtBearerOptions.TokenValidationParameters);
            Assert.Equal("Bearer", jwtBearerOptions.Challenge);
            Assert.True(jwtBearerOptions.TokenValidationParameters.ValidateIssuer);
            Assert.True(jwtBearerOptions.RequireHttpsMetadata);
            Assert.True(jwtBearerOptions.SaveToken);
        }

        [Fact]
        public void AddJwtConfiguration_ShouldConfigureJwtAuthentication()
        {
            // Arrange
            var configurationData = new Dictionary<string, string>
            {
                { "Jwt:Key", "my-super-secret-key" },
                { "Jwt:Issuer", "my-issuer" }
            };

            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(configurationData)
                .Build();

            var services = new ServiceCollection();
            var builder = WebApplication.CreateBuilder();
            builder.Configuration.AddConfiguration(configuration);
            builder.Services.Add(services);

            // Act
            JwtConfiguration.AddJwtConfiguration(builder);

            // Assert
            var serviceProvider = builder.Services.BuildServiceProvider();

            var optionsMonitor = serviceProvider.GetService<IOptionsMonitor<JwtBearerOptions>>();
            Assert.NotNull(optionsMonitor);

            var options = optionsMonitor.Get(JwtBearerDefaults.AuthenticationScheme);
            Assert.False(options.RequireHttpsMetadata);
            Assert.True(options.SaveToken);

            var tokenValidationParams = options.TokenValidationParameters;
            Assert.True(tokenValidationParams.ValidateLifetime);
            Assert.True(tokenValidationParams.ValidateIssuerSigningKey);
            Assert.Equal("my-issuer", tokenValidationParams.ValidIssuer);

            var symmetricKey = tokenValidationParams.IssuerSigningKey as SymmetricSecurityKey;
            Assert.NotNull(symmetricKey);
            Assert.Equal(Encoding.ASCII.GetBytes("my-super-secret-key"), symmetricKey.Key);
        }
    }
}