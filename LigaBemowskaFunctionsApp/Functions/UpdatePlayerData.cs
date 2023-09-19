using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using System.Linq;
using HtmlAgilityPack;
using System.Net.Http;
using System.Threading.Tasks;
using Azure.Data.Tables;
using Azure;
using LigaBemowskaFunctionsApp.Models;
using LigaBemowskaFunctionsApp.Helpers;
using LigaBemowskaFunctionsApp.Services;

namespace LigaBemowskaFunctionsApp.Functions
{
    public class UpdatePlayerData
    {
        [FunctionName("UpdatePlayerData")]
        public async Task RunAsync([TimerTrigger("*/5 * * * * *")] TimerInfo myTimer, ILogger log)
        {
            log.LogInformation($"Started UpdatePlayerData function executed at: {DateTime.Now}");

            var playerService = new PlayerService();

            for(int i = 100; i < 105; i++)
            {
                try
                {
                    await playerService.AddOrUpdatePlayer(i);
                }
                catch (Exception ex)
                {
                    log.LogError($"Failed to upload Player %{i} because of {ex.Message}");
                }
            }           

            log.LogInformation($"Finished UpdatePlayerData function executed at: {DateTime.Now}");

        }
    }
}
