using DevExpress.Mvvm;
using Dice.Client.Web;
using DiceBot.Service;
using System;
using System.Windows;
using System.Windows.Input;

namespace DiceBot.ViewModel
{
    class SettingsViewModel : BindableBase
    {
        public bool IsValid { get; set; }
        public string Mirror
        {
            get => Properties.Settings.Default.Mirror;
            set
            {
                Properties.Settings.Default.Mirror = value;
                IsValid = false;
            }
        }

        public AsyncCommand Valide
        {
            get
            {
                return new AsyncCommand(async () =>
                {
                    try
                    {
                        DiceWebAPI.WebUri = new Uri(Mirror + "/api/web.aspx");
                        var data = await DiceWebAPI.BeginSessionAsync(Properties.Settings.Default.ApiKey);

                        IsValid = data.Success;
                    }
                    catch
                    {
                        IsValid = false;
                        MessageBox.Show("Не верная ссылка на зеркало");
                        return;
                    }
                });
            }
        }

        public ICommand Save
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    Properties.Settings.Default.Save();

                    Navigation.ChangePage(StaticData.LoginPage);
                });
            }
        }
    }
}
