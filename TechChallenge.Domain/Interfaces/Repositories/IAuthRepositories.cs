using TechChallenge.Domain;

namespace TechChallenge.Domain.Interfaces.Repositories
{
    public interface IAuthRepositories
    {
        Task<Usuario?> GetConfirmLoginAndPassword(string username, string password);
    }
}
