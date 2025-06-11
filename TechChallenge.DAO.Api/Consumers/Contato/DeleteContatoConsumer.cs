using MassTransit;
using TechChallenge.DAO.Api.Services.Interfaces;
using TechChallenge.DAO.Api.ViewModel;

namespace TechChallenge.DAO.Api.Consumers.Contato
{
    public class DeleteContatoConsumer : IConsumer<ContatoExclusaoViewModel>
    {
        IContatoService _contatoService;

        public DeleteContatoConsumer(IContatoService contatoService)
        {
            _contatoService = contatoService;
        }

        public Task Consume(ConsumeContext<ContatoExclusaoViewModel> context)
        {
            var contato = context.Message;
            _contatoService.DeleteAsync(contato);
            return Task.CompletedTask;
        }
    }
}
