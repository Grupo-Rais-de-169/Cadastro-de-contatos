using TechChallenge.Core.ViewModels;

namespace TechChallenge.Cadastro.Api.Producers.Contato
{
    public interface IContatoProducer
    {
        Task ExecuteAsync(ContatoInclusaoViewModel contato);
        Task ExecuteAsync(ContatoAlteracaoViewModel contato);
        Task ExecuteAsync(ContatoExclusaoViewModel contato);
    }
}
