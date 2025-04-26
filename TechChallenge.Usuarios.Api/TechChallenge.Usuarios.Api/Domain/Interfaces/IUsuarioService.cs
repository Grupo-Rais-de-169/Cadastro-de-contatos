using TechChallenge.Usuarios.Api.Utils;
using TechChallenge.Usuarios.Api.ViewModels;

namespace TechChallenge.Usuarios.Api.Domain.Interfaces
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
