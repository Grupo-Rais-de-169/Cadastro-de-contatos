using TechChallenge.Usuarios.Api.Domain.Entities;

namespace TechChallenge.Usuarios.Api.Domain.Interfaces
{
    public interface IAuthService
    {
        Task<Usuario?> GetConfirmLoginAndPassword(string username, string password);
    }
}
