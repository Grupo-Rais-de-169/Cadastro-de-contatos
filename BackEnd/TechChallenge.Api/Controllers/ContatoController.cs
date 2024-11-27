using Microsoft.AspNetCore.Mvc;
using TechChallenge.Domain;
using TechChallenge.Domain.Interfaces.Repositories;
using TechChallenge.Domain.Model;

namespace TechChallenge.Api.Controllers
{
    public class ContatoController : Controller
    {
        private readonly IContatosRepository _repository;

        public ContatoController(IContatosRepository repository)
        {
            _repository = repository;
        }

        [HttpGet("GetContatoPorDDD/{ddd}")]
        public async Task<IEnumerable<ContatoDTO>> GetContatoPorDDD(int ddd)
        {
            return await _repository.GetContatoByDDD(ddd);
        }
    }
}
