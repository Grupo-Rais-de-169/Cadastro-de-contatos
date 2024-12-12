using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.OpenApi.Models;
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
    .AddScoped<ITokenServices, TokenServices>()
    .AddScoped<IAuthService, AuthService>()
    .AddScoped<IAuthRepositories, AuthRepositories>()
    .AddScoped<IUsuarioService, UsuarioService>()
    .AddScoped<IContatoService, ContatoService>()
    .AddScoped<IPasswordService, PasswordService>()
    .AddScoped<IContatosRepository, ContatosRepository>()
    .AddScoped<ICodigoDeAreaRepository, CodigoDeAreaRepository>()
    .AddScoped<IUsuarioRepository, UsuarioRepository>()
    .AddScoped<IPermissaoRepository, PermissaoRepository>()
    .AddSingleton<DbConnectionProvider>();
#endregion

builder.Services.AddEndpointsApiExplorer();
builder.AddJwtConfiguration();

builder.Logging.ClearProviders();
builder.Host.UseNLog();

IMapper mapper = MappingConfig.RegisterMaps().CreateMapper();
builder.Services.AddSingleton(mapper);
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Api de Contatos",
        Version = "v1",
        Description = "Challenge realizado pelo grupo 13",
        License = new OpenApiLicense
        {
            Name = "MIT",
            Url = new Uri("https://opensource.org/licenses/MIT")
        }
    });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Insira o token JWT no campo. Exemplo: Bearer {seu token}",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        In = ParameterLocation.Header
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] {}
                    }
                });
});



var app = builder.Build();
var cache = app.Services.GetRequiredService<IMemoryCache>();

cache.Set("key", "value", new MemoryCacheEntryOptions
{
    AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(8)
});

app.UseSwagger();
app.UseSwaggerUI();

app.UseReDoc(c =>
{
    c.SpecUrl("/swagger/v1/swagger.json"); // URL do JSON do Swagger
    c.DocumentTitle = "Documentação da API com ReDoc";
    c.RoutePrefix = "redoc"; // ReDoc acessível em /redoc
});
app.UseCors(x => x
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader());

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

await app.RunAsync();