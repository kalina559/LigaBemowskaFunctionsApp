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
using LigaBemowskaFunctionsApp.Services;

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

            var tableService = new TableService(log);
            var players = tableService.GetAllPlayers();

            var json = JsonConvert.SerializeObject(players.ToArray());

            return json;
        }

        
    }
}
