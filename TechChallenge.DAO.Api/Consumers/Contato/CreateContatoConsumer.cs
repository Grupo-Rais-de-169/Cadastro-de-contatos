using MassTransit;
using TechChallenge.Core.ViewModels;
using TechChallenge.DAO.Api.Services.Interfaces;

namespace TechChallenge.DAO.Api.Consumers.Contato
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
