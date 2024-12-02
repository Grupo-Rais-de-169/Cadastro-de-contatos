using TechChallenge.Domain;
using TechChallenge.Domain.Interfaces.Repositories;
using TechChallenge.Infra.Context;

namespace TechChallenge.Infra.Repositories
{
    public class CodigoDeAreaRepository : Repository<CodigoDeArea>, ICodigoDeAreaRepository
    {
        public CodigoDeAreaRepository(MainContext context) : base(context)
        {
        }
    }
}
