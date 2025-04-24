using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Diagnostics.CodeAnalysis;
using TechChallenge.DAO.Api;
using TechChallenge.DAO.Api.Context;
using TechChallenge.DAO.Api.Repository;
using TechChallenge.DAO.Domain.Config;

namespace TechChallenge.Cadastro.Api.Configuration
{
    [ExcludeFromCodeCoverage]
    public static class StartupExtensions
    {
        public static WebApplicationBuilder ConfigureServices(this WebApplicationBuilder builder)
        {
            builder.Services.AddDbContextFactory<MainContext>(options =>
                options.UseNpgsql(builder.Configuration.GetConnectionString("PostgreSQL")));

            builder.Services.AddMemoryCache();
            builder.Services.AddControllers();

            builder.Services
                .AddScoped<IContatosRepository, ContatosRepository>()
                .AddScoped<ICodigoDeAreaRepository, CodigoDeAreaRepository>()
                .AddSingleton<DbConnectionProvider>();

            builder.Services.AddEndpointsApiExplorer();
            //builder.AddJwtConfiguration();

            IMapper mapper = MappingConfig.RegisterMaps().CreateMapper();
            builder.Services.AddSingleton(mapper);
            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Api DAO",
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
                        .WithOrigins("https://127.0.0.1", "https://localhost")
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
                        .WithOrigins("https://meudominio.com")
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials();
                });
            }
        }
    }
}
