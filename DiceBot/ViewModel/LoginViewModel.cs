using DevExpress.Mvvm;
using Dice.Client.Web;
using DiceBot.Model;
using DiceBot.Service;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace DiceBot.ViewModel
{
    class LoginViewModel : ViewModelBase
    {
        public ObservableCollection<Account> Accounts
        {
            get => StaticData.Data.Accounts; set
            {
                Accounts = value;
            }
        }
        public Account SelectedAccount { get; set; }
        public int? AuthKey { get; set; }

        AsyncCommand _Login;
        public AsyncCommand Login
        {
            get
            {
                return _Login ?? (_Login = new AsyncCommand(async() =>
                {
                    var session = await DiceWebAPI.BeginSessionAsync(Properties.Settings.Default.ApiKey, SelectedAccount.Login, SelectedAccount.Pass, AuthKey.GetValueOrDefault());

                    if (session.Success)
                    {
                        StaticData.Data.CurrentSession = session.Session;

                        Navigation.ChangePage(StaticData.SelectTacticPage);
                    }
                    else
                    {
                        var error = session.WrongUsernameOrPassword ? "Не верный логин и пароль" :
                                    session.TotpFailure ? "Не верный код аунтетификации" :
                                    "Не войти в аккаунт.";

                        MessageBox.Show(error, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                },
                ()=> SelectedAccount != null));
            }
        }

        public ICommand Register => new DelegateCommand(()=>
        {
            Navigation.ChangePage(StaticData.RegisterPage);
        });

        public ICommand OpenSettings
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    Navigation.ChangePage(StaticData.SettingsPage);
                });
            }
        }

    }
}
