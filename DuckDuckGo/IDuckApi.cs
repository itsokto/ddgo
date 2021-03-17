using System;
using System.Threading;
using System.Threading.Tasks;
using Refit;

namespace DuckDuckGo
{
	public interface IDuckApi
	{
		[Get("/i.js?f=,,,&o=json")]
		Task<DuckResponse<DuckImage>> GetImagesAsync([AliasAs("q")] string query,
		                                             [AliasAs("vqd")] string vqd,
		                                             [AliasAs("p")] DuckSearchFilter duckSearchFilter = DuckSearchFilter.Off,
		                                             CancellationToken cancellationToken = default);

		[Get("/")]
		Task<string> GetRawPageAsync([AliasAs("q")] string query, CancellationToken cancellationToken = default);

		[Get("/{**next}&vqd={vqd}")]
		[QueryUriFormat(UriFormat.Unescaped)]
		Task<DuckResponse<T>> NextAsync<T>(string next, string vqd, CancellationToken cancellationToken = default);
	}
}