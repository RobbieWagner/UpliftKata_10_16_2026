using System.Collections.Generic;
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

        [Fact]
        public void UpdateQualityAppliesRulesToItems()
        {
            var items = new List<Item>
            {
                new Item(1, "Regular item", 10, 5)
            };
            var gildedRose = new ItemQualityUpdater();

            gildedRose.UpdateQuality(items);

            Assert.Equal(new Item(1, "Regular item", 9, 4), items[0]);
        }
    }
}
