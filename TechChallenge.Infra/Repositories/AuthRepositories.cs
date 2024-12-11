using Microsoft.EntityFrameworkCore;
using TechChallenge.Domain;
using TechChallenge.Domain.Interfaces.Repositories;
using TechChallenge.Infra.Context;

namespace TechChallenge.Infra.Repositories
{
    public class AuthRepositories : IAuthRepositories
    {
        private readonly MainContext _mainContext;
        public AuthRepositories(MainContext mainContext)
        {
            _mainContext = mainContext;
        }

        public Task<Usuario?> GetConfirmLoginAndPassword(string username)
        {
            try
            {
                var user = _mainContext.Usuarios.Include(c=>c.Permissao).FirstOrDefault(u => u.Login == username);
                return Task.FromResult(user);

            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
