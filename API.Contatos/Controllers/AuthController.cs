using API.Contatos.Models;
using API.Contatos.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace API.Contatos.Controllers
{
    [Route("api")]
    [ApiController]
    [AllowAnonymous]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger _logger;
        private readonly TokenServices _tokenServices;

        public AuthController(IConfiguration configuration, ILogger<AuthController> logger, TokenServices tokenServices)
        {
            _configuration = configuration;
            _logger = logger;
            _tokenServices = tokenServices;
        }


        [HttpPost("login")]
        public ActionResult<dynamic> Login([FromBody] AuthValidateModel authValidate)
        {
            try
            {
                if (authValidate == null)
                {
                    _logger.LogError("Tentativa inválida de acesso");
                    return BadRequest();
                }
                var user = _tokenServices.GetUser(authValidate.login, authValidate.senha);
                if (user == null) 
                {
                    _logger.LogError("Não autorizado");
                    return Unauthorized();
                }
                var token = _tokenServices.GenerateToken(user);
                var refreshToken = _tokenServices.GenerateRefreshToken();
                _tokenServices.SaveRefreshToken(user.login, refreshToken);
                return Ok(new
                {
                    user.login,
                    user.permissao,
                    token,
                    refreshToken,
                    create = DateTime.Now.ToString("g"),
                    validate = DateTime.Now.AddHours(2).ToString("g")
                });

            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro ao efetuar o Login: {ex.Message}");
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("refresh")]
        public ActionResult<dynamic> Refresh([FromBody] InputRefreshModel inputRefresh)
        {
            try
            {
                var principal = _tokenServices.GetPrincipalFromExpiredToken(inputRefresh.token);
                var username = principal.Claims.ElementAt(0).Value;
                var savedRefreshToken = _tokenServices.GetRefreshToken(username);
                if (savedRefreshToken != inputRefresh.refreshToken)
                {
                    _logger.LogError("Inválido Refresh");
                    throw new SecurityTokenException("Inválido Refresh");
                }

                var newJwtToken = _tokenServices.GenerateToken(principal.Claims);
                var newRefreshToken = _tokenServices.GenerateRefreshToken();
                _tokenServices.DeleteRefreshToken(username, inputRefresh.refreshToken);
                _tokenServices.SaveRefreshToken(username, newRefreshToken);

                return new
                {
                    token = newJwtToken,
                    refreshToken = newRefreshToken,
                    create = DateTime.Now.ToString("g"),
                    validate = DateTime.Now.AddHours(2).ToString("g")
                };
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro ao efetuar o Refresh: {ex.Message}");
                return BadRequest(ex.Message);
            }
        }

        /*private string GenerateToken(string username, string role)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, username),
                new Claim(ClaimTypes.Role, role),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }*/

    }
}
