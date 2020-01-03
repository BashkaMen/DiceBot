using DevExpress.Mvvm;
using Dice.Client.Web;
using DiceBot.Model;
using DiceBot.Service;
using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

namespace DiceBot.ViewModel
{
    class GameViewModel : ViewModelBase
    {
        public double Balance { get; set; }
        public int BetCount { get; set; }
        public double Profit { get; set; }
        public int WinCount { get; set; }
        public int LoseCount { get; set; }
        public ChartValues<double> Points { get; set; }
        public ObservableCollection<GameResult> Games { get; set; }
        public Currencies Valute { get; set; }
        public MartingaleSettings Settings { get; set; }
        public bool StopIfWinFlag { get; set; }
        public bool ChartInfo { get; set; } = true;

        public GameViewModel()
        {
            Points = new ChartValues<double>();
            Games = new ObservableCollection<GameResult>();
            BindingOperations.EnableCollectionSynchronization(Games, new object());
        }

        AsyncCommand _Start;
        public AsyncCommand Start
        {
            get
            {
                return _Start ?? (_Start = new AsyncCommand(async () =>
                {
                    StopIfWinFlag = false;

                    var balance = await DiceWebAPI.GetBalanceAsync(StaticData.Data.CurrentSession, Valute);
                    Balance = (double)balance.Balance;

                    var r = new Random();
                    var bet = Settings.StartBet.GetValueOrDefault();
                    var winPercent = Settings.WinPercent.GetValueOrDefault();

                    while (!Start.IsCancellationRequested)
                    {

                        var max = 1000000 / 100 * winPercent - 1;

                        var game = await DiceWebAPI.PlaceBetAsync(StaticData.Data.CurrentSession, (decimal)bet, 0, (long)max, r.Next(), Valute);

                        if (game?.Success == true)
                        {
                            BetCount++;

                            var result = new GameResult
                            {
                                Bet = bet,
                                Profit = game.PayOut > 0 ? (double)game.PayOut - bet : -bet,
                                Result = game.PayOut > 0
                            };

                            await AddGame(result);
                            if (ChartInfo) await AddPoint(Profit);


                            Balance += result.Profit;
                            Profit += result.Profit;

                            if (result.Result)
                            {
                                WinCount++;

                                winPercent = Settings.WinPercent.GetValueOrDefault();
                                bet = Settings.StartBet.GetValueOrDefault();

                                if (StopIfWinFlag)
                                {
                                    StopIfWinFlag = false;
                                    return;
                                }
                            }
                            else
                            {
                                LoseCount++;

                                switch (Settings.Operation)
                                {
                                    case Operation.Multiply:
                                        bet *= Settings.Factor.GetValueOrDefault();
                                        break;
                                    case Operation.Divide:
                                        bet /= Settings.Factor.GetValueOrDefault();
                                        break;
                                    case Operation.Add:
                                        bet += Settings.Factor.GetValueOrDefault();
                                        break;
                                    case Operation.Substract:
                                        bet -= Settings.Factor.GetValueOrDefault();
                                        break;
                                    default:
                                        break;
                                }

                                if (Settings.CanChangeWinPercent)
                                {
                                    winPercent = Settings.ChangeWinPercent.GetValueOrDefault();
                                }

                                if (bet >= Settings.MaxBet.GetValueOrDefault())
                                {
                                    return;
                                }

                            }
                        }
                        else
                        {
                            if (game?.RateLimited == false)
                            {
                                if (game?.InsufficientFunds == true)
                                    MessageBox.Show("Недостаточно средств");
                                if (game?.MaxPayoutExceeded == true)
                                    MessageBox.Show("Превышение максимальной суммы выплат");
                                if (game?.NoPossibleProfit == true)
                                    MessageBox.Show("Нет возможной прибыли");
                                if (game?.ChanceTooHigh == true)
                                    MessageBox.Show("Шанс победы слишком высок");
                                if (game?.ChanceTooLow == true)
                                    MessageBox.Show("Шанс победы слишком низок");
                                return;
                            }
                            else
                            {
                                Console.WriteLine("Rate!");
                            }
                        }



                    }

                }));
            }
        }

        public ICommand StopIfWin
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    StopIfWinFlag = true;
                },
                () => Start.IsExecuting && !StopIfWinFlag);
            }
        }

        public ICommand Back
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    Navigation.ChangePage(StaticData.SelectTacticPage);
                },
                () => !Start.IsExecuting);
            }
        }


        public async Task AddPoint(double value)
        {
            await Task.Factory.StartNew(() =>
            {
                Points.Add(value);

                if (Points.Count > 500)
                {
                    Points.RemoveAt(0);
                }
            });
        }

        public async Task AddGame(GameResult game)
        {
            await Task.Factory.StartNew(() =>
            {
                Games.Insert(0, game);

                if (Games.Count > 500)
                {
                    Games.RemoveAt(500);
                }
            });
        }

    }
}
