using DevExpress.Mvvm;
using Dice.Client.Web;
using DiceBot.Model;
using DiceBot.Pages;
using DiceBot.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DiceBot.ViewModel
{
    class MartingaleViewModel : ViewModelBase
    {
        public MartingaleSettings MartingaleSettings { get; set; }

        private Currencies _Valute;
        public Currencies Valute
        {
            get { return _Valute; }
            set
            {
                _Valute = value;

                if (value != Currencies.None)
                {
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

        public int MaxBetCount { get; set; }
        public double Balance { get; set; }

        public MartingaleViewModel()
        {
            MartingaleSettings = new MartingaleSettings();
            Valute = Currencies.None;
        }

        public ICommand _50_50
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    MartingaleSettings = MartingaleFactory._50_50;
                });
            }
        }
        public ICommand _95_75
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    MartingaleSettings = MartingaleFactory._95_75;
                });
            }
        }
        public ICommand Reset
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    MartingaleSettings = new MartingaleSettings();
                });
            }
        }
        public ICommand Save
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    StaticData.Data.MartingaleSettings = MartingaleSettings;
                    StaticData.Data.Save();
                });
            }
        }
        public ICommand Load
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    MartingaleSettings = StaticData.Data.MartingaleSettings;
                });
            }
        }
        public ICommand Start
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    
                    Navigation.ChangePage(PageFactory.Create(MartingaleSettings, Valute));
                }, 
                () => MartingaleSettings.Validate() && Valute != Currencies.None);
            }
        }
        public ICommand Back
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    Navigation.ChangePage(StaticData.SelectTacticPage);
                });
            }
        }
        public ICommand Calculate
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    MaxBetCount = getMaxCountBet(MartingaleSettings.StartBet.GetValueOrDefault(), MartingaleSettings.Factor.GetValueOrDefault(), Balance);
                }, ()=> MartingaleSettings.StartBet.GetValueOrDefault() > 0 && Balance > 0 && MartingaleSettings.Factor.GetValueOrDefault() > 0);
            }
        }

        int getMaxCountBet(double bet, double factor, double balance)
        {
            if (factor <= 1.0)
            {
                return 99999999;
            }

            var summ = bet;
            int i = 0;
            while (true)
            {
                if (summ < balance)
                {
                    i++;
                    switch (MartingaleSettings.Operation)
                    {
                        case Operation.Multiply:
                            summ *= MartingaleSettings.Factor.GetValueOrDefault();
                            break;
                        case Operation.Divide:
                            summ /= MartingaleSettings.Factor.GetValueOrDefault();
                            break;
                        case Operation.Add:
                            summ += MartingaleSettings.Factor.GetValueOrDefault();
                            break;
                        case Operation.Substract:
                            summ -= MartingaleSettings.Factor.GetValueOrDefault();
                            break;
                        default:
                            break;
                    }
                }
                else
                {
                    return i - 1;
                }
            }



        }

    }
}
