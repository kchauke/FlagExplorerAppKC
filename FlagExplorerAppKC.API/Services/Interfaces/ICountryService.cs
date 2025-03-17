using FlagExplorerAppKC.API.Models;

namespace FlagExplorerAppKC.API.Services.Interfaces
{
    public interface ICountryService
    {
        Task<IEnumerable<Country>> GetAllCountriesAsync();

        Task<CountryDetails?> GetCountryDetailsAsync(string countryName);
    }
}
