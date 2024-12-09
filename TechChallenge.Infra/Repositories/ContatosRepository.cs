﻿using Dapper;
using Microsoft.Extensions.Caching.Memory;
using TechChallenge.Domain;
using TechChallenge.Domain.Interfaces.Repositories;
using TechChallenge.Domain.Model;
using TechChallenge.Infra.Context;
using TechChallenge.Infra.Query;

namespace TechChallenge.Infra.Repositories
{
    public class ContatosRepository : Repository<Contato>, IContatosRepository
    {
        private readonly IMemoryCache _cache;
        private readonly DbConnectionProvider _dbProvider;
        public ContatosRepository(MainContext context, DbConnectionProvider dbProvider, IMemoryCache cache) : base(context)
        {
            _dbProvider = dbProvider;
            _cache = cache;
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
