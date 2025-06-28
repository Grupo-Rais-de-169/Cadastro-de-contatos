using MassTransit;
using TechChallenge.DAO.Worker.Services.Interfaces;
using TechChallenge.DAO.Worker.ViewModel;

namespace TechChallenge.DAO.Worker.Consumers
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
