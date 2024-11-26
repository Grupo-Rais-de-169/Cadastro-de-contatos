﻿using TechChallenge.Infra.Context;
using TechChallenge.Infra.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Net;
using TechChallenge.Infra.Interfaces;

namespace TechChallenge.Infra.Repositories
{
    public class AuthRepositories: IAuthRepositories
    {
        private readonly MainContext _mainContext;
        public AuthRepositories(MainContext mainContext) 
        { 
            _mainContext = mainContext;
        }

        public Task<Usuario?> GetConfirmLoginAndPassword(string username, string password)
        {
            try
            {
                List<Usuario> users = [.. (from Usuario u in _mainContext.Usuarios select u).Include(u => u.Permissao)];
                return Task.FromResult(users.FirstOrDefault());
                
            }
            catch (Exception ex)
            {
                throw;
            }
            
            
        }
    }
}
