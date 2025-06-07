using TechChallenge.Cadastro.Api.ViewModel;

namespace TechChallenge.Cadastro.Api.Producers.Contato
{
    public interface IContatoProducer
    {
        Task Execute(ContatoInclusaoViewModel contato);
    }
}
