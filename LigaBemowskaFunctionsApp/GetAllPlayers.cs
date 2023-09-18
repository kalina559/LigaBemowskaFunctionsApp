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

namespace LigaBemowskaFunctionsApp
{
    public static class GetAllPlayers
    {
        [FunctionName("GetAllPlayers")]
        public static string Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            [Table("Players")] IQueryable<Players> players,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            var json = JsonConvert.SerializeObject(players.ToArray());

            return json;
        }

        public class Players : ITableEntity
        {
            public string RowKey { get; set; }
            public string Name { get; set; }
            public string Goals { get; set; }
            public string Assists { get; set; }
            public string YellowCards { get; set; }
            public string RedCards { get; set; }
            public string MOTMS { get; set; }
            public string PartitionKey { get; set; }
            public DateTimeOffset? Timestamp { get; set; }
            public ETag ETag { get; set; }
        }
    }
}
