using DevExpress.Mvvm;
using Dice.Client.Web;
using DiceBot.Service;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace DiceBot.ViewModel
{
    class RegisterViewModel : BindableBase
    {
        public string Login { get; set; }
        public string Pass { get; set; }
        public int? AuthKey { get; set; }



        public ICommand Register => new AsyncCommand(async () =>
        {
            if (StaticData.Data.Accounts.Any(s => s.Login == Login))
            {
                MessageBox.Show("Такой аккаунт уже есть в программе.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var session = await DiceWebAPI.BeginSessionAsync(Properties.Settings.Default.ApiKey, Login, Pass, AuthKey.GetValueOrDefault());

            if (session.Success)
            {
                StaticData.Data.Accounts.Add(new Model.Account
                {
                    Login = Login,
                    Pass = Pass
                });
                StaticData.Data.Save();
                MessageBox.Show("Аккаунт успешно добавлен!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                Navigation.ChangePage(StaticData.LoginPage);
            }
            else
            {
                var error = session.WrongUsernameOrPassword ? "Не верный логин и пароль" :
                            session.TotpFailure ? "Не верный код аунтетификации" :
                            "Не войти в аккаунт.";

                MessageBox.Show(error, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }, () => !string.IsNullOrEmpty(Login) && !string.IsNullOrEmpty(Pass));

        public ICommand GoToLogin => new DelegateCommand(() =>
        {
            Navigation.ChangePage(StaticData.LoginPage);
        });

    }
}
