namespace TechChallenge.Domain.Interfaces.Services
{
    public interface IAuthService
    {
        Task<Usuario?> GetConfirmLoginAndPassword(string username, string password);
    }
}
