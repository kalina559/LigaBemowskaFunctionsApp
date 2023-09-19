using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LigaBemowskaFunctionsApp.Models
{
    public class PlayerData
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Appearances { get; set; }
        public string Goals { get; set; }
        public string Assists { get; set; }
        public string YellowCards { get; set; }
        public string RedCards { get; set; }
        public string MOTMS { get; set; }
    }
}
