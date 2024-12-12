﻿using TechChallenge.Domain.Interfaces.Repositories;
using TechChallenge.Domain;
using TechChallenge.Infra.Context;

namespace TechChallenge.Infra.Repositories
{
    public class PermissaoRepository : Repository<Permissao>, IPermissaoRepository
    {
        public PermissaoRepository(MainContext context) : base(context)
        {
        }
    }
}