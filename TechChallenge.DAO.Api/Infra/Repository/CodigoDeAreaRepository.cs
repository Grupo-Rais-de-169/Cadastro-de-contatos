using System.Diagnostics.CodeAnalysis;
using TechChallenge.DAO.Api.Entities;
using TechChallenge.DAO.Api.Infra.Context;
using TechChallenge.DAO.Api.Infra.Repository.Interfaces;

namespace TechChallenge.DAO.Api.Infra.Repository
{
    [ExcludeFromCodeCoverage]
    public class CodigoDeAreaRepository : Repository<CodigoDeArea>, ICodigoDeAreaRepository
    {
        public CodigoDeAreaRepository(MainContext context) : base(context)
        {
        }
    }
}
