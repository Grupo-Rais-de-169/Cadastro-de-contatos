using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using NLog.Web;
using TechChallenge.Api.Configuration;
using TechChallenge.Api.Services;
using TechChallenge.Domain.Config;
using TechChallenge.Domain.Interfaces.Repositories;
using TechChallenge.Domain.Interfaces.Services;
using TechChallenge.Domain.Services;
using TechChallenge.Infra;
using TechChallenge.Infra.Context;
using TechChallenge.Infra.Repositories;

public class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        #region [DB]
        builder.Services.AddDbContextFactory<MainContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("PostgreSQL")));
        #endregion

        builder.Services.AddMemoryCache();
        builder.Services.AddControllers();

        #region [DI]
        builder.Services
            .AddScoped<TokenServices>()
            .AddScoped<IAuthService, AuthService>()
            .AddScoped<IAuthRepositories, AuthRepositories>()
            .AddScoped<IContatoService, ContatoService>()
            .AddScoped<IContatosRepository, ContatosRepository>()
            .AddSingleton<DbConnectionProvider>();

        #endregion

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerConfiguration();
        builder.AddJwtConfiguration();

        builder.Logging.ClearProviders();
        builder.Host.UseNLog();

        IMapper mapper = MappingConfig.RegisterMaps().CreateMapper();
        builder.Services.AddSingleton(mapper);
        builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

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
