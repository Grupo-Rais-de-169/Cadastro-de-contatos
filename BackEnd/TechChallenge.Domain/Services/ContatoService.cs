using AutoMapper;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechChallenge.Domain.Interfaces.Repositories;
using TechChallenge.Domain.Interfaces.Services;
using TechChallenge.Domain.Model;

namespace TechChallenge.Domain.Services
{
    public class ContatoService : IContatoService
    {
        private readonly IContatosRepository _contatosRepository;
        private readonly IMapper _mapper;

        public ContatoService(IContatosRepository ContatosRepository, IMapper Mapper)
        {
            _contatosRepository = ContatosRepository;
            _mapper = Mapper;
        }
        public async Task<IEnumerable<CodigoDeArea>> GetAllDDD(int? id = null)
        {
            return await _contatosRepository.GetAllDDD(id);
        }
        public async Task<IEnumerable<ContatoDTO>> GetContatoByDDD(int id)
        {
            return _mapper.Map<IEnumerable<ContatoDTO>>(await _contatosRepository.GetContatoByDDD(id));
        }
        public IList<ContatoDTO> GetAll()
        {
            return _mapper.Map<List<ContatoDTO>>(_contatosRepository.GetAll());
        }

        public async Task<IList<ContatoDTO>> GetAllAsync()
        {
            return _mapper.Map<List<ContatoDTO>>(await _contatosRepository.GetAllAsync());
        }

        public async Task<ContatoDTO> GetByIdAsync(int id)
        {
            return _mapper.Map<ContatoDTO>(await _contatosRepository.GetByIdAsync(id));
        }
        public ContatoDTO GetById(int id)
        {
            return _mapper.Map<ContatoDTO>(id);
        }
        public async Task AddAsync(Contato contato)
        {
            await _contatosRepository.AddAsync(contato);
        }

        public void Add(Contato contato)
        {
            _contatosRepository.Add(contato);
        }
        public void Update(Contato input)
        {
            var contato = _contatosRepository.GetById(input.Id);
            if (contato == null) 
                throw new Exception("Contato não encontrado!");

            contato.Nome = input.Nome;
            contato.Email = input.Email;
            contato.Telefone = input.Telefone;
            contato.IdDDD = input.IdDDD;

            _contatosRepository.Update(contato);
        }

        public void Delete(int id)
        {
            var contato = _contatosRepository.GetById(id);
            if (contato == null)
                throw new Exception("Contato não encontrado!");
            _contatosRepository.Delete(id);
        }
    }
}
