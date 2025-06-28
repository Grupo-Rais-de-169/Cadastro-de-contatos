using MassTransit;
using MassTransit.Middleware;
using TechChallenge.DAO.Worker;
using TechChallenge.DAO.Worker.Consumers;
using TechChallenge.DAO.Worker.Services;
using TechChallenge.DAO.Worker.Services.Interfaces;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        services.AddScoped<IContatoService, ContatoService>();

        var config = hostContext.Configuration;
        var server = config.GetSection("MassTransit")["server"] ?? string.Empty;
        var user = config.GetSection("MassTransit")["user"] ?? string.Empty;
        var password = config.GetSection("MassTransit")["password"] ?? string.Empty;
        var createQueue = config.GetSection("MassTransit")["createQueue"] ?? string.Empty;
        var updateQueue = config.GetSection("MassTransit")["updateQueue"] ?? string.Empty;
        var deleteQueue = config.GetSection("MassTransit")["deleteQueue"] ?? string.Empty;

        services.AddHostedService<Worker>();

        services.AddMassTransit(x =>
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

                    e.ConfigureDeadLetter(x =>
                    {
                        x.UseFilter(new DeadLetterTransportFilter());
                    });
                });
                //cfg.ReceiveEndpoint(updateQueue, e =>
                //{
                //    e.Consumer<UpdateContatoConsumer>(context);
                //});
                //cfg.ReceiveEndpoint(deleteQueue, e =>
                //{
                //    e.Consumer<DeleteContatoConsumer>(context);
                //});

                cfg.ConfigureEndpoints(context);
            });

            //x.AddConsumer<CreateContatoConsumer>();
            //x.AddConsumer<UpdateContatoConsumer>();
            //x.AddConsumer<DeleteContatoConsumer>();
        });
    })
    .Build();

host.Run();