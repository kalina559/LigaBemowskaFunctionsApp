using HtmlAgilityPack;
using LigaBemowskaFunctionsApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LigaBemowskaFunctionsApp.Helpers
{
    public class HtmlHelper
    {

        public static PlayerData GetDataFromDocument(HtmlDocument document, int id)
        {
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

            return new PlayerData
            {
                Id = id,
                Name = name,
                Appearances = appearances,
                Goals = goals,
                Assists = assists,
                YellowCards = yellowCards,
                RedCards = redCards,
                MOTMS = MOTMS
            };
        }

    }
}
