using MassTransit;
using TechChallenge.Core.ViewModels;
using TechChallenge.DAO.Worker.Services.Interfaces;

namespace TechChallenge.DAO.Worker.Consumers
{
    public class CreateContatoConsumer : IConsumer<ContatoInclusaoViewModel>
    {
        private readonly IContatoService _contatoService;

        public CreateContatoConsumer(IContatoService contatoService)
        {
            _contatoService = contatoService;
        }
        public async Task Consume(ConsumeContext<ContatoInclusaoViewModel> context)
        {
            var contatoModel = context.Message;
            await _contatoService.AddAsync(contatoModel);
        }
    }
}
