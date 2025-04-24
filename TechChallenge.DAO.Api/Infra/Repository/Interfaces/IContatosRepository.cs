using TechChallenge.DAO.Api.Entities;

namespace TechChallenge.DAO.Api.Infra.Repository.Interfaces
{
    public interface IContatosRepository: IRepository<Contato>
    {
        Task<IEnumerable<CodigoDeArea>> GetAllDDD(int? id = null);
        Task<IEnumerable<Contato>> GetContatoByDDD(int id);
    }
}
