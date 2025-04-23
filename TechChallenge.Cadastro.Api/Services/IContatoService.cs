using TechChallenge.Cadastro.Api.Model;

namespace TechChallenge.Cadastro.Api.Services
{
    public interface IContatoService
    {
        //Task<IEnumerable<CodigoDeArea>> GetAllDDD(int? id = null);
        Task<IEnumerable<Contato>> GetContatoByDDD(int id);
        //Task<IList<ContatoDto>> GetAllAsync();
        //IList<ContatoDto> GetAll();
        //Task<ContatoDto> GetByIdAsync(int id);
        //ContatoDto GetById(int id);
        //Task<Result> AddAsync(ContatoInclusaoViewModel contato);
        //Result Update(ContatoAlteracaoViewModel contatoModel);
        //Result Delete(int id);
        //bool DDDExiste(int ddd);
        //void DeletaCache();
        //Contato MontarContatoParaEditar(ContatoAlteracaoViewModel contatoModel, Contato contato);
    }
}
