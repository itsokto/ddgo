using System;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace DuckDuckGo
{
	public class VqdJsonConverter : JsonConverter<string>
	{
		public override bool CanConvert(Type objectType)
		{
			return objectType.IsAssignableFrom(typeof(string));
		}

		public override string? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
		{
			var jsonDocument = JsonDocument.ParseValue(ref reader);
			var jsonProperty = jsonDocument.RootElement.EnumerateObject().FirstOrDefault();

			return jsonProperty.Value.GetString();
		}

		public override void Write(Utf8JsonWriter writer, string value, JsonSerializerOptions options)
		{
			JsonSerializer.Serialize(writer, value, options);
		}
	}
}