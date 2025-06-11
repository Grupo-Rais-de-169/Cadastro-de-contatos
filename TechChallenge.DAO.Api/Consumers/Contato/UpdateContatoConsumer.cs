using MassTransit;
using TechChallenge.DAO.Api.Services.Interfaces;
using TechChallenge.DAO.Api.ViewModel;

namespace TechChallenge.DAO.Api.Consumers.Contato
{
    public class UpdateContatoConsumer : IConsumer<ContatoAlteracaoViewModel>
    {
        IContatoService _contatoService;

        public UpdateContatoConsumer(IContatoService contatoService)
        {
            _contatoService = contatoService;
        }
        public Task Consume(ConsumeContext<ContatoAlteracaoViewModel> context)
        {
            var contato = context.Message;
            _contatoService.UpdateAsync(contato);
            return Task.CompletedTask;
        }
    }
}
