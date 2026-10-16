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

Commit 5
- Implemented the UpdateQualityMethod to pass all unit tests
- have the post endpoint call the new UpdateQuality method (This took longer than expected. Had issues with my version of the db command and had to defer to copilot)
- had copilot write up a manual testing set up testing instructions (mostly so I wouldn't have to remember so many steps myself later)
- fixed conjured item behavior which I missed implementing

At the end of this commit, I've run out of time. I will come back later and try to run the generated steps to make sure everything works from that perspective, otherwise this is the final version. With more time, I would have spent more time ensuring all cases were covered, or designed a system that doesn't handle these different cases in a daisy chained if else structure, but with little time to do either of these things, I will leave it where it is now

## How to Test

The database used by the kata is hosted in AWS and is not accessible. To test the API locally, run it against the local PostgreSQL test database provided in the repository's `database` directory. This database has the same `items` table shape and is separate from the inaccessible AWS database.

### Start the local database

Start Docker, then run these commands in PowerShell from the repository root:

```powershell
Set-Location .\database
docker build -t advanced-gilded-rose .
docker run -d --name advanced-gilded-rose-db -p 5432:5432 advanced-gilded-rose
docker logs -f advanced-gilded-rose-db
```

Wait for PostgreSQL to report that it is ready, then press **Ctrl+C** to stop following the logs. The database container initializes the database and sample rows using `create-database.sql`. The API's configured connection string must point to this local database on `localhost:5432`.

### Run the API

Open another PowerShell window at the repository root and run:

```powershell
dotnet run --project .\c-sharp\src\Advanced.Gilded.Rose\Advanced.Gilded.Rose.csproj --launch-profile Advanced.Gilded.Rose
```

The launch profile serves the API at `http://localhost:5000` and `https://localhost:5001`.

### Call the endpoints

The API has two endpoints:

- `GET /` returns all rows from the `items` table.
- `POST /` updates the quality and sell-in values for every row in the database. It does not take a request body.

Use `curl.exe` in PowerShell to call the HTTP endpoint:

```powershell
curl.exe -i http://localhost:5000/
curl.exe -i -X POST http://localhost:5000/
curl.exe -i http://localhost:5000/
```

The first GET shows the starting values. POST should return `200 OK` with an empty response body. The last GET shows the values persisted by POST. Each POST simulates another day, so reset or recreate the test data before repeating a scenario.

### Set up specific test cases

There is no API endpoint to add or edit items. For targeted scenarios, prepare the rows directly in the **local test database**, not the inaccessible AWS database. For example, connect to the local container:

```powershell
docker exec -it advanced-gilded-rose-db psql -U docker -d docker
```

At the `psql` prompt, replace the sample rows with test cases:

```sql
TRUNCATE TABLE items RESTART IDENTITY;

INSERT INTO items (name, quality, sellIn) VALUES
  ('Normal', 10, 5),
  ('Normal overdue', 10, 0),
  ('Aged Brie', 49, 0),
  ('Backstage passes to a TAFKAL80ETC concert', 10, 10),
  ('Backstage passes to a TAFKAL80ETC concert', 10, 5),
  ('Backstage passes to a TAFKAL80ETC concert', 20, 0),
  ('Sulfuras, Hand of Ragnaros', 80, 0),
  ('Conjured Mana Cake', 10, 5);
```

After one POST, the expected results are:

| Item scenario | Starting quality / sell-in | Expected quality / sell-in |
| --- | ---: | ---: |
| Normal item | 10 / 5 | 9 / 4 |
| Normal item past sell-by | 10 / 0 | 8 / -1 |
| Aged Brie at the quality cap boundary | 49 / 0 | 50 / -1 |
| Backstage passes with 10 days remaining | 10 / 10 | 12 / 9 |
| Backstage passes with 5 days remaining | 10 / 5 | 13 / 4 |
| Backstage passes after the concert | 20 / 0 | 0 / -1 |
| Sulfuras | 80 / 0 | 80 / 0 |
| Conjured item (per requirements) | 10 / 5 | 8 / 4 |