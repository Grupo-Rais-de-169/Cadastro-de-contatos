using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TechChallenge.Cadastro.Api.Services.Interfaces;
using TechChallenge.Cadastro.Api.ViewModel;

namespace TechChallenge.Cadastro.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    //[Authorize]
    public class ContatoController : ControllerBase
    {
        private readonly IContatoService _contatoService;
        public ContatoController(IContatoService ContatoService)
        {
            _contatoService = ContatoService;
        }
        /// <summary>
        /// Obtém contatos pelo DDD.
        /// </summary>
        /// <param name="ddd">Código DDD para buscar contatos.</param>
        /// <returns>Lista de contatos associados ao DDD informado.</returns>
        [HttpGet("GetAllContatos")]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetContatoAll() => Ok(await _contatoService.GetAllAsync());

        /// <summary>
        /// Obtém contatos pelo DDD.
        /// </summary>
        /// <param name="ddd">Código DDD para buscar contatos.</param>
        /// <returns>Lista de contatos associados ao DDD informado.</returns>
        [HttpGet("GetContatoPorDDD/{ddd}")]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetContatoPorDDD(int ddd)
        {
            var contatos = await _contatoService.GetContatoByDDD(ddd);
            if (!contatos.Any())
                return NotFound(new { message = "Não foi encontrado contatos com o DDD informado" });

            return Ok(contatos);
        }



        /// <summary>
        /// Cria um novo contato.
        /// </summary>
        /// <param name="contato">Dados do contato a ser criado.</param>
        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        //[Authorize(Roles = "admin")]
        public async Task<IActionResult> CriaContato([FromBody] ContatoInclusaoViewModel contato)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _contatoService.AddAsync(contato);

            if (!result.IsSuccess)
            {
                return NotFound(new { message = result.Message });
            }

            return CreatedAtAction(nameof(GetContatoPorDDD), new { ddd = contato.IdDDD }, contato);
        }

        ///// <summary>
        ///// Altera um contato existente.
        ///// </summary>
        ///// <param name="contato">Dados do contato a serem atualizados.</param>
        [HttpPut]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        //[Authorize(Roles = "admin")]
        public async Task<IActionResult> AlteraContato([FromBody] ContatoAlteracaoViewModel contato)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _contatoService.UpdateAsync(contato);

            if (!result.IsSuccess)
            {
                return NotFound(new { message = result.Message });
            }

            return NoContent();
        }

        /// <summary>
        /// Deleta um contato.
        /// </summary>
        /// <param name="id">ID do contato a ser excluído.</param>
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        //[Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteContato(int id = -1)
        {
            var result = await _contatoService.DeleteAsync(id);

            if (!result.IsSuccess)
            {
                return NotFound(new { message = result.Message });
            }

            return NoContent();
        }
    }
}
