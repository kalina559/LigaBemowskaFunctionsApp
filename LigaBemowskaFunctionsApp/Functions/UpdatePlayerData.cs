using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using LigaBemowskaFunctionsApp.Services;

namespace LigaBemowskaFunctionsApp.Functions
{
    public class UpdatePlayerData
    {
        [FunctionName("UpdatePlayerData")]
        public async Task RunAsync([TimerTrigger("0 19 * * TUE")] TimerInfo myTimer, ILogger log)
        {
            log.LogInformation($"Started UpdatePlayerData function executed at: {DateTime.Now}");

            var playerService = new PlayerService();

            //var counter = 0;
            //var id = 0;

            //while (counter < 20)
            //{
            //    try
            //    {
            //        counter--;
            //        await playerService.AddOrUpdatePlayer(id);
            //    }
            //    catch (Exception ex)
            //    {
            //        counter++;
            //        log.LogError($"Failed to upload Player %{id} because of {ex.Message}");
            //    }

            //    id++;
            //}

            for (int i = 100; i < 600; i++)
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
