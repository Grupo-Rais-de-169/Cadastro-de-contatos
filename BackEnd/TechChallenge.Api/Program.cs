using API.Contatos.Configuration;
using API.Contatos.Services;
using Infra.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using NLog.Web;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        #region [DB]
        string postgresConnectionString = builder.Configuration.GetConnectionString("PostgreSQL");
        builder.Services.AddDbContextFactory<MainContext>(options => options.UseNpgsql(postgresConnectionString));
        #endregion

        builder.Services.AddMemoryCache();
        builder.Services.AddControllers();

        #region [DI]
        builder.Services
            .AddScoped<TokenServices>();
        #endregion

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerConfiguration();
        builder.AddJwtConfiguration();

        builder.Logging.ClearProviders();
        builder.Host.UseNLog();

        var app = builder.Build();
        var cache = app.Services.GetRequiredService<IMemoryCache>();

        cache.Set("key", "value", new MemoryCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(8)
        });

        app.UseSwaggerConfiguration();
        app.UseCors(x => x
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}
