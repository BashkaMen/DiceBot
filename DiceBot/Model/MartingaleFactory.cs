using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiceBot.Model
{
    static class MartingaleFactory
    {
        public static MartingaleSettings _50_50 => new MartingaleSettings
        {
            CanChangeWinPercent = false,
            ChangeWinPercent = 0,
            Factor = 2,
            MaxBet = 1000000,
            WinPercent = 50,
            StartBet = 1,
            Operation = Operation.Multiply
        };

        public static MartingaleSettings _95_75 => new MartingaleSettings
        {
            CanChangeWinPercent = true,
            ChangeWinPercent = 75.5,
            Factor = 4,
            MaxBet = 1000000,
            Operation = Operation.Multiply,
            StartBet = 1,
            WinPercent = 95
        };
    }
}
