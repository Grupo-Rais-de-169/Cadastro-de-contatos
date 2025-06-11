using TechChallenge.Cadastro.Api.ViewModel;

namespace TechChallenge.Cadastro.Api.Producers.Contato
{
    public interface IContatoProducer
    {
        Task ExecuteAsync(ContatoInclusaoViewModel contato);
        Task ExecuteAsync(ContatoAlteracaoViewModel contato);
        Task ExecuteAsync(ContatoExclusaoViewModel contato);
    }
}
