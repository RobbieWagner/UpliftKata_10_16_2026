Commit 1
- Single failing unit test already existed, so I updated it to instead test that the api does not work for an endpoint that doesn't exist. Next, I will consider creating unit tests for this application without mocking. This may require slight restructuring of the application.

Commit 2
- Created a class ItemQualityUpdater which will apply rules for teh Gilded Rose
- Created unit tests that assumed the data I enter is being used. Since the business rules are coupled to an inaccessible database, these should fail
In next commits, I will implement the "UpdateQuality" method, likely just ripping the existing logic from the existing controller

Commit 3
- Implemented the UpdateQuality method 
- Ensured happy path test works
In the next commit, I create more unit tests for the UpdateQualityMethod, one for each of the unique cases listed in the called out rules for items

Commit 4
- Added unit tests to cover the business rules:
"""
- Once the sell by date has passed, Quality degrades twice as fast
- The Quality of an item is never negative
- "Aged Brie" actually increases in Quality the older it gets
- The Quality of an item is never more than 50
- "Sulfuras", being a legendary item, never has to be sold or decreases in Quality
- "Backstage passes", like aged brie, increases in Quality as its SellIn value approaches;
Quality increases by 2 when there are 10 days or less and by 3 when there are 5 days or less but
Quality drops to 0 after the concert
"""
- Tests currently fail as most haven't been implemented
Next commit, I will implement a version of this UpdateQuality Method that will pass all of the created unit tests