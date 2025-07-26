using TechChallenge.Cadastro.Api.Model;
using TechChallenge.Cadastro.Api.Utils;
using TechChallenge.Core.ViewModels;

namespace TechChallenge.Cadastro.Api.Services.Interfaces
{
    public interface IContatoService
    {
        Task<IEnumerable<Contato>> GetContatoByDDD(int id);
        Task<IEnumerable<Contato>> GetAllAsync();
        Task<Result> AddAsync(ContatoInclusaoViewModel contato);
        Task<Result> UpdateAsync(ContatoAlteracaoViewModel contatoModel);
        Task<Result> DeleteAsync(ContatoExclusaoViewModel contato);
        Task<Contato?> GetContatoById(int id);
        Task<CodigoDeArea?> GetDDDById(int id);
    }
}
