using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace DiceBot.Service
{

    static class Navigation
    {
        public static Action<Page> ChangePage { get; set; }
    }
}
