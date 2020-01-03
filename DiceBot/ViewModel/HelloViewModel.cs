using DevExpress.Mvvm;
using DiceBot.Service;
using System;
using System.Diagnostics;
using System.Net;
using System.Windows.Input;

namespace DiceBot.ViewModel
{
    class HelloViewModel : ViewModelBase
    {
        private Version Version = new Version(4, 5);


        public string UpdateText { get; set; }

        public HelloViewModel()
        {
            UpdateText = "Проверка обновлений...";

            UpdateText = CheckUpdate() ? "Есть обновления" : "Вы используете последнюю версию";
        }

        public ICommand Next
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    Navigation.ChangePage(StaticData.SettingsPage);
                });
            }
        }

        public ICommand Update
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    try
                    {
                        Process.Start("https://sbm159.wixsite.com/dicebot");
                    }
                    catch (Exception ex)
                    {
                        Process.Start("IExplore.exe", "https://sbm159.wixsite.com/dicebot");
                    }
                });
            }
        }

        bool CheckUpdate()
        {
            WebClient wb = new WebClient();

            try
            {
                var data = wb.DownloadString("https://www.dropbox.com/s/bcurau3g1aufdpf/Version.txt?dl=1");
                var vers = Version.Parse(data);

                if (Version < vers)
                {
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }
    }
}
