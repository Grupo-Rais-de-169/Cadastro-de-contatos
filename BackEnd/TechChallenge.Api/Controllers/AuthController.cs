using TechChallenge.Api.Services;
using TechChallenge.Infra.Context;
using TechChallenge.Infra.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using TechChallenge.Domain.Models;

namespace TechChallenge.Api.Controllers
{
    [Route("api/auth")]
    [ApiController]
    [AllowAnonymous]
    public class AuthController : ControllerBase
    {
        private readonly MainContext _mainContext;
        private readonly IConfiguration _configuration;
        private readonly ILogger _logger;
        private readonly TokenServices _tokenServices;

        public AuthController(IConfiguration configuration, ILogger<AuthController> logger, TokenServices tokenServices, MainContext mainContext)
        {
            _configuration = configuration;
            _logger = logger;
            _tokenServices = tokenServices;
            _mainContext = mainContext;
        }


        [HttpPost("login")]
        public ActionResult<dynamic> Login([FromBody] AuthRequestModel authValidate)
        {
            try
            {
                if (authValidate == null)
                {
                    _logger.LogError("Tentativa inválida de acesso");
                    return BadRequest();
                }
                Usuario? user = _tokenServices.GetUserByLoginAndPassword(authValidate.login, authValidate.password);
                if (user == null) 
                {
                    _logger.LogError("Não autorizado");
                    return Unauthorized();
                }
                var token = _tokenServices.GenerateToken(user);
                var refreshToken = _tokenServices.GenerateRefreshToken();
                _tokenServices.SaveRefreshToken(user.Login, refreshToken);
                return Ok(new
                {
                    user.Login,
                    user.Permissao,
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
        public ActionResult<dynamic> Refresh([FromBody] RefreshRequestModel inputRefresh)
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
    }
}
