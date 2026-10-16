using System;
using System.Collections.Generic;

namespace Advanced.Gilded.Rose
{
    public class ItemQualityUpdater
    {
        private const string AgedBrie = "Aged Brie";
        private const string BackstagePasses = "Backstage passes to a TAFKAL80ETC concert";
        private const string ConjuredPrefix = "Conjured";
        private const string Sulfuras = "Sulfuras, Hand of Ragnaros";

        public void UpdateQuality(List<Item> items)
        {
            foreach (var item in items)
            {
                if (item.Name == Sulfuras)
                    continue;

                if (item.Name == AgedBrie)
                {
                    IncreaseQuality(item);
                }
                else if (item.Name == BackstagePasses)
                {
                    IncreaseQuality(item);

                    if (item.SellIn < 11)
                        IncreaseQuality(item);

                    if (item.SellIn < 6)
                        IncreaseQuality(item);
                }
                else
                {
                    DecreaseQuality(item);

                    if (IsConjured(item))
                        DecreaseQuality(item);
                }

                item.SellIn--;

                if (item.SellIn < 0)
                {
                    if (item.Name == AgedBrie)
                        IncreaseQuality(item);
                    else if (item.Name == BackstagePasses)
                        item.Quality = 0;
                    else
                    {
                        DecreaseQuality(item);

                        if (IsConjured(item))
                            DecreaseQuality(item);
                    }
                }
            }
        }

        private static void DecreaseQuality(Item item)
        {
            if (item.Quality > 0)
                item.Quality--;
        }

        private static bool IsConjured(Item item)
        {
            return item.Name.StartsWith(ConjuredPrefix, StringComparison.Ordinal);
        }

        private static void IncreaseQuality(Item item)
        {
            if (item.Quality < 50)
                item.Quality++;
        }
    }
}