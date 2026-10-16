using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace Advanced.Gilded.Rose
{
    [ApiController]
    [Route("")]
    public class ItemsController : Controller
    {
        private readonly IConfiguration _config;

        public ItemsController(IConfiguration config)
        {
            _config = config;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            await using var connection = new NpgsqlConnection(_config.GetConnectionString("Database"));
            await connection.OpenAsync();
            await using var command = connection.CreateCommand();
            command.CommandText = "select * from items";
            await using var reader = await command.ExecuteReaderAsync();
            var items = new List<Item>();
            while (await reader.ReadAsync())
            {
                items.Add(new Item(
                    reader.GetInt32(0),
                    reader.GetString(1),
                    reader.GetInt32(2),
                    reader.GetInt32(3))
                );
            }

            return Ok(items);
        }

        [HttpPost]
        public async Task<IActionResult> Update()
        {
            await using var connection = new NpgsqlConnection(_config.GetConnectionString("Database"));
            await connection.OpenAsync();

            var items = new List<Item>();
            await using (var getAllCommand = connection.CreateCommand())
            {
                getAllCommand.CommandText = "select * from items";
                await using var reader = await getAllCommand.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    items.Add(new Item(
                        reader.GetInt32(0),
                        reader.GetString(1),
                        reader.GetInt32(2),
                        reader.GetInt32(3))
                    );
                }
            }

            new ItemQualityUpdater().UpdateQuality(items);

            await using var updateCommand = connection.CreateCommand();
            updateCommand.CommandText = "update items set quality = @quality, sellIn = @sellIn where id = @id";
            updateCommand.Parameters.AddWithValue("quality", 0);
            updateCommand.Parameters.AddWithValue("sellIn", 0);
            updateCommand.Parameters.AddWithValue("id", 0);

            foreach (var item in items)
            {
                updateCommand.Parameters["quality"].Value = item.Quality;
                updateCommand.Parameters["sellIn"].Value = item.SellIn;
                updateCommand.Parameters["id"].Value = item.Id;
                await updateCommand.ExecuteNonQueryAsync();
            }

            return Ok();
        }
    }
}