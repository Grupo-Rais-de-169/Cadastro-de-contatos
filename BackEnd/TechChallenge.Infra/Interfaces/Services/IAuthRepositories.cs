using TechChallenge.Infra.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechChallenge.Infra.Interfaces
{
    public interface IAuthRepositories
    {
        Task<Usuario?> GetConfirmLoginAndPassword(string username, string password);
    }
}
