using TechChallenge.Domain.Interfaces.Repositories;
using TechChallenge.Domain;
using TechChallenge.Infra.Context;
using System.Diagnostics.CodeAnalysis;

namespace TechChallenge.Infra.Repositories
{
    [ExcludeFromCodeCoverage]
    public class PermissaoRepository : Repository<Permissao>, IPermissaoRepository
    {
        public PermissaoRepository(MainContext context) : base(context)
        {
        }
    }
}