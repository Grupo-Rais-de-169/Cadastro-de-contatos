using Microsoft.OpenApi.Models;

namespace TechChallenge.Api.Configuration
{
    public static class SwaggerConfiguration
    {
    //    public static void AddSwaggerConfiguration(this IServiceCollection services)
    //    {
    //        services.AddSwaggerGen(c =>
    //        {
    //            c.SwaggerDoc("v1", new OpenApiInfo
    //            {
    //                Title = "Api de Contatos",
    //                Version = "v1",
    //                Description = "Challenge realizado pelo grupo 13",
    //                License = new OpenApiLicense
    //                {
    //                    Name = "MIT",
    //                    Url = new Uri("https://opensource.org/licenses/MIT")
    //                }
    //            });

    //            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    //            {
    //                Description = "Insira o token JWT no campo. Exemplo: Bearer {seu token}",
    //                Name = "Authorization",
    //                Type = SecuritySchemeType.ApiKey,
    //                Scheme = "Bearer",
    //                In = ParameterLocation.Header
    //            });

    //            c.AddSecurityRequirement(new OpenApiSecurityRequirement
    //            {
    //                {
    //                    new OpenApiSecurityScheme
    //                    {
    //                        Reference = new OpenApiReference
    //                        {
    //                            Type = ReferenceType.SecurityScheme,
    //                            Id = "Bearer"
    //                        }
    //                    },
    //                    new string[] {}
    //                }
    //            });
    //        });

    //    }

    //    public static void UseSwaggerConfiguration(this IApplicationBuilder app)
    //    {
    //        app.UseSwagger();
    //        app.UseSwaggerUI();

    //        app.UseReDoc(c =>
    //        {
    //            c.SpecUrl("/swagger/v1/swagger.json"); // URL do JSON do Swagger
    //            c.DocumentTitle = "Documentação da API com ReDoc";
    //            c.RoutePrefix = "redoc"; // ReDoc acessível em /redoc
    //        });
    //    }
    }
}
