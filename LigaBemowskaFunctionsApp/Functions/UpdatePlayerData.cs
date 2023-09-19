using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using LigaBemowskaFunctionsApp.Services;
using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Blob;
using System.IO;
using System.Text;

namespace LigaBemowskaFunctionsApp.Functions
{
    public class UpdatePlayerData
    {
        const string CONNECTION_STRING = "DefaultEndpointsProtocol=https;AccountName=ligabemowskastats;AccountKey=8TMzEAfg6lzzV/VfBqZhCmzZMUwDcdYh81Laal2vqOrS7/q77gTlyccgxJcCCrFVQcAVxgEn214d+ASt+OZxkw==;EndpointSuffix=core.windows.net";
        const string CONTAINER_NAME = "ligabemowska";
        const string BLOB_NAME = "iterator.txt";

        [FunctionName("UpdatePlayerData")]
        public async Task RunAsync([TimerTrigger("0 */5 20-23 * * *")] TimerInfo myTimer, ILogger log)
        {
            log.LogInformation($"Started UpdatePlayerData function executed at: {DateTime.Now}");

            var playerService = new PlayerService(log);

            int startIndex = 0;
            int consecutiveErrorCounter = 0;

            var storageAccount = CloudStorageAccount.Parse(CONNECTION_STRING);
            var blobClient = storageAccount.CreateCloudBlobClient();
            var container = blobClient.GetContainerReference(CONTAINER_NAME);
            var blob = container.GetBlockBlobReference(BLOB_NAME);


            if (blob.Exists())
            {
                using (Stream blobStream = blob.OpenRead())
                {
                    using (StreamReader reader = new StreamReader(blobStream))
                    {
                        startIndex = Convert.ToInt32(reader.ReadToEnd());
                        log.LogInformation($"Retrieved startIndex. The current value is {startIndex}");
                    }
                }
            }

            for (int i = startIndex; i < startIndex + 100; i++)
            {
                try
                {
                    await playerService.AddOrUpdatePlayer(i);
                    consecutiveErrorCounter = 0;
                }
                catch (Exception ex)
                {
                    log.LogError($"Failed to upload Player %{i} because of {ex.Message}");
                    consecutiveErrorCounter++;
                }

                if(consecutiveErrorCounter == 20)
                {
                    // it means that we probably went through all the players. time to reset the startIndex
                    // save the next start index
                    using (Stream stream = new MemoryStream(Encoding.UTF8.GetBytes((0).ToString())))
                    {
                        blob.UploadFromStream(stream);
                    }

                    break;
                }
            }

            // save the next start index
            using (Stream stream = new MemoryStream(Encoding.UTF8.GetBytes((startIndex + 100).ToString())))
            {
                blob.UploadFromStream(stream);
            }

            log.LogInformation($"Finished UpdatePlayerData function executed at: {DateTime.Now}");
        }
    }
}
