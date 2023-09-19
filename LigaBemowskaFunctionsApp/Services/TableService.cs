using Azure;
using Azure.Data.Tables;
using LigaBemowskaFunctionsApp.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;

namespace LigaBemowskaFunctionsApp.Services
{
    public class TableService
    {
        const string PLAYER_TABLE_NAME = "Players";
        const string CONNECTION_STRING = "DefaultEndpointsProtocol=https;AccountName=ligabemowskastats;AccountKey=8TMzEAfg6lzzV/VfBqZhCmzZMUwDcdYh81Laal2vqOrS7/q77gTlyccgxJcCCrFVQcAVxgEn214d+ASt+OZxkw==;EndpointSuffix=core.windows.net";
        ILogger _logger;

        private readonly TableClient tableClient;

        public TableService(ILogger log)
        {
            var serviceClient = new TableServiceClient(CONNECTION_STRING);
            tableClient = serviceClient.GetTableClient(PLAYER_TABLE_NAME);
            _logger = log;
        }

        public Player GetPlayerById(int id)
        {
            var filter = TableClient.CreateQueryFilter($"RowKey eq '{id}'");
            var queryResults = tableClient.Query<Player>(filter);
            return queryResults.SingleOrDefault();
        }

        public async void AddPlayer(PlayerData data)
        {
            var player = new Player(data);
            await tableClient.AddEntityAsync(player);
            _logger.LogInformation($"Inserted a new Player #{data.Id} successfully.");
        }

        public async void UpdatePlayer(Player player)
        {
            await tableClient.UpdateEntityAsync(player, ETag.All);
            _logger.LogInformation($"Updated Player #{player.RowKey} successfully.");
        }
    }
}
