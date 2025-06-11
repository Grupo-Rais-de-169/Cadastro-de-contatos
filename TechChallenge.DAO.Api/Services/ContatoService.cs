using AutoMapper;
using TechChallenge.DAO.Api.Entities;
using TechChallenge.DAO.Api.Infra.Repository.Interfaces;
using TechChallenge.DAO.Api.Services.Interfaces;
using TechChallenge.DAO.Api.ViewModel;

namespace TechChallenge.DAO.Api.Services
{
    public class ContatoService : IContatoService
    {
        private readonly IContatosRepository _contatoRepository;
        private readonly ICodigoDeAreaRepository _codigoAreaRepository;
        private readonly IMapper _mapper;

        public ContatoService(
            IContatosRepository contatosRepository,
            ICodigoDeAreaRepository codigoDeAreaRepository,
            IMapper mapper)
        {
            _contatoRepository = contatosRepository;
            _codigoAreaRepository = codigoDeAreaRepository;
            _mapper = mapper;
        }

        public async Task AddAsync(ContatoInclusaoViewModel contato)
        {
            if (DDDExiste(contato.IdDDD))
                await _contatoRepository.AddAsync(_mapper.Map<Contato>(contato));
        }

        public Task DeleteAsync(ContatoExclusaoViewModel contatoModel)
        {
            var contato = _contatoRepository.GetById(contatoModel.Id);
            if (contato != null)
                _contatoRepository.Delete(contatoModel.Id);

            return Task.CompletedTask;
        }

        public Task UpdateAsync(ContatoAlteracaoViewModel contatoModel)
        {
            var contato = _contatoRepository.GetById(contatoModel.Id);
            if (contato != null)
            {
                if (DDDExiste(contatoModel.IdDDD))
                {
                    contato = MontarContatoParaEditar(contatoModel, contato);

                    _contatoRepository.Update(contato);
                }
            }

            return Task.CompletedTask;
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
