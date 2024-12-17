using System.Diagnostics.CodeAnalysis;
using TechChallenge.Domain;
using TechChallenge.Domain.Interfaces.Repositories;
using TechChallenge.Infra.Context;

namespace TechChallenge.Infra.Repositories
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