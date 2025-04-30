using System.Diagnostics.CodeAnalysis;
using TechChallenge.Usuarios.Api.Domain.Entities;
using TechChallenge.Usuarios.Api.Domain.Interfaces;
using TechChallenge.Usuarios.Api.Infra.Context;

namespace TechChallenge.Usuarios.Api.Infra.Repository
{
    [ExcludeFromCodeCoverage]
    public class PermissaoRepository : Repository<Permissao>, IPermissaoRepository
    {
        public PermissaoRepository(MainContext context) : base(context)
        {
        }
    }
}
