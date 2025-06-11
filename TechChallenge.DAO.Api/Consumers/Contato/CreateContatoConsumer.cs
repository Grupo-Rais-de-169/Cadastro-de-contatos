using MassTransit;
using TechChallenge.DAO.Api.Services.Interfaces;
using TechChallenge.DAO.Api.ViewModel;

namespace TechChallenge.DAO.Api.Consumers.Contato
{
    public class CreateContatoConsumer : IConsumer<ContatoInclusaoViewModel>
    {
        private readonly IContatoService _contatoService;

        public CreateContatoConsumer(IContatoService contatoService)
        {
            _contatoService = contatoService;
        }
        public Task Consume(ConsumeContext<ContatoInclusaoViewModel> context)
        {
            var contatoModel = context.Message;

            _contatoService.AddAsync(contatoModel);
            return Task.CompletedTask;
        }
    }
}
