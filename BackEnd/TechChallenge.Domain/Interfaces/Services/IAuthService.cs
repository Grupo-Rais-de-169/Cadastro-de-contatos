using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechChallenge.Domain.Interfaces.Services
{
    public interface IAuthService
    {
        Task<Usuario?> GetConfirmLoginAndPassword(string username, string password);
    }
}
