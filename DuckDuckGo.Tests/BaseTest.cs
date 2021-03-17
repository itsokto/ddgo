using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DuckDuckGo.Tests
{
	public abstract class BaseTest
	{
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