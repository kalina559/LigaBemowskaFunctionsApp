using Azure;
using Azure.Data.Tables;
using Newtonsoft.Json;
using System;

namespace LigaBemowskaFunctionsApp.Models
{
    public class Player : ITableEntity
    {
        [JsonIgnore]
        public string RowKey { get; set; }
        [JsonIgnore]
        public string PartitionKey { get; set; }
        [JsonIgnore]
        ETag ITableEntity.ETag { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public int Appearances { get; set; }
        public int Goals { get; set; }
        public int Assists { get; set; }
        public int YellowCards { get; set; }
        public int RedCards { get; set; }
        public int MOTMS { get; set; }        
        public DateTimeOffset? Timestamp { get; set; }

        public Player()
        {
        }

        public Player(PlayerData data)
        {
            PartitionKey = "1";   // always 1
            RowKey = data.Id.ToString();
            Id = data.Id;
            Name = data.Name;
            Appearances = int.Parse(data.Appearances);
            Goals = int.Parse(data.Goals);
            Assists = int.Parse(data.Assists);
            YellowCards = int.Parse(data.YellowCards);
            RedCards = int.Parse(data.RedCards);
            MOTMS = int.Parse(data.MOTMS);
        }

        public void UpdatePlayer(PlayerData data)
        {
            Name = data.Name;
            Appearances = int.Parse(data.Appearances);
            Goals = int.Parse(data.Goals);
            Assists = int.Parse(data.Assists);
            YellowCards = int.Parse(data.YellowCards);
            RedCards = int.Parse(data.RedCards);
            MOTMS = int.Parse(data.MOTMS);
        }
    }
}
