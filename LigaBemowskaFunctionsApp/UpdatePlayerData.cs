using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using System.Linq;
using HtmlAgilityPack;
using System.Net.Http;
using System.Threading.Tasks;
using Azure.Data.Tables;
using Azure;

namespace LigaBemowskaFunctionsApp
{
    public class UpdatePlayerData
    {
        [FunctionName("UpdatePlayerData")]
        public async Task RunAsync([TimerTrigger("0 0 * * * *")] TimerInfo myTimer, ILogger log)
        {
            log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");


            string connectionString = "DefaultEndpointsProtocol=https;AccountName=ligabemowskastats;AccountKey=8TMzEAfg6lzzV/VfBqZhCmzZMUwDcdYh81Laal2vqOrS7/q77gTlyccgxJcCCrFVQcAVxgEn214d+ASt+OZxkw==;EndpointSuffix=core.windows.net";
            string tableName = "Players";

            var serviceClient = new TableServiceClient(connectionString);
            var tableClient = serviceClient.GetTableClient(tableName);

            var id = 6302;




            using var httpClient = new HttpClient();
            var html = await httpClient.GetStringAsync($"https://ligabemowska.pl/component/joomleague/player/1/1/{id}");

            var document = new HtmlDocument();
            document.LoadHtml(html);


            var playerNameDiv = document.DocumentNode.SelectSingleNode("//div[@class='player-desc m-4 player-text']");

            var playerName = playerNameDiv.Descendants().Where(n => n.Name == "h3").SingleOrDefault();
            var name = playerName.InnerHtml.Trim();

            var stats_total = document.DocumentNode.SelectSingleNode("//tr[@class='career_stats_total']");

            //0 - appearances, 1 - goals, 2 - assists, 3 - yellow cards, 4 - red cards, 5 - motm
            var statsCells = stats_total.Descendants(0).Where(n => n.HasClass("td_c"));

            var appearances = statsCells.ElementAt(0).InnerHtml.Trim();
            var goals = statsCells.ElementAt(1).InnerHtml.Trim();
            var assists = statsCells.ElementAt(2).InnerHtml.Trim();
            var yellowCards = statsCells.ElementAt(3).InnerHtml.Trim();
            var redCards = statsCells.ElementAt(4).InnerHtml.Trim();
            var MOTMS = statsCells.ElementAt(5).InnerHtml.Trim();


            var filter = TableClient.CreateQueryFilter($"RowKey eq '{id}'");

            var queryResults = tableClient.Query<TableEntity>(filter);

            var playerRecord = queryResults.SingleOrDefault();

            if (playerRecord != null)
            {
                playerRecord["Name"] = name;
                playerRecord["Appearances"] = appearances;
                playerRecord["Goals"] = goals;
                playerRecord["Assists"] = assists;
                playerRecord["YellowCards"] = yellowCards;
                playerRecord["RedCards"] = redCards;
                playerRecord["MOTMS"] = MOTMS;

                await tableClient.UpdateEntityAsync(playerRecord, ETag.All);
                Console.WriteLine($"Updated Player #{id} successfully.");
            }
            else
            {
                playerRecord = new TableEntity("1", id.ToString())  // Partition is always 1 and RowKey is Player's Id
                {
                    { "Name", name },
                    { "Appearances", appearances },
                    { "Goals", goals },
                    { "Assists", assists },
                    { "YellowCards", yellowCards },
                    { "RedCards", redCards },
                    { "MOTMS", MOTMS }
                };

                await tableClient.AddEntityAsync(playerRecord);

                Console.WriteLine($"Inserted a new Player #{id} successfully.");
            }
        }
    }
}
