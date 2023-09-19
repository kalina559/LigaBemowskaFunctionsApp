using HtmlAgilityPack;
using LigaBemowskaFunctionsApp.Helpers;
using System.Net.Http;
using System.Threading.Tasks;

namespace LigaBemowskaFunctionsApp.Services
{
    public class PlayerService
    {
        const string PLAYER_SUMMARY_URL = "https://ligabemowska.pl/component/joomleague/player/1/1/{0}";
        private readonly HttpClient httpClient;
        private readonly TableService tableService;

        public PlayerService()
        {
            httpClient = new HttpClient();
            tableService = new TableService();
        }

        public async Task AddOrUpdatePlayer(int id)
        {
            var html = await httpClient.GetStringAsync(string.Format(PLAYER_SUMMARY_URL, id));
            var document = new HtmlDocument();
            document.LoadHtml(html);

            var playerHtmlData = HtmlHelper.GetDataFromDocument(document, id);

            var player = tableService.GetPlayerById(id);

            if (player != null)
            {
                player.UpdatePlayer(playerHtmlData);            
            }
            else
            {
                tableService.AddPlayer(playerHtmlData);
            }
        }
    }
}
