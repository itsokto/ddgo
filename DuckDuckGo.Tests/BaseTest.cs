using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Refit;
using RichardSzalay.MockHttp;

namespace DuckDuckGo.Tests
{
	public abstract class BaseTest
	{
		private const string BaseUrl = "https://duckduckgo.com/";

		protected IDuckApi DuckApi { get; }

		protected MockHttpMessageHandler MockHttp = new();

		public BaseTest()
		{
			var settings = new RefitSettings
			{
				HttpMessageHandlerFactory = () => MockHttp
			};

			DuckApi = RestService.For<IDuckApi>(BaseUrl, settings);
		}

		protected string ReadFile(params string[] fileRelativePaths)
		{
			var folders = new List<string>
			{
				AppContext.BaseDirectory,
				"TestData"
			};

			folders.AddRange(fileRelativePaths);

			var path = Path.Combine(folders.ToArray());

			if (!File.Exists(path))
			{
				throw new FileNotFoundException(path);
			}

			return File.ReadAllText(path, Encoding.UTF8);
		}
	}
}