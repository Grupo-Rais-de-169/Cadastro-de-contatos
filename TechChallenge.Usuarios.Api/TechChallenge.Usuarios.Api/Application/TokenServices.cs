using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using TechChallenge.Usuarios.Api.Domain.Entities;
using TechChallenge.Usuarios.Api.Domain.Interfaces;

namespace TechChallenge.Usuarios.Api.Application
{
    public class TokenServices : ITokenServices
    {
        private readonly string _token;
        private readonly List<(string, string)> _refreshTokens = new();
        private readonly IAuthRepositories _authRepositories;
        private readonly IPasswordService _passwordService;


        public TokenServices(IConfiguration config,
                            IAuthRepositories authRepositories,
                            IPasswordService passwordService)
        {
            _token = config["Jwt:Key"] ?? throw new ArgumentException("Jwt:Key está vazia");
            _authRepositories = authRepositories;
            _passwordService = passwordService;
        }

        public string GenerateToken(Usuario validate)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_token);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new (JwtRegisteredClaimNames.Sub, validate.Login.ToString()),
                    new (ClaimTypes.Role, validate.Permissao.Funcao.ToString()),
                    new (JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                }),
                Expires = DateTime.UtcNow.AddHours(2),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public string GenerateToken(IEnumerable<Claim> claims)
        {

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_token);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(2),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)

            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }
        public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_token)),
                ValidateLifetime = false
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var securityToken);

            if (securityToken is not JwtSecurityToken jwtSecurityToken || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            {
                throw new SecurityTokenException("Invalid token");
            }
            return principal;
        }

        public void SaveRefreshToken(string username, string refreshToken)
        {
            _refreshTokens.Add((username, refreshToken));
        }

        public string GetRefreshToken(string username)
        {
            return _refreshTokens.LastOrDefault(x => x.Item1 == username).Item2;
        }

        public void DeleteRefreshToken(string username, string refreshToken)
        {
            var item = _refreshTokens.Find(x => x.Item1 == username && x.Item2 == refreshToken);
            _refreshTokens.Remove(item);
        }

        public Usuario? GetUserByLoginAndPassword(string login, string password)
        {

            var usuario = _authRepositories.GetConfirmLoginAndPassword(login).Result;

            if (usuario != null && !_passwordService.VerificarSenha(password, usuario.Senha))
                return null;

            return usuario;
        }
    }
}
