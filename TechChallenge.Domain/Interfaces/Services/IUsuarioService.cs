using TechChallenge.Domain.Model.DTO;
using TechChallenge.Domain.Model.ViewModel;
using TechChallenge.Domain.Utils;

namespace TechChallenge.Domain.Interfaces.Services
{
    public interface IUsuarioService
    {
        Task<IList<UsuarioDTO>> GetAllAsync();
        Task<UsuarioDTO> GetByIdAsync(int id);
        Task<Result> AddAsync(UsuarioInclusaoViewModel contato);
        Result Update(UsuarioAlteracaoViewModel usuarioModel);
        Result Delete(int id);
        bool PerfilExiste(int permissaoId);
    }
}
