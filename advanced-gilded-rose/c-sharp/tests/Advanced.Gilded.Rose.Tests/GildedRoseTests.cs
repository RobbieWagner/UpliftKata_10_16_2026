using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace Advanced.Gilded.Rose.Tests
{
    public class GildedRoseTests
    {
        [Fact]
        public async Task ApplicationStartsAndReturnsNotFoundForUnknownRoute()
        {
            using var factory = new WebApplicationFactory<Startup>();
            using var client = factory.CreateClient();

            var response = await client.GetAsync("/does-not-exist");

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
    }
}
