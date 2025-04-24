using TechChallenge.Cadastro.Api.Model;
using TechChallenge.Cadastro.Api.Utils;
using TechChallenge.Cadastro.Api.ViewModel;

namespace TechChallenge.Cadastro.Api.Services
{
    public interface IContatoService
    {
        Task<IEnumerable<Contato>> GetContatoByDDD(int id);
        Task<IEnumerable<Contato>> GetAllAsync();
        Task<Result> AddAsync(ContatoInclusaoViewModel contato);
        Task<Result> UpdateAsync(ContatoAlteracaoViewModel contatoModel);
        Task<Result> DeleteAsync(int id);
    }
}
