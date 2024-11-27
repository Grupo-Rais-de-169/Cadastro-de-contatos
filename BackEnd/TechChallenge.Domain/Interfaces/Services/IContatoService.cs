using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechChallenge.Domain.Model;

namespace TechChallenge.Domain.Interfaces.Services
{
    public interface IContatoService
    {
        Task<IEnumerable<CodigoDeArea>> GetAllDDD(int? id = null);
        Task<IEnumerable<ContatoDTO>> GetContatoByDDD(int id);
        Task<IList<ContatoDTO>> GetAllAsync();
        IList<ContatoDTO> GetAll();
        Task<ContatoDTO> GetByIdAsync(int id);
        ContatoDTO GetById(int id);
        Task AddAsync(Contato contato);
        void Add(Contato contato);
        void Update(Contato contato);
        void Delete(int id);
    }
}
