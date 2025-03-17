namespace FlagExplorerAppKC.API.Models
{
    public class CountryDetails
    {
        public required string Name { get; set; }

        public int Population { get; set; }

        public string? Capital { get; set; }
    }
}
