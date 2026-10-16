using System.Collections.Generic;
using Microsoft.AspNetCore.Http.Metadata;

namespace Advanced.Gilded.Rose
{
    public class ItemQualityUpdater
    {
        private const string AgedBrie = "Aged Brie";
        private const string BackstagePasses = "Backstage passes to a TAFKAL80ETC concert";
        private const string Sulfuras = "Sulfuras, Hand of Ragnaros";

        public void UpdateQuality(List<Item> items)
        {
            foreach (var item in items)
            {
                item.SellIn--;
                
                if (item.Quality > 0)
                    item.Quality--;
            }
        }
    }
}
