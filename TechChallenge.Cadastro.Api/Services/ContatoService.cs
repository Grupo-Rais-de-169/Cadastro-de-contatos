using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Text;
using System.Text.Json;
using TechChallenge.Cadastro.Api.Configuration;
using TechChallenge.Cadastro.Api.Model;
using TechChallenge.Cadastro.Api.Services.Interfaces;
using TechChallenge.Cadastro.Api.Utils;
using TechChallenge.Cadastro.Api.ViewModel;

namespace TechChallenge.Cadastro.Api.Services
{
    public class ContatoService : IContatoService
    {
        private readonly IMemoryCache _cache;
        private readonly HttpClient _httpClient; 
        private readonly string _urlDAO;

        public ContatoService(HttpClient httpClient,
                              IOptions<MicroservicoConfig> config,
                              IMemoryCache cache)
        {
            _httpClient = httpClient;
            _cache = cache;
            _urlDAO = config.Value.DAO;
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


        //public async Task<ContatoDto> GetByIdAsync(int id) =>
        //     _mapper.Map<ContatoDto>(await _contatosRepository.GetByIdAsync(id));

        //public ContatoDto GetById(int id) => _mapper.Map<ContatoDto>(id);

        public async Task<Result> AddAsync(ContatoInclusaoViewModel contato)
        {
            var url = $"{_urlDAO}CadastraContato/";
            var json = JsonConvert.SerializeObject(contato);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            DeletaCache();
            var response = await _httpClient.PostAsync(url, content);

            var responseBody = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<Result>(responseBody);
            return result;
        }

        public async Task<Result> UpdateAsync(ContatoAlteracaoViewModel contatoModel)
        {
            var url = $"{_urlDAO}AtualizaContato/";
            var json = JsonConvert.SerializeObject(contatoModel);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            DeletaCache();
            var response = await _httpClient.PutAsync(url, content);

            var responseBody = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<Result>(responseBody);
            return result;
        }

        public async Task<Result> DeleteAsync(int id)
        {
            var url = $"{_urlDAO}DeletaContato/{id}";

            DeletaCache();

            var response = await _httpClient.DeleteAsync(url);

            var responseBody = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<Result>(responseBody);
            return result;
        }


        public void DeletaCache()
        {
            _cache.Remove("Contatos");
            _cache.Remove("ContatosAsync");
        }
    }
}
