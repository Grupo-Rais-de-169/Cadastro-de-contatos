using System.Security.Claims;
using TechChallenge.Usuarios.Api.Domain.Entities;

namespace TechChallenge.Usuarios.Api.Domain.Interfaces
{
    public interface ITokenServices
    {
        string GenerateToken(Usuario validate);

        string GenerateToken(IEnumerable<Claim> claims);
        string GenerateRefreshToken();
        public ClaimsPrincipal GetPrincipalFromExpiredToken(string token);

        void SaveRefreshToken(string username, string refreshToken);

        string GetRefreshToken(string username);

        void DeleteRefreshToken(string username, string refreshToken);

        Usuario? GetUserByLoginAndPassword(string login, string password);

    }
}
