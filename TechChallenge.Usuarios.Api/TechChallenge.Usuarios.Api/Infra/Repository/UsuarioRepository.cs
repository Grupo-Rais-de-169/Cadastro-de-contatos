using System.Diagnostics.CodeAnalysis;
using TechChallenge.Usuarios.Api.Domain.Entities;
using TechChallenge.Usuarios.Api.Domain.Interfaces;
using TechChallenge.Usuarios.Api.Infra.Context;

namespace TechChallenge.Usuarios.Api.Infra.Repository
{
    [ExcludeFromCodeCoverage]
    public class UsuarioRepository : Repository<Usuario>, IUsuarioRepository
    {
        //private readonly DbConnectionProvider _dbProvider;
        public UsuarioRepository(MainContext context/*, DbConnectionProvider dbProvider*/) : base(context)
        {
            //_dbProvider = dbProvider;
        }
    }
}
