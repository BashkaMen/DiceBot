using Dice.Client.Web;
using DiceBot.Pages;
using DiceBot.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiceBot.Model
{
    static class PageFactory
    {
        public static GamePage Create(MartingaleSettings settings, Currencies valute)
        {
            return new GamePage()
            {
                DataContext = new GameViewModel
                {
                    Valute = valute,
                    Settings = settings
                }
            };
        }


    }
}
