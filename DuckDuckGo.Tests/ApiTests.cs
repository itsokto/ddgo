using System;
using System.Threading.Tasks;
using Xunit;

namespace DuckDuckGo.Tests
{
	public class ApiTests : BaseTest
	{
		private readonly IDuckApi _duckGoApi;

		public ApiTests()
		{
			_duckGoApi = new DuckApiBuilder().Build();
		}

		[Fact]
		public async Task GetTokenTest()
		{
			var html = ReadFile("get_token_car.html");

			var token = await _duckGoApi.GetTokenAsync("car");

			Assert.Equal("3-46082209034006461627445001051878587260-103201150309019047502164315555969820387", token);
		}

		[Fact]
		public async Task GetTokenThrowsInvalidOperationExceptionTest()
		{
			var html = ReadFile("get_token_car_without_vqd.html");

			await Assert.ThrowsAsync<InvalidOperationException>(() => _duckGoApi.GetTokenAsync("car"));
		}

		[Fact]
		public async Task GetImagesTest()
		{
			var html = ReadFile("get_token_car.html");
			var json = ReadFile("get_images_car.json");

			var response = await _duckGoApi.GetImagesAsync("car");

			Assert.NotNull(response);
			Assert.Equal("i.js?q=car&o=json&p=-1&s=100&u=bing&f=,,,&l=us-en", response.Next);
			Assert.Equal("car", response.Query);
			Assert.NotEmpty(response.Results);
			Assert.Equal("3-46081923812621494983278470627847170412-103201150309019047502164315555969820387", response.Vqd);
		}

		[Fact]
		public async Task SearchImagesNextTest()
		{
			var response = await _duckGoApi.GetImagesAsync("car");
			var nextResponse = await _duckGoApi.NextAsync(response);

			Assert.NotNull(nextResponse);
			Assert.NotEmpty(nextResponse.Results);
		}
	}
}