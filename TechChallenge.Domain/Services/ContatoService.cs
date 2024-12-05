using AutoMapper;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using TechChallenge.Domain.Interfaces.Repositories;
using TechChallenge.Domain.Interfaces.Services;
using TechChallenge.Domain.Model;
using TechChallenge.Domain.Model.ViewModel;
using TechChallenge.Domain.Utils;

namespace TechChallenge.Domain.Services
{
    public class ContatoService : IContatoService
    {
        private readonly IContatosRepository _contatosRepository;
        private readonly ICodigoDeAreaRepository _codigoAreaRepository;
        private readonly IMapper _mapper;

        public ContatoService(IMapper Mapper, IContatosRepository ContatosRepository, ICodigoDeAreaRepository codigoAreaRepository)
        {
            _mapper = Mapper;
            _contatosRepository = ContatosRepository;            
            _codigoAreaRepository = codigoAreaRepository;
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
        public async Task<Result> AddAsync(ContatoInclusaoViewModel contato)
        {
            if(!DDDExiste(contato.IdDDD))
                return Result.Failure("O DDD informado não existe.");

            await _contatosRepository.AddAsync(_mapper.Map<Contato>(contato));
            return Result.Success();
        }

        public Result Add(ContatoInclusaoViewModel contato)
        {
            if (!DDDExiste(contato.IdDDD))
                return Result.Failure("O DDD informado não existe.");

             _contatosRepository.AddAsync(_mapper.Map<Contato>(contato));
            return Result.Success();
        }
        public Result Update(ContatoAlteracaoViewModel input)
        {
            var contato = _contatosRepository.GetById(input.Id);
            if (contato == null)
                return Result.Failure("Contato não encontrado!");

            contato.Nome = input.Nome;
            contato.Email = input.Email;
            contato.Telefone = input.Telefone;
            contato.IdDDD = input.IdDDD;

            _contatosRepository.Update(contato);
            return Result.Success();
        }

        public Result Delete(int id)
        {
            var contato = _contatosRepository.GetById(id);
            if (contato == null)
                return Result.Failure("Contato não encontrado!");
            _contatosRepository.Delete(id);
            return Result.Success();
        }

        private bool DDDExiste(int ddd)
        {
           return  _codigoAreaRepository.GetById(ddd) != null;
        }
    }
}
