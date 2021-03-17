using System;
using System.Net;
using System.Net.Http;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using RichardSzalay.MockHttp;
using Xunit;

namespace DuckDuckGo.Tests
{
	public class ApiTests : BaseTest
	{
		[Fact]
		public async Task GetTokenTest()
		{
			var html = ReadFile("get_token_car.html");

			MockHttp.Expect(HttpMethod.Get, "/")
			        .Respond(HttpStatusCode.OK, new StringContent(html, Encoding.UTF8, MediaTypeNames.Text.Html));

			var token = await DuckApi.GetTokenAsync("car");

			Assert.Equal("3-46082209034006461627445001051878587260-103201150309019047502164315555969820387", token);
		}

		[Fact]
		public async Task GetTokenThrowsInvalidOperationExceptionTest()
		{
			var html = ReadFile("get_token_car_without_vqd.html");

			MockHttp.Expect(HttpMethod.Get, "/")
			        .Respond(HttpStatusCode.OK, new StringContent(html, Encoding.UTF8, MediaTypeNames.Text.Html));

			await Assert.ThrowsAsync<InvalidOperationException>(() => DuckApi.GetTokenAsync("car"));
		}

		[Fact]
		public async Task GetImagesTest()
		{
			var html = ReadFile("get_token_car.html");
			var json = ReadFile("get_images_car.json");

			MockHttp.Expect(HttpMethod.Get, "/")
			        .Respond(HttpStatusCode.OK, new StringContent(html, Encoding.UTF8, MediaTypeNames.Text.Html));
			MockHttp.Expect(HttpMethod.Get, "/i.js")
			        .Respond(HttpStatusCode.OK, new StringContent(json, Encoding.UTF8, MediaTypeNames.Application.Json));

			var response = await DuckApi.GetImagesAsync("car");

			Assert.NotNull(response);
			Assert.Equal("i.js?q=car&o=json&p=-1&s=100&u=bing&f=,,,&l=us-en", response.Next);
			Assert.Equal("car", response.Query);
			Assert.NotEmpty(response.Results);
			Assert.Equal("3-46081923812621494983278470627847170412-103201150309019047502164315555969820387", response.Vqd);
		}

		[Fact]
		public async Task SearchImagesNextTest()
		{
			var html = ReadFile("get_token_car.html");
			var json = ReadFile("get_images_car.json");

			MockHttp.Expect(HttpMethod.Get, "/")
			        .Respond(HttpStatusCode.OK, new StringContent(html, Encoding.UTF8, MediaTypeNames.Text.Html));
			MockHttp.Expect(HttpMethod.Get, "/i.js")
			        .Respond(HttpStatusCode.OK, new StringContent(json, Encoding.UTF8, MediaTypeNames.Application.Json));
			MockHttp.Expect(HttpMethod.Get, "/i.js")
			        .Respond(HttpStatusCode.OK, new StringContent(json, Encoding.UTF8, MediaTypeNames.Application.Json));

			var response = await DuckApi.GetImagesAsync("car");
			var nextResponse = await DuckApi.NextAsync(response);

			Assert.NotNull(nextResponse);
			Assert.NotEmpty(nextResponse.Results);
		}
	}
}