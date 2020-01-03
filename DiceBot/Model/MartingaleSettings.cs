using DevExpress.Mvvm;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiceBot.Model
{
    [JsonObject]
    class MartingaleSettings : ViewModelBase
    {
        public double? StartBet { get; set; }
        public double? MaxBet { get; set; }
        public double? WinPercent { get; set; }
        public double? Factor { get; set; }
        public double? ChangeWinPercent { get; set; }
        public bool CanChangeWinPercent { get; set; }
        public Operation Operation { get; set; }

        public bool Validate()
        {
            return StartBet.GetValueOrDefault() > 0
                && MaxBet.GetValueOrDefault() > 0
                && WinPercent.GetValueOrDefault() > 0
                && Factor.GetValueOrDefault() > 0
                && (!CanChangeWinPercent || ChangeWinPercent.GetValueOrDefault() > 0);
        }

    }
}
