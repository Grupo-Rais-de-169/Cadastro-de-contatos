using AutoMapper;
using Microsoft.Extensions.Caching.Memory;
using TechChallenge.Usuarios.Api.Domain.Entities;
using TechChallenge.Usuarios.Api.Domain.Interfaces;
using TechChallenge.Usuarios.Api.Utils;
using TechChallenge.Usuarios.Api.ViewModels;

namespace TechChallenge.Usuarios.Api.Application
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IMapper _mapper;
        private readonly IMemoryCache _cache;
        private readonly IPasswordService _passwordService;
        private readonly IPermissaoRepository _permissaoRepository;

        public UsuarioService(IUsuarioRepository usuarioRepository,
                              IMapper mapper,
                              IMemoryCache cache,
                              IPasswordService passwordService,
                              IPermissaoRepository permissaoRepository)
        {
            _usuarioRepository = usuarioRepository;
            _mapper = mapper;
            _cache = cache;
            _passwordService = passwordService;
            _permissaoRepository = permissaoRepository;
        }

        public async Task<IList<UsuarioDTO>> GetAllAsync()
        {
            var usuarios = await _cache.GetOrCreateAsync("UsuariosAsync", async entry =>
            {
                entry.AbsoluteExpiration = DateTimeOffset.Now.AddHours(1);
                try
                {
                    return await _usuarioRepository.GetAllAsync();
                }
                catch (Exception)
                {
                    throw;
                }
            });

            return await Task.FromResult(_mapper.Map<List<UsuarioDTO>>(usuarios));
        }

        public async Task<UsuarioDTO> GetByIdAsync(int id) =>

             _mapper.Map<UsuarioDTO>(await _usuarioRepository.GetByIdAsync(id));

        public async Task<Result> AddAsync(UsuarioInclusaoViewModel contato)
        {
            contato.Login = contato.Login.ToLower();
            if (!PerfilExiste(contato.PermissaoId))
                return Result.Failure("O ID da permissão informado não existe.");

            if (ExisteLogin(contato.Login))
                return Result.Failure("Usuário já existente!");

            contato.Senha = _passwordService.GerarHash(contato.Senha);
            var usuarioModel = _mapper.Map<Usuario>(contato);
            await _usuarioRepository.AddAsync(usuarioModel);
            DeletaCache();
            return Result.Success();
        }

        public Result Update(UsuarioAlteracaoViewModel usuarioModel)
        {
            var usuario = _usuarioRepository.GetById(usuarioModel.Id);
            if (usuario == null)
                return Result.Failure("Usuário não encontrado!");
            if (!PerfilExiste(usuarioModel.PermissaoId))
                return Result.Failure("O ID da permissão informado não existe.");

            usuarioModel.Senha = _passwordService.GerarHash(usuarioModel.Senha);

            usuario.Login = usuarioModel.Login;
            usuario.Senha = usuarioModel.Senha;
            usuario.PermissaoId = usuarioModel.PermissaoId;

            _usuarioRepository.Update(usuario);
            DeletaCache();
            return Result.Success();
        }

        public Result Delete(int id)
        {
            var usuario = _usuarioRepository.GetById(id);
            if (usuario == null)
                return Result.Failure("Usuário não encontrado!");
            _usuarioRepository.Delete(id);
            DeletaCache();
            return Result.Success();
        }

        public bool PerfilExiste(int permissaoId) =>
             _permissaoRepository.GetById(permissaoId) != null;

        private bool ExisteLogin(string login) =>
             _usuarioRepository.GetAllAsNoTracking().Where(x => x.Login == login).Any();

        private void DeletaCache() => _cache.Remove("UsuariosAsync");

    }
}
