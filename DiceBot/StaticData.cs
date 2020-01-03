using DiceBot.Model;
using DiceBot.Pages;
using System.Windows.Controls;

namespace DiceBot
{
    static class StaticData
    {
        public static DataBase Data = DataBase.LoadOrDefault();


        public static Page HelloPage = new HelloPage();
        public static Page SettingsPage = new SettingsPage();
        public static Page LoginPage = new LoginPage();
        public static Page SelectTacticPage = new SelectTacticPage();
        public static Page MartingalePage = new MartingalePage();
        public static Page RegisterPage = new RegisterPage();

        static StaticData()
        {

        }
    }
}
