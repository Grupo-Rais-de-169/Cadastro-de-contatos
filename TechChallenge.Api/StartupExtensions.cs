using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using TechChallenge.Api.Configuration;
using TechChallenge.Api.Services;
using TechChallenge.Domain.Config;
using TechChallenge.Domain.Interfaces.Repositories;
using TechChallenge.Domain.Interfaces.Services;
using TechChallenge.Domain.Services;
using TechChallenge.Infra.Context;
using TechChallenge.Infra.Repositories;
using TechChallenge.Infra;

namespace TechChallenge.Api
{
    public static class StartupExtensions
    {
        public static WebApplicationBuilder ConfigureServices(this WebApplicationBuilder builder)
        {
            builder.Services.AddDbContextFactory<MainContext>(options =>
                options.UseNpgsql(builder.Configuration.GetConnectionString("PostgreSQL")));

            builder.Services.AddMemoryCache();
            builder.Services.AddControllers();

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

            builder.Services.AddEndpointsApiExplorer();
            builder.AddJwtConfiguration();

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
                        Array.Empty<string>()
                    }
                });
            });

            return builder;
        }




        public static WebApplication ConfigureMiddleware(this WebApplication app)
        {
            app.UseSwagger();
            app.UseSwaggerUI();

            app.UseReDoc(c =>
            {
                c.SpecUrl("/swagger/v1/swagger.json");
                c.DocumentTitle = "Documentação da API com ReDoc";
                c.RoutePrefix = "redoc";
            });

            ConfigureCors(app);

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            return app;
        }

        private static void ConfigureCors(WebApplication app)
        {
            if (app.Environment.IsDevelopment())
            {
                app.UseCors(corsPolicy =>
                {
                    corsPolicy
                        .AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials();
                });
            }
            else
            {
                app.UseCors(corsPolicy =>
                {
                    corsPolicy
                        .WithOrigins("https://127.0.0.1", "http://172.0.0.1")
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials();
                });
            }
        }
    }
}
