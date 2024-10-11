using Messenger.Shared.Models;
using System.Net.Http.Json;

namespace Messenger.Client.Services
{
    public class MainService<T> : IMainService<T> where T : class

    {

        public HttpClient _httpClient;

        public MainService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }



        public async Task<List<T>> GetAll(string apiname)
        {
            return await _httpClient.GetFromJsonAsync<List<T>>(apiname);
        }

        public async Task<T> GetRow(string apiname)
        {
            return await _httpClient.GetFromJsonAsync<T>(apiname);
        }



    }

}