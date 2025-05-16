using System.Diagnostics.CodeAnalysis;
using TechChallenge.Usuarios.Api.Configuration;

namespace TechChallenge.Usuarios.Api
{
    [ExcludeFromCodeCoverage]
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.ConfigureServices();

            var app = builder.Build();

            app.ConfigureMiddleware();

            await app.RunAsync();
        }
    }
}