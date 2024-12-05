using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TechChallenge.Domain.Interfaces.Services;
using TechChallenge.Domain.Model;
using TechChallenge.Domain.Model.ViewModel;

namespace TechChallenge.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
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
        [HttpGet("GetContatoPorDDD/{ddd}")]
        [ProducesResponseType(typeof(IEnumerable<ContatoDTO>), 200)] // Sucesso
        [ProducesResponseType(404)] // Nenhum contato encontrado
        public async Task<IActionResult> GetContatoPorDDD(int ddd)
        {
            var contatos = await _contatoService.GetContatoByDDD(ddd);
            if (contatos.Count() == 0)
                return NotFound(new { message = "Não foi encontrado contatos com o DDD informado" });

            return Ok(contatos);
        }

        /// <summary>
        /// Obtém contatos pelo DDD.
        /// </summary>
        /// <param name="ddd">Código DDD para buscar contatos.</param>
        /// <returns>Lista de contatos associados ao DDD informado.</returns>
        [HttpGet("GetAllContatos")]
        [ProducesResponseType(typeof(IEnumerable<ContatoDTO>), 200)] // Sucesso
        [ProducesResponseType(404)] // Nenhum contato encontrado
        public async Task<IActionResult> GetContatoAll() => Ok(await _contatoService.GetAllAsync());

        /// <summary>
        /// Cria um novo contato.
        /// </summary>
        /// <param name="contato">Dados do contato a ser criado.</param>
        [HttpPost]
        [ProducesResponseType(201)] // Criado com sucesso
        [ProducesResponseType(400)] // Dados inválidos
        [Authorize(Roles = "admin")]
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

        /// <summary>
        /// Altera um contato existente.
        /// </summary>
        /// <param name="contato">Dados do contato a serem atualizados.</param>
        [HttpPut]
        [ProducesResponseType(204)] // Alterado com sucesso
        [ProducesResponseType(400)] // Dados inválidos
        [ProducesResponseType(404)] // Contato não encontrado
        [Authorize(Roles = "admin")]
        public IActionResult AlteraContato([FromBody] ContatoAlteracaoViewModel contato)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = _contatoService.Update(contato);

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
        [ProducesResponseType(204)] // Excluído com sucesso
        [ProducesResponseType(404)] // Contato não encontrado
        [ProducesResponseType(400)] // BadRequest
        [Authorize(Roles = "admin")]
        public IActionResult DeleteContato(int id)
        {
            var result = _contatoService.Delete(id);

            if (!result.IsSuccess)
            {
                return NotFound(new { message = result.Message });
            }

            return NoContent();
        }
    }
}
