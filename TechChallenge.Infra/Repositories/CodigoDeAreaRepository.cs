using System.Diagnostics.CodeAnalysis;
using TechChallenge.Domain;
using TechChallenge.Domain.Interfaces.Repositories;
using TechChallenge.Infra.Context;

namespace TechChallenge.Infra.Repositories
{
    [ExcludeFromCodeCoverage]
    public class CodigoDeAreaRepository : Repository<CodigoDeArea>, ICodigoDeAreaRepository
    {
        public CodigoDeAreaRepository(MainContext context) : base(context)
        {
        }
    }
}
