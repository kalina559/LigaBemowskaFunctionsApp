using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using System.Linq;
using HtmlAgilityPack;
using System.Net.Http;
using System.Threading.Tasks;

namespace LigaBemowskaFunctionsApp
{
    public class UpdatePlayerData
    {
        [FunctionName("UpdatePlayerData")]
        public async Task RunAsync([TimerTrigger("*/5 * * * * *")]TimerInfo myTimer, ILogger log)
        {
            log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");

            using var httpClient = new HttpClient();
            var html = await httpClient.GetStringAsync($"https://ligabemowska.pl/component/joomleague/player/1/1/6302");

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

        }
    }
}
