using Microsoft.AspNetCore.Mvc;
using TechChallenge.Domain;
using TechChallenge.Domain.Interfaces.Repositories;
using TechChallenge.Domain.Interfaces.Services;
using TechChallenge.Domain.Model;

namespace TechChallenge.Api.Controllers
{
    [ApiController]
    [Route("/[controller]")]
    public class ContatoController : ControllerBase
    {
        private readonly IContatoService _contatoService;
        public ContatoController(IContatoService ContatoService)
        {
            _contatoService = ContatoService;
        }

        [HttpGet("GetContatoPorDDD/{ddd}")]
        public async Task<IActionResult> GetContatoPorDDD(int ddd)
        {
            try
            {
                return Ok(await _contatoService.GetContatoByDDD(ddd));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

        }
    }
}
