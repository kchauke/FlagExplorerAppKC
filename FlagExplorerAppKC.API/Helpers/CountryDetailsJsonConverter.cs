using System.Text.Json;
using System.Text.Json.Serialization;
using FlagExplorerAppKC.API.Models;


namespace FlagExplorerAppKC.API.Helpers
{
    public class CountryDetailsJsonConverter : JsonConverter<CountryDetails>
    {
        public override CountryDetails Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            using (JsonDocument document = JsonDocument.ParseValue(ref reader))
            {
                JsonElement root = document.RootElement;

                string? name = root.GetProperty("name").GetProperty("common").GetString();
                int? population = root.GetProperty("population").GetInt32();
                string? capitalList = root.GetProperty("capital").GetRawText();

                // taking the array and creating a single line string.
                string? capitalString = string.IsNullOrEmpty(capitalList) ? null : capitalList.Replace("[", string.Empty).Replace("]", string.Empty).Replace("\"", string.Empty).Replace(",", ", ");

                if (!population.HasValue)
                {
                    throw new JsonException("Population property is null or empty");
                }

                var countryDetails = new CountryDetails
                {
                    Name = name ?? throw new JsonException("Name property is null"),
                    Population = population.Value,
                    Capital = capitalString
                };

                return countryDetails;
            }
        }

        public override void Write(Utf8JsonWriter writer, CountryDetails value, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }
    }
}
