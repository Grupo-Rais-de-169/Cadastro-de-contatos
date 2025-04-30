using TechChallenge.Usuarios.Api.Domain.Entities;
using TechChallenge.Usuarios.Api.Domain.Interfaces;

namespace TechChallenge.Usuarios.Api.Application
{
    public class AuthService : IAuthService
    {
        private readonly IAuthRepositories _authRepositories;

        public AuthService(IAuthRepositories AuthRepositories)
        {
            _authRepositories = AuthRepositories;
        }
        public async Task<Usuario?> GetConfirmLoginAndPassword(string username, string password)
        {
            return await _authRepositories.GetConfirmLoginAndPassword(username);
        }
    }
}
