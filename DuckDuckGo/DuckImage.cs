using System.Text.Json.Serialization;

namespace DuckDuckGo
{
	public class DuckImage
	{
		[JsonPropertyName("height")]
		public long Height { get; set; }

		[JsonPropertyName("image")]
		public string Image { get; set; }

		[JsonPropertyName("url")]
		public string Url { get; set; }

		[JsonPropertyName("thumbnail")]
		public string Thumbnail { get; set; }

		[JsonPropertyName("title")]
		public string Title { get; set; }

		[JsonPropertyName("width")]
		public long Width { get; set; }
	}
}