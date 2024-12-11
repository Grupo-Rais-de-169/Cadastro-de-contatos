using AutoMapper;
using Microsoft.Extensions.Caching.Memory;
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
        private readonly IMemoryCache _cache;

        public ContatoService(IMapper Mapper, IContatosRepository ContatosRepository, ICodigoDeAreaRepository codigoAreaRepository, IMemoryCache cache)
        {
            _mapper = Mapper;
            _contatosRepository = ContatosRepository;            
            _codigoAreaRepository = codigoAreaRepository;
            _cache = cache;
        }
        public async Task<IEnumerable<CodigoDeArea>> GetAllDDD(int? id = null)
        {
            return await _contatosRepository.GetAllDDD(id);
        }
        public async Task<IEnumerable<ContatoDto>> GetContatoByDDD(int id)
        {
            return _mapper.Map<IEnumerable<ContatoDto>>(await _contatosRepository.GetContatoByDDD(id));
        }
        public IList<ContatoDto> GetAll()
        {
            var contatos = _cache.GetOrCreate("Contatos", entry => 
            {
                entry.AbsoluteExpiration = DateTimeOffset.Now.AddHours(1);
                try
                {
                    return _contatosRepository.GetAll();
                }
                catch (Exception)
                {
                    throw;
                }
            });
            return _mapper.Map<List<ContatoDto>>(contatos);

        }

        public async Task<IList<ContatoDto>> GetAllAsync()
        {
            var contatos = _cache.GetOrCreate("ContatosAsync", entry =>
            {
                entry.AbsoluteExpiration = DateTimeOffset.Now.AddHours(1);
                try
                {
                    return _contatosRepository.GetAllAsync();
                }
                catch (Exception)
                {

                    throw;
                }
            });

            return await Task.FromResult(_mapper.Map<List<ContatoDto>>(contatos));
        }

        public async Task<ContatoDto> GetByIdAsync(int id)
        {
            return _mapper.Map<ContatoDto>(await _contatosRepository.GetByIdAsync(id));
        }
        public ContatoDto GetById(int id)
        {
            return _mapper.Map<ContatoDto>(id);
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
        public Result Update(ContatoAlteracaoViewModel contatoModel)
        {
            var contato = _contatosRepository.GetById(contatoModel.Id);
            if (contato == null)
                return Result.Failure("Contato não encontrado!");

            contato.Nome = contatoModel.Nome;
            contato.Email = contatoModel.Email;
            contato.Telefone = contatoModel.Telefone;
            contato.IdDDD = contatoModel.IdDDD;

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
