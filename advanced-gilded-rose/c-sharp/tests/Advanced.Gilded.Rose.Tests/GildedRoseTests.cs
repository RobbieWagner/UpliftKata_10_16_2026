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
        public void UpdateQuality_DecreasesRegularItemQualityAndSellIn()
        {
            var item = new Item(1, "Regular item", 10, 5);

            new ItemQualityUpdater().UpdateQuality(new List<Item> { item });

            Assert.Equal(new Item(1, "Regular item", 9, 4), item);
        }

        [Fact]
        public void UpdateQuality_DecreasesRegularItemQualityTwiceAfterSellByDate()
        {
            var item = new Item(1, "Regular item", 10, 0);

            new ItemQualityUpdater().UpdateQuality(new List<Item> { item });

            Assert.Equal(new Item(1, "Regular item", 8, -1), item);
        }

        [Fact]
        public void UpdateQuality_DoesNotDecreaseRegularItemQualityBelowZero()
        {
            var item = new Item(1, "Regular item", 0, 5);

            new ItemQualityUpdater().UpdateQuality(new List<Item> { item });

            Assert.Equal(new Item(1, "Regular item", 0, 4), item);
        }

        [Fact]
        public void UpdateQuality_IncreasesAgedBrieQuality()
        {
            var item = new Item(1, "Aged Brie", 10, 5);

            new ItemQualityUpdater().UpdateQuality(new List<Item> { item });

            Assert.Equal(new Item(1, "Aged Brie", 11, 4), item);
        }

        [Fact]
        public void UpdateQuality_IncreasesAgedBrieQualityTwiceAfterSellByDate()
        {
            var item = new Item(1, "Aged Brie", 10, 0);

            new ItemQualityUpdater().UpdateQuality(new List<Item> { item });

            Assert.Equal(new Item(1, "Aged Brie", 12, -1), item);
        }

        [Fact]
        public void UpdateQuality_DoesNotIncreaseQualityAboveFifty()
        {
            var item = new Item(1, "Aged Brie", 50, 5);

            new ItemQualityUpdater().UpdateQuality(new List<Item> { item });

            Assert.Equal(new Item(1, "Aged Brie", 50, 4), item);
        }

        [Fact]
        public void UpdateQuality_IncreasesBackstagePassQualityByOneWhenSellInIsAboveTen()
        {
            var item = new Item(1, "Backstage passes to a TAFKAL80ETC concert", 10, 11);

            new ItemQualityUpdater().UpdateQuality(new List<Item> { item });

            Assert.Equal(new Item(1, "Backstage passes to a TAFKAL80ETC concert", 11, 10), item);
        }

        [Fact]
        public void UpdateQuality_IncreasesBackstagePassQualityByTwoWhenSellInIsTenOrLess()
        {
            var item = new Item(1, "Backstage passes to a TAFKAL80ETC concert", 10, 11);

            new ItemQualityUpdater().UpdateQuality(new List<Item> { item });

            Assert.Equal(new Item(1, "Backstage passes to a TAFKAL80ETC concert", 12, 10), item);
        }

        [Fact]
        public void UpdateQuality_IncreasesBackstagePassQualityByThreeWhenSellInIsFiveOrLess()
        {
            var item = new Item(1, "Backstage passes to a TAFKAL80ETC concert", 10, 5);

            new ItemQualityUpdater().UpdateQuality(new List<Item> { item });

            Assert.Equal(new Item(1, "Backstage passes to a TAFKAL80ETC concert", 13, 4), item);
        }

        [Fact]
        public void UpdateQuality_DropsBackstagePassQualityToZeroAfterConcert()
        {
            var item = new Item(1, "Backstage passes to a TAFKAL80ETC concert", 20, 0);

            new ItemQualityUpdater().UpdateQuality(new List<Item> { item });

            Assert.Equal(new Item(1, "Backstage passes to a TAFKAL80ETC concert", 0, -1), item);
        }

        [Fact]
        public void UpdateQuality_DoesNotChangeSulfurasQualityOrSellIn()
        {
            var item = new Item(1, "Sulfuras, Hand of Ragnaros", 80, 0);

            new ItemQualityUpdater().UpdateQuality(new List<Item> { item });

            Assert.Equal(new Item(1, "Sulfuras, Hand of Ragnaros", 80, 0), item);
        }
    }
}
