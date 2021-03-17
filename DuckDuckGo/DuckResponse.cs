using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace DuckDuckGo
{
	public class DuckResponse<T>
	{
		[JsonConverter(typeof(VqdJsonConverter))]
		[JsonPropertyName("vqd")]
		public string Vqd { get; set; }

		[JsonPropertyName("next")]
		public string Next { get; set; }

		[JsonPropertyName("results")]
		public IList<T> Results { get; set; }

		[JsonPropertyName("query")]
		public string Query { get; set; }
	}
}