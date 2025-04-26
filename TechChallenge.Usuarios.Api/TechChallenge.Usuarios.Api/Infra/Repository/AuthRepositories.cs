using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;
using TechChallenge.Usuarios.Api.Domain.Entities;
using TechChallenge.Usuarios.Api.Domain.Interfaces;
using TechChallenge.Usuarios.Api.Infra.Context;

namespace TechChallenge.Usuarios.Api.Infra.Repository
{
    public class AuthRepositories : IAuthRepositories
    {
        private readonly MainContext _mainContext;
        public AuthRepositories(MainContext mainContext)
        {
            _mainContext = mainContext;
        }

        [ExcludeFromCodeCoverage]
        public Task<Usuario?> GetConfirmLoginAndPassword(string username)
        {
            try
            {
                var user = _mainContext.Usuarios.Include(c => c.Permissao).FirstOrDefault(u => u.Login == username);
                return Task.FromResult(user);

            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
