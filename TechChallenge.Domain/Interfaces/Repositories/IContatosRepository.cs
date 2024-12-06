using TechChallenge.Domain.Model;

namespace TechChallenge.Domain.Interfaces.Repositories
{
    public interface IContatosRepository: IRepository<Contato>
    {
        Task<IEnumerable<CodigoDeArea>> GetAllDDD(int? id = null);
        Task<IEnumerable<ContatoDto>> GetContatoByDDD(int id);
    }
}
