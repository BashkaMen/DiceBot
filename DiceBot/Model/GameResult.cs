using DevExpress.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiceBot.Model
{
    class GameResult : ViewModelBase
    {
        public bool Result { get; set; }
        public double Profit { get; set; }
        public double Bet { get; set; }

    }
}
