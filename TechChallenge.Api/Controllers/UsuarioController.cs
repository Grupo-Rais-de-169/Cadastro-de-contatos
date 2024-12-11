using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TechChallenge.Domain.Interfaces.Services;
using TechChallenge.Domain.Model.DTO;
using TechChallenge.Domain.Model.ViewModel;

namespace TechChallenge.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioService _usuarioService;
        public UsuarioController(IUsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        /// <summary>
        /// Obtém todos os usuários
        /// </summary>
        /// <returns>Lista usuários.</returns>
        [HttpGet("GetAllUsers")]
        [ProducesResponseType(typeof(IEnumerable<UsuarioDTO>), 200)]
        [ProducesResponseType(404)]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> GetAll() => Ok(await _usuarioService.GetAllAsync());

        /// <summary>
        /// Obtém usuário pelo ID.
        /// </summary>
        /// <param name="id">Id do usuário.</param>
        /// <returns>Usuário retornado pelo ID.</returns>
        [HttpGet("GetById/{id}")]
        [ProducesResponseType(typeof(IEnumerable<UsuarioDTO>), 200)]
        [ProducesResponseType(404)]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> GetById(int id)
        {
            var usuario = await _usuarioService.GetByIdAsync(id);
            if (usuario == null)
                return NotFound(new { message = "Não foi encontrado usuário com o Id informado" });

            return Ok(usuario);
        }

        /// <summary>
        /// Cria um novo usuário.
        /// </summary>
        /// <param name="usuario">Dados do usuário a ser criado.</param>
        [HttpPost("Create")]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)] 
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> CriaUsuario([FromBody] UsuarioInclusaoViewModel usuario)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _usuarioService.AddAsync(usuario);

            if (!result.IsSuccess)
            {
                return NotFound(new { message = result.Message });
            }

            return CreatedAtAction(nameof(GetAll), usuario);
        }

        /// <summary>
        /// Altera um usuário existente.
        /// </summary>
        /// <param name="usuario">Dados do usuário a serem atualizados.</param>
        [HttpPut("Update")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)] 
        [ProducesResponseType(404)] 
        [Authorize(Roles = "admin")]
        public IActionResult AlteraUsuario([FromBody] UsuarioAlteracaoViewModel usuario)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = _usuarioService.Update(usuario);

            if (!result.IsSuccess)
            {
                return NotFound(new { message = result.Message });
            }

            return NoContent();
        }

        /// <summary>
        /// Deleta um usuário.
        /// </summary>
        /// <param name="id">ID do usuário a ser excluído.</param>
        [HttpDelete("{id}")]
        [ProducesResponseType(204)] 
        [ProducesResponseType(404)] 
        [ProducesResponseType(400)] 
        [Authorize(Roles = "admin")]
        public IActionResult DeleteUsuario(int id = -1)
        {
            var result = _usuarioService.Delete(id);

            if (!result.IsSuccess)
            {
                return NotFound(new { message = result.Message });
            }

            return NoContent();
        }
    }
}
