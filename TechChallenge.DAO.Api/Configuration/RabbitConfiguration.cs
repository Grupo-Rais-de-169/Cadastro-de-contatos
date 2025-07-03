using MassTransit;
using MassTransit.Middleware;
using System.Diagnostics.CodeAnalysis;
using TechChallenge.DAO.Api.Consumers.Contato;

namespace TechChallenge.DAO.Api.Configuration
{
    [ExcludeFromCodeCoverage]
    public static class RabbitConfiguration
    {
        public static WebApplicationBuilder Configure(this WebApplicationBuilder builder)
        {
            //RabbitMQ
            builder.Services
                .Configure<MassTransitConfig>(builder.Configuration.GetSection("MassTransit"));

            var config = builder.Configuration;
            var server = config.GetSection("MassTransit")["server"] ?? string.Empty;
            var user = config.GetSection("MassTransit")["user"] ?? string.Empty;
            var password = config.GetSection("MassTransit")["password"] ?? string.Empty;
            var createQueue = config.GetSection("MassTransit")["createQueue"] ?? string.Empty;
            var updateQueue = config.GetSection("MassTransit")["updateQueue"] ?? string.Empty;
            var deleteQueue = config.GetSection("MassTransit")["deleteQueue"] ?? string.Empty;

            builder.Services.AddMassTransit(x =>
            {
                x.AddConsumer<CreateContatoConsumer>();
                x.AddConsumer<UpdateContatoConsumer>();
                x.AddConsumer<DeleteContatoConsumer>();

                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host(server, "/", h =>
                    {
                        h.Username(user);
                        h.Password(password);
                    });
                    cfg.ReceiveEndpoint(createQueue, e =>
                    {
                        e.ConfigureConsumeTopology = false;
                        e.Consumer<CreateContatoConsumer>(context);

                        e.ConfigureDeadLetter(x =>
                        {
                            x.UseFilter(new DeadLetterTransportFilter());
                        });
                    });
                    cfg.ReceiveEndpoint(updateQueue, e =>
                    {
                        e.ConfigureConsumeTopology = false;
                        e.Consumer<UpdateContatoConsumer>(context);
                        e.ConfigureDeadLetter(x =>
                        {
                            x.UseFilter(new DeadLetterTransportFilter());
                        });
                    });
                    cfg.ReceiveEndpoint(deleteQueue, e =>
                    {
                        e.ConfigureConsumeTopology = false;
                        e.Consumer<DeleteContatoConsumer>(context);
                        e.ConfigureDeadLetter(x =>
                        {
                            x.UseFilter(new DeadLetterTransportFilter());
                        });
                    });

                });
            });
            return builder;
        }
    }
}
