using System;
using System.Reflection;
using Refit;

namespace DuckDuckGo
{
	public class DuckApiBuilder
	{
		private const string BaseUrl = "https://duckduckgo.com/";
		
		public IDuckApi Build()
		{
			var settings = new RefitSettings
			{
				UrlParameterFormatter = new EnumsAsIntegersParameterFormatter()
			};
			return RestService.For<IDuckApi>(BaseUrl, settings);
		}
	}

	public class EnumsAsIntegersParameterFormatter : IUrlParameterFormatter
	{
		public virtual string? Format(object? value, ICustomAttributeProvider attributeProvider, Type type)
		{
			if (type.IsEnum)
			{
				return ((int) value).ToString();
			}

			return value?.ToString();
		}
	}
}