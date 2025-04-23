//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.IdentityModel.Tokens;
//using TechChallenge.Api.Services;
//using TechChallenge.Domain;
//using TechChallenge.Domain.Models;

//namespace TechChallenge.Cadastro.Api.Controllers
//{
//    [Route("api/auth")]
//    [ApiController]
//    [AllowAnonymous]
//    public class AuthController : ControllerBase
//    {

//        private readonly ILogger _logger;
//        private readonly ITokenServices _tokenServices;

//        public AuthController(ILogger<AuthController> logger, ITokenServices tokenServices)
//        {
//            _logger = logger;
//            _tokenServices = tokenServices;
//        }

//        [HttpPost("login")]
//        public ActionResult<dynamic> Login([FromBody] AuthRequestModel? authValidate)
//        {
//            try
//            {
//                if (authValidate == null)
//                {
//                    _logger.LogError("Tentativa inválida de acesso");
//                    return BadRequest();
//                }
//                Usuario? user = _tokenServices.GetUserByLoginAndPassword(authValidate.login, authValidate.password);
//                if (user == null) 
//                {
//                    _logger.LogError("Não autorizado");
//                    return Unauthorized();
//                }
//                var token = _tokenServices.GenerateToken(user);
//                var refreshToken = _tokenServices.GenerateRefreshToken();
//                _tokenServices.SaveRefreshToken(user.Login, refreshToken);
//                return Ok(new
//                {
//                    user.Login,
//                    user.Permissao,
//                    token,
//                    refreshToken,
//                    create = DateTime.Now.ToString("g"),
//                    validate = DateTime.Now.AddHours(2).ToString("g")
//                });

//            }
//            catch (Exception ex)
//            {
//                _logger.LogError("Erro ao efetuar o Login: {ErrorMessage}", ex.Message);
//                return BadRequest(ex.Message);
//            }
//        }

//        [HttpPost]
//        [Route("refresh")]
//        public ActionResult<dynamic> Refresh([FromBody] RefreshRequestModel inputRefresh)
//        {
//            try
//            {
//                var principal = _tokenServices.GetPrincipalFromExpiredToken(inputRefresh.token);
//                var username = principal.Claims.ElementAt(0).Value;
//                var savedRefreshToken = _tokenServices.GetRefreshToken(username);
//                if (savedRefreshToken != inputRefresh.refreshToken)
//                {
//                    throw new SecurityTokenException("Inválido Refresh");
//                }

//                var newJwtToken = _tokenServices.GenerateToken(principal.Claims);
//                var newRefreshToken = _tokenServices.GenerateRefreshToken();
//                _tokenServices.DeleteRefreshToken(username, inputRefresh.refreshToken);
//                _tokenServices.SaveRefreshToken(username, newRefreshToken);

//                return new
//                {
//                    token = newJwtToken,
//                    refreshToken = newRefreshToken,
//                    create = DateTime.Now.ToString("g"),
//                    validate = DateTime.Now.AddHours(2).ToString("g")
//                };
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError("Erro ao efetuar o Refresh: {ErrorMessage}", ex.Message);
//                return BadRequest(ex.Message);
//            }
//        }
//    }
//}
