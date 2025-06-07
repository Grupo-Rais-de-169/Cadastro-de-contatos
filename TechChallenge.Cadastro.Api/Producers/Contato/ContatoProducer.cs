
using MassTransit;
using Microsoft.Extensions.Options;
using TechChallenge.Cadastro.Api.Configuration;
using TechChallenge.Cadastro.Api.ViewModel;

namespace TechChallenge.Cadastro.Api.Producers.Contato
{
    public class ContatoProducer : IContatoProducer
    {
        private readonly IBus _bus;
        private readonly string queue;
        public ContatoProducer(IBus bus, IOptions<MassTransitConfig> config)
        {
            _bus = bus;
            queue = config.Value.Queue;
        }
        public async Task Execute(ContatoInclusaoViewModel contato)
        {
            var endpoint = await _bus.GetSendEndpoint(new Uri(queue));
            await endpoint.Send(contato);
        }
    }
}
