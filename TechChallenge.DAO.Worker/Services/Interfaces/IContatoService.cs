using TechChallenge.DAO.Worker.Entities;
using TechChallenge.DAO.Worker.ViewModel;

namespace TechChallenge.DAO.Worker.Services.Interfaces
{
    public interface IContatoService
    {
        Task AddAsync(ContatoInclusaoViewModel contato);
        Task UpdateAsync(ContatoAlteracaoViewModel contatoModel);
        Task DeleteAsync(ContatoExclusaoViewModel contato);
    }
}
