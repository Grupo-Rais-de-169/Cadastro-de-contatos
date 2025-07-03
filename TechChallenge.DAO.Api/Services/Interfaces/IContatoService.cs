using TechChallenge.Core.ViewModels;

namespace TechChallenge.DAO.Api.Services.Interfaces
{
    public interface IContatoService
    {
        Task AddAsync(ContatoInclusaoViewModel contato);
        Task UpdateAsync(ContatoAlteracaoViewModel contatoModel);
        Task DeleteAsync(ContatoExclusaoViewModel contato);
    }
}
