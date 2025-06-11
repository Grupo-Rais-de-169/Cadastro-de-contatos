using AutoMapper;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Diagnostics.CodeAnalysis;
using TechChallenge.DAO.Api;
using TechChallenge.DAO.Api.Configuration;
using TechChallenge.DAO.Api.Consumers.Contato;
using TechChallenge.DAO.Api.Infra.Context;
using TechChallenge.DAO.Api.Infra.Repository;
using TechChallenge.DAO.Api.Infra.Repository.Interfaces;
using TechChallenge.DAO.Api.Monitoramento;
using TechChallenge.DAO.Api.Services;
using TechChallenge.DAO.Api.Services.Interfaces;
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
                .AddScoped<IContatoService, ContatoService>()
                .AddSingleton<DbConnectionProvider>()
                .AddSingleton<SystemMetricsCollector>();

            builder.Services.AddEndpointsApiExplorer();
            builder.AddJwtConfiguration();

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

            //RabbitMQ
            builder.Services
                .Configure<MassTransitConfig>(builder.Configuration.GetSection("MassTransit"));

            var config = builder.Configuration;
            var server = config.GetSection("MassTransit")["server"] ?? string.Empty;
            var user = config.GetSection("MassTransit")["user"] ?? string.Empty;
            var password = config.GetSection("MassTransit")["password"] ?? string.Empty;
            var createQueue = config.GetSection("MassTransit")["CreateContato"] ?? string.Empty;
            var updateQueue = config.GetSection("MassTransit")["UpdateContato"] ?? string.Empty;
            var deleteQueue = config.GetSection("MassTransit")["DeleteContato"] ?? string.Empty;

            builder.Services.AddMassTransit(x =>
            {
                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host(server, "/", h =>
                    {
                        h.Username(user);
                        h.Password(password);
                    });
                    cfg.ReceiveEndpoint(createQueue, e =>
                    {
                        e.Consumer<CreateContatoConsumer>(context);
                    });
                    cfg.ReceiveEndpoint(updateQueue, e =>
                    {
                        e.Consumer<UpdateContatoConsumer>(context);
                    });
                    cfg.ReceiveEndpoint(deleteQueue, e =>
                    {
                        e.Consumer<DeleteContatoConsumer>(context);
                    });

                    cfg.ConfigureEndpoints(context);
                });

                x.AddConsumer<CreateContatoConsumer>();
                x.AddConsumer<UpdateContatoConsumer>();
                x.AddConsumer<DeleteContatoConsumer>();
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
