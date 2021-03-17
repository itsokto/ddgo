using System;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace DuckDuckGo
{
	public static class DuckApiExtensions
	{
		private const string Pattern = @"vqd=\'(?<vqd>[\d-]+)\'";

		private static readonly Regex VqdRegex = new(Pattern, RegexOptions.Multiline | RegexOptions.IgnoreCase | RegexOptions.Compiled);

		public static async Task<string> GetTokenAsync(this IDuckApi api, string query, CancellationToken cancellationToken = default)
		{
			var page = await api.GetRawPageAsync(query, cancellationToken).ConfigureAwait(false);

			var math = VqdRegex.Match(page);
			if (!math.Success)
			{
				throw new InvalidOperationException("No match for vqd-token.");
			}

			var vqd = math.Groups["vqd"].Value;

			if (string.IsNullOrWhiteSpace(vqd))
			{
				throw new InvalidOperationException("Expected vqd-token, but was an empty string.");
			}

			return vqd;
		}

		public static async Task<DuckResponse<DuckImage>> GetImagesAsync(this IDuckApi api,
		                                                                 string query,
		                                                                 DuckSearchFilter duckSearchFilter = DuckSearchFilter.Off,
		                                                                 CancellationToken cancellationToken = default)
		{
			var vqd = await GetTokenAsync(api, query, cancellationToken).ConfigureAwait(false);
			return await api.GetImagesAsync(query, vqd, duckSearchFilter, cancellationToken).ConfigureAwait(false);
		}

		public static Task<DuckResponse<T>> NextAsync<T>(this IDuckApi api,
		                                                 DuckResponse<T> response,
		                                                 CancellationToken cancellationToken = default)
		{
			return api.NextAsync<T>(response.Next, response.Vqd, cancellationToken);
		}
	}
}