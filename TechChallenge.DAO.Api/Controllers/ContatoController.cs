using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TechChallenge.DAO.Api.Entities;
using TechChallenge.DAO.Api.Infra.Repository.Interfaces;
using TechChallenge.DAO.Api.Utils;
using TechChallenge.DAO.Api.ViewModel;

namespace TechChallenge.DAO.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    //[Authorize]
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
        /// Cria um novo contato.
        /// </summary>
        /// <param name="contato">Dados do contato a ser criado.</param>
        [HttpPost("CadastraContato")]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        //[Authorize(Roles = "admin")]
        public async Task<IActionResult> CriaContato([FromBody] ContatoInclusaoViewModel contato)
        {
            if (!DDDExiste(contato.IdDDD))
                return BadRequest(Result.Failure("O DDD informado não existe."));

            await _contatoRepository.AddAsync(_mapper.Map<Contato>(contato));
            return Ok(Result.Success());
        }

        /// <summary>
        /// Altera um contato existente.
        /// </summary>
        /// <param name="contato">Dados do contato a serem atualizados.</param>
        [HttpPut("AtualizaContato")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        //[Authorize(Roles = "admin")]
        public async Task<IActionResult> AlteraContato([FromBody] ContatoAlteracaoViewModel contatoModel)
        {
            var contato = _contatoRepository.GetById(contatoModel.Id);
            if (contato == null)
                return NotFound(Result.Failure("Contato não encontrado!"));
            if (!DDDExiste(contatoModel.IdDDD))
                return BadRequest(Result.Failure("O DDD informado não existe."));

            contato = MontarContatoParaEditar(contatoModel, contato);

            _contatoRepository.Update(contato);

            return Ok(Result.Success());
        }

        /// <summary>
        /// Deleta um contato.
        /// </summary>
        /// <param name="id">ID do contato a ser excluído.</param>
        [HttpDelete("DeletaContato/{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        //[Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteContato(int id = -1)
        {
            var contato = _contatoRepository.GetById(id);
            if (contato == null)
                return NotFound(Result.Failure("Contato não encontrado!"));
            _contatoRepository.Delete(id);

            return Ok(Result.Success());
        }

        private bool DDDExiste(int ddd) =>
            _codigoAreaRepository.GetById(ddd) != null;

        private Contato MontarContatoParaEditar(ContatoAlteracaoViewModel contatoModel, Contato contato)
        {
            contato.Nome = contatoModel.Nome;
            contato.Email = contatoModel.Email;
            contato.Telefone = contatoModel.Telefone;
            contato.IdDDD = contatoModel.IdDDD;

            return contato;
        }
    }
}
