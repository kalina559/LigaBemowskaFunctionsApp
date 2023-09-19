using Azure;
using Azure.Data.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace LigaBemowskaFunctionsApp.Models
{
    public class Player : ITableEntity
    {
        public string Name { get; set; }
        public string Appearances { get; set; }
        public string Goals { get; set; }
        public string Assists { get; set; }
        public string YellowCards { get; set; }
        public string RedCards { get; set; }
        public string MOTMS { get; set; }        
        public DateTimeOffset? Timestamp { get; set; }

        [JsonIgnore]
        public string RowKey { get; set; }
        [JsonIgnore]
        public string PartitionKey { get; set; }
        [JsonIgnore]
        ETag ITableEntity.ETag { get; set; }

        public Player()
        {
        }

        public Player(PlayerData data)
        {
            PartitionKey = "1";   // always 1
            RowKey = data.Id.ToString();
            Name = data.Name;
            Appearances = data.Appearances;
            Goals = data.Goals;
            Assists = data.Assists;
            YellowCards = data.YellowCards;
            RedCards = data.RedCards;
            MOTMS = data.MOTMS;
        }

        public void UpdatePlayer(PlayerData data)
        {
            Name = data.Name;
            Appearances = data.Appearances;
            Goals = data.Goals;
            Assists = data.Assists;
            YellowCards = data.YellowCards;
            RedCards = data.RedCards;
            MOTMS = data.MOTMS;
        }
    }
}
