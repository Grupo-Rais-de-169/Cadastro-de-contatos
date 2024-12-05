using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechChallenge.Domain.Interfaces.Repositories;
using TechChallenge.Domain.Interfaces.Services;

namespace TechChallenge.Domain.Services
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
            return await _authRepositories.GetConfirmLoginAndPassword(username, password);
        }
    }
}
