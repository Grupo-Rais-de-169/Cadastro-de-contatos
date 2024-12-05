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
            .AddScoped<ICodigoDeAreaRepository, CodigoDeAreaRepository>()
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
builder.Services.AddCors(opt => 
{
    opt.AddDefaultPolicy(builder => 
    {
        builder.WithOrigins("*");
    });
    opt.AddPolicy(name: "EnableAllPolicy", builder =>
    {
        builder.WithOrigins("*");
    });
    opt.AddPolicy(name: "OtherPolicy", builder =>
    {
        builder.AllowAnyOrigin();
    });

});
builder.Services.AddControllers();

        var app = builder.Build();
        var cache = app.Services.GetRequiredService<IMemoryCache>();

        cache.Set("key", "value", new MemoryCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(8)
        });

        app.UseSwaggerConfiguration();
        //app.UseCors(x => x
        //    .AllowAnyOrigin()
        //    .AllowAnyMethod()
        //    .AllowAnyHeader());

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();

        app.Run();
