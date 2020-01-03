using DevExpress.Mvvm;
using DiceBot.Model;
using DiceBot.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace DiceBot.ViewModel
{
    class MainViewModel : ViewModelBase
    {
        #region WindowControl
        public ICommand DragMove
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    try
                    {
                        var w = App.Current.MainWindow;

                        w.DragMove();
                    }
                    catch { }

                });
            }
        }
        public ICommand Close
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    App.Current.MainWindow.Close();
                });
            }
        }
        public ICommand Minimize
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    App.Current.MainWindow.WindowState = System.Windows.WindowState.Minimized;
                });
            }
        }
        public ICommand Maximize
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    if (App.Current.MainWindow.WindowState == System.Windows.WindowState.Normal)
                    {
                        App.Current.MainWindow.WindowState = System.Windows.WindowState.Maximized;
                    }
                    else
                    {
                        App.Current.MainWindow.WindowState = System.Windows.WindowState.Normal;
                    }
                });
            }
        }
        #endregion

        public Page CurrentPage { get; set; }

        public MainViewModel()
        {
            Navigation.ChangePage = (p) => CurrentPage = p;


            Navigation.ChangePage(StaticData.HelloPage);

            //Properties.Settings.Default.Reset();
        }

    }
}
