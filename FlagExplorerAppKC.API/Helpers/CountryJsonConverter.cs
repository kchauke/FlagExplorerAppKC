using System.Text.Json;
using System.Text.Json.Serialization;
using FlagExplorerAppKC.API.Models;


namespace FlagExplorerAppKC.API.Helpers
{
    public class CountryJsonConverter : JsonConverter<Country>
    {
        public override Country Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            using (JsonDocument document = JsonDocument.ParseValue(ref reader))
            {
                JsonElement root = document.RootElement;

                string? name = root.GetProperty("name").GetProperty("common").GetString();
                string? flag = root.GetProperty("flags").GetProperty("png").GetString();

                var country = new Country
                {
                    Name = name ?? throw new JsonException("Name property is null"),
                    Flag = flag
                };

                return country;
            }
        }

        public override void Write(Utf8JsonWriter writer, Country value, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }
    }
}
