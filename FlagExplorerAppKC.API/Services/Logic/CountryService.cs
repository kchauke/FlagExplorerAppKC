using FlagExplorerAppKC.API.Helpers;
using FlagExplorerAppKC.API.Models;
using FlagExplorerAppKC.API.Services.Interfaces;
using System.Net.Http;
using System.Text.Json;
using System.Xml.Linq;

namespace FlagExplorerAppKC.API.Services.Logic
{
    public class CountryService : ICountryService
    {
        private readonly HttpClient _httpClient;

        public CountryService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<Country>> GetAllCountriesAsync()
        {
            string response = await _httpClient.GetStringAsync("all");
            JsonSerializerOptions options = new JsonSerializerOptions();
            options.Converters.Add(new CountryJsonConverter());
            return JsonSerializer.Deserialize<IEnumerable<Country>>(response, options) ?? Enumerable.Empty<Country>();
        }

        public async Task<CountryDetails?> GetCountryDetailsAsync(string countryName)
        {
            string response = await _httpClient.GetStringAsync($"name/{countryName}");
            JsonSerializerOptions options = new JsonSerializerOptions();
            options.Converters.Add(new CountryDetailsJsonConverter());
            var countryDetails = JsonSerializer.Deserialize<IEnumerable<CountryDetails>>(response, options);
            return countryDetails?.FirstOrDefault();
        }
    }
}
