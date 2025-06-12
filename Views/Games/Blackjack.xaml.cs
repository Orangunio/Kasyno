using Kasyno.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Kasyno.Views.Games
{
    /// <summary>
    /// Logika interakcji dla klasy Blackjack.xaml
    /// </summary>
    public partial class Blackjack : Window
    {
        public Blackjack()
        {
            InitializeComponent();
            this.Closing += Blackjack_Closing;
        }
        private void Blackjack_Closing(object? sender, CancelEventArgs e)
        {
            if (DataContext is BlackjackViewModel vm)
            {
                vm.OnWindowClosing();
            }
        }
    }
}
