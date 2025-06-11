using MassTransit;
using Microsoft.Extensions.Options;
using TechChallenge.Cadastro.Api.Configuration;
using TechChallenge.Cadastro.Api.ViewModel;

namespace TechChallenge.Cadastro.Api.Producers.Contato
{
    public class ContatoProducer : IContatoProducer
    {
        private readonly IBus _bus;
        private readonly string _createQueue;
        private readonly string _updateQueue;
        private readonly string _deleteQueue;
        public ContatoProducer(IBus bus, IOptions<MassTransitConfig> config)
        {
            _bus = bus;
            _createQueue = config.Value.CreateQueue;
            _updateQueue = config.Value.UpdateQueue;
            _deleteQueue = config.Value.DeleteQueue;
        }
        public async Task ExecuteAsync(ContatoInclusaoViewModel contato)
        {
            var uriQueue = "queue:" + _createQueue;
            var endpoint = await _bus.GetSendEndpoint(new Uri(uriQueue));
            await endpoint.Send(contato);
        }

        public async Task ExecuteAsync(ContatoAlteracaoViewModel contato)
        {
            var uriQueue = "queue:" + _updateQueue;
            var endpoint = await _bus.GetSendEndpoint(new Uri(uriQueue));
            await endpoint.Send(contato);
        }

        public async Task ExecuteAsync(ContatoExclusaoViewModel contato)
        {
            var uriQueue = "queue:" + _deleteQueue;
            var endpoint = await _bus.GetSendEndpoint(new Uri(uriQueue));
            await endpoint.Send(contato);
        }
    }
}
