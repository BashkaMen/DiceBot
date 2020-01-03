using DevExpress.Mvvm;
using Dice.Client.Web;
using DiceBot.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace DiceBot.ViewModel
{
    class SelectTacticViewModel : ViewModelBase
    {
        public string MyAddress { get; set; }
        public string Address { get; set; }
        public int? AuthKey { get; set; }
        public double? Summ { get; set; }
        public double Balance { get; set; }

        private Currencies _Valute;
        public Currencies Valute
        {
            get { return _Valute; }
            set
            {
                _Valute = value;

                if (value != Currencies.None)
                {
                    DiceWebAPI.GetDepositAddressAsync(StaticData.Data.CurrentSession, Valute).ContinueWith(s =>
                    {
                        MyAddress = s?.Result?.DepositAddress;
                    });
                    DiceWebAPI.GetBalanceAsync(StaticData.Data.CurrentSession, Valute).ContinueWith(s =>
                    {
                        if (s.Result != null)
                        {
                            Balance = (double)s.Result.Balance;
                        }
                    });
                }

            }
        }

        public ICommand Matringale
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    Navigation.ChangePage(StaticData.MartingalePage);
                });
            }
        }

        public ICommand Back
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    Navigation.ChangePage(StaticData.LoginPage);
                });
            }
        }

        public ICommand WithDraw
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    var withdraw = DiceWebAPI.Withdraw(StaticData.Data.CurrentSession, (decimal)Summ, Address, AuthKey.GetValueOrDefault(), Valute);
                    if (withdraw?.Success == true)
                    {
                        MessageBox.Show("Запрос отправлен в обработку.", "OK", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        if (withdraw?.InsufficientFunds == true)
                        {
                            MessageBox.Show("Не достаточно денег на счету", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }
                        if (withdraw?.WithdrawalTooSmall == true)
                        {
                            MessageBox.Show("Ваша сумма меньше минимальной", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }
                        MessageBox.Show("Не удалось вывести средства", "Error", MessageBoxButton.OK, MessageBoxImage.Error);

                    }
                }, ()=> !string.IsNullOrEmpty(Address) && Summ.GetValueOrDefault() > 0 && Balance > 0);
            }
        }

        public ICommand WithDrawAll
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    if (MessageBox.Show("Вы уверены что хотите вывести все?", "Вывод", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No) return;


                    var withdraw = DiceWebAPI.WithdrawAll(StaticData.Data.CurrentSession, Address, AuthKey.GetValueOrDefault(), Valute);
                    if (withdraw?.Success == true)
                    {
                        MessageBox.Show("Запрос отправлен в обработку.", "OK", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        if (withdraw?.InsufficientFunds == true)
                        {
                            MessageBox.Show("Не достаточно денег на счету", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }
                        if (withdraw?.WithdrawalTooSmall == true)
                        {
                            MessageBox.Show("Ваша сумма меньше минимальной", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }
                        MessageBox.Show("Не удалось вывести средства", "Error", MessageBoxButton.OK, MessageBoxImage.Error);

                    }
                }, () => !string.IsNullOrEmpty(Address) && Balance > 0);
            }
        }

        
    }
}
