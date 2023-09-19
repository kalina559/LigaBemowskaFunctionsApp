using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Azure;
using Azure.Data.Tables;
using System.Linq;
using LigaBemowskaFunctionsApp.Models;

namespace LigaBemowskaFunctionsApp.Functions
{
    public static class GetAllPlayers
    {
        [FunctionName("GetAllPlayers")]
        public static string Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string connectionString = "DefaultEndpointsProtocol=https;AccountName=ligabemowskastats;AccountKey=8TMzEAfg6lzzV/VfBqZhCmzZMUwDcdYh81Laal2vqOrS7/q77gTlyccgxJcCCrFVQcAVxgEn214d+ASt+OZxkw==;EndpointSuffix=core.windows.net";
            string tableName = "Players";

            var serviceClient = new TableServiceClient(connectionString);
            var tableClient = serviceClient.GetTableClient(tableName);
            var query = TableClient.CreateQueryFilter($"");
            var queryResults = tableClient.Query<Player>(query);

            var json = JsonConvert.SerializeObject(queryResults.ToArray());

            return json;
        }

        
    }
}
