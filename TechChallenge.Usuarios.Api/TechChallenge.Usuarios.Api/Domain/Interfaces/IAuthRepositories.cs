using TechChallenge.Usuarios.Api.Domain.Entities;

namespace TechChallenge.Usuarios.Api.Domain.Interfaces
{
    public interface IAuthRepositories
    {
        Task<Usuario?> GetConfirmLoginAndPassword(string username);
    }
}
