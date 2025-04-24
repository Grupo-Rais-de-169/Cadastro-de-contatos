using Dapper;
using System.Diagnostics.CodeAnalysis;
using TechChallenge.DAO.Api.Entities;
using TechChallenge.DAO.Api.Infra.Context;
using TechChallenge.DAO.Api.Infra.Query;
using TechChallenge.DAO.Api.Infra.Repository.Interfaces;

namespace TechChallenge.DAO.Api.Infra.Repository
{
    [ExcludeFromCodeCoverage]
    public class ContatosRepository : Repository<Contato>, IContatosRepository
    {
        private readonly DbConnectionProvider _dbProvider;
        public ContatosRepository(MainContext context, DbConnectionProvider dbProvider) : base(context)
        {
            _dbProvider = dbProvider;
        }

        public async Task<IEnumerable<CodigoDeArea>> GetAllDDD(int? id = null)
        {
            using var connection = _dbProvider.GetConnection();
            var query = ContatosQuery.ListDDD(id);
            return await connection.QueryAsync<CodigoDeArea>(query, new { Id = id });
        }

        public async Task<IEnumerable<Contato>> GetContatoByDDD(int id)
        {
            using var connection = _dbProvider.GetConnection();
            var query = ContatosQuery.GetContatoByDDD(id);
            return await connection.QueryAsync<Contato>(query, new { Id = id });
        }
    }
}
