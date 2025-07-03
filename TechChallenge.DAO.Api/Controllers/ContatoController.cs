using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TechChallenge.DAO.Api.Entities;
using TechChallenge.DAO.Api.Infra.Repository.Interfaces;

namespace TechChallenge.DAO.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ContatoController : ControllerBase
    {
        private readonly IContatosRepository _contatoRepository;
        private readonly ICodigoDeAreaRepository _codigoAreaRepository; 
        private readonly IMapper _mapper;
        public ContatoController(IContatosRepository ContatoRepository,
                                 ICodigoDeAreaRepository codigoAreaRepository,
                                 IMapper mapper)
        {
            _contatoRepository = ContatoRepository;
            _codigoAreaRepository = codigoAreaRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// Obtém contatos pelo DDD.
        /// </summary>
        /// <param name="ddd">Código DDD para buscar contatos.</param>
        /// <returns>Lista de contatos associados ao DDD informado.</returns>
        [HttpGet("GetAllContatos")]
        [ProducesResponseType(typeof(IEnumerable<Contato>), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetContatoAll() => Ok(await _contatoRepository.GetAllAsync());

        /// <summary>
        /// Obtém contatos pelo DDD.
        /// </summary>
        /// <param name="ddd">Código DDD para buscar contatos.</param>
        /// <returns>Lista de contatos associados ao DDD informado.</returns>
        [HttpGet("GetContatoPorDDD/{ddd}")]
        [ProducesResponseType(typeof(IEnumerable<Contato>), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetContatoPorDDD(int ddd)
        {
            var contatos = await _contatoRepository.GetContatoByDDD(ddd);
            if (!contatos.Any())
                return NotFound(new { message = "Não foi encontrado contatos com o DDD informado" });

            return Ok(contatos);
        }

        /// <summary>
        /// Obtém Modelo do DDD.
        /// </summary>
        /// <param name="ddd">Código DDD para buscar contatos.</param>
        /// <returns>DDD com o id informado</returns>
        [HttpGet("GetDDDById/{id}")]
        [ProducesResponseType(typeof(CodigoDeArea), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetDDDById(int id)
        {            
            return Ok(await _codigoAreaRepository.GetByIdAsync(id));
        }

        /// <summary>
        /// Obtém contatos pelo DDD.
        /// </summary>
        /// <param name="ddd">Código DDD para buscar contatos.</param>
        /// <returns>Lista de contatos associados ao DDD informado.</returns>
        [HttpGet("GetContatoById/{id}")]
        [ProducesResponseType(typeof(Contato), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetContatoById(int id)
        {
            return Ok(await _contatoRepository.GetByIdAsync(id));
        } 

    }
}
