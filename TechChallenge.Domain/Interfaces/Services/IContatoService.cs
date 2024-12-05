using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechChallenge.Domain.Model;
using TechChallenge.Domain.Model.ViewModel;
using TechChallenge.Domain.Utils;

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
        Task<Result> AddAsync(ContatoInclusaoViewModel contato);
        Result Add(ContatoInclusaoViewModel contato);
        Result Update(ContatoAlteracaoViewModel contato);
        Result Delete(int id);
    }
}
