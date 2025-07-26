using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using TechChallenge.Cadastro.Api.Configuration;
using TechChallenge.Cadastro.Api.Model;
using TechChallenge.Cadastro.Api.Producers.Contato;
using TechChallenge.Cadastro.Api.Services.Interfaces;
using TechChallenge.Cadastro.Api.Utils;
using TechChallenge.Core.ViewModels;

namespace TechChallenge.Cadastro.Api.Services
{
    public class ContatoService : IContatoService
    {
        private readonly IMemoryCache _cache;
        private readonly HttpClient _httpClient; 
        private readonly string _urlDAO;
        private readonly IContatoProducer _contatoProducer;

        public ContatoService(HttpClient httpClient,
                              IOptions<MicroservicoConfig> config,
                              IMemoryCache cache,
                              IContatoProducer contatoProducer)
        {
            _httpClient = httpClient;
            _cache = cache;
            _urlDAO = config.Value.DAO;
            _contatoProducer = contatoProducer;
        }


        public async Task<IEnumerable<Contato>> GetContatoByDDD(int ddd)
        {
            var url = $"{_urlDAO}GetContatoPorDDD/{ddd}";

            var response = await _httpClient.GetAsync(url);

            var content = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<IEnumerable<Contato>>(content);
        }

        public async Task<IEnumerable<Contato>> GetAllAsync()
        {
            var url = $"{_urlDAO}GetAllContatos/";

            return await _cache.GetOrCreateAsync("Contatos", async entry =>
            {
                entry.AbsoluteExpiration = DateTimeOffset.Now.AddHours(1);

                var response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<IEnumerable<Contato>>(content);
            });
        }

        public async Task<Result> AddAsync(ContatoInclusaoViewModel contato)
        {
            DeletaCache();
            await _contatoProducer.ExecuteAsync(contato);
            return Result.Success("Contato adicionado com sucesso");
        }

        public async Task<Result> UpdateAsync(ContatoAlteracaoViewModel contatoModel)
        {
            DeletaCache();
            await _contatoProducer.ExecuteAsync(contatoModel);
            return Result.Success("Contato atualizado com sucesso");
        }

        public async Task<Result> DeleteAsync(ContatoExclusaoViewModel contatoModel)
        {
            DeletaCache();
            await _contatoProducer.ExecuteAsync(contatoModel);
            return Result.Success("Contato removido com sucesso");
        }

        public async Task<Contato?> GetContatoById(int id)
        {
            var url = $"{_urlDAO}GetContatoById/{id}";

            var response = await _httpClient.GetAsync(url);

            var content = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<Contato>(content);
        }
        public async Task<CodigoDeArea?> GetDDDById(int id)
        {
            var url = $"{_urlDAO}GetDDDById/{id}";

            var response = await _httpClient.GetAsync(url);

            var content = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<CodigoDeArea>(content);
        }

        public void DeletaCache()
        {
            _cache.Remove("Contatos");
            _cache.Remove("ContatosAsync");
        }
    }
}
