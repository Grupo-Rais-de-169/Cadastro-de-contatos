using System.Diagnostics.CodeAnalysis;
using TechChallenge.DAO.Api.Context;
using TechChallenge.DAO.Api.Entities;

namespace TechChallenge.DAO.Api.Repository
{
    [ExcludeFromCodeCoverage]
    public class CodigoDeAreaRepository : Repository<CodigoDeArea>, ICodigoDeAreaRepository
    {
        public CodigoDeAreaRepository(MainContext context) : base(context)
        {
        }
    }
}
