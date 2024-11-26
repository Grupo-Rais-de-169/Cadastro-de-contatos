using TechChallenge.Infra.Entities;

namespace TechChallenge.Infra.Interfaces
{
    public interface IAuthRepositories
    {
        Task<Usuario?> GetConfirmLoginAndPassword(string username, string password);
    }
}
