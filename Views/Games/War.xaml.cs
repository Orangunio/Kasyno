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
    /// Logika interakcji dla klasy War.xaml
    /// </summary>
    public partial class War : Window
    {
        public War()
        {
            InitializeComponent();
            Closing += War_Closing;
        }

        public object DeckPile { get; internal set; }

        private void War_Closing(object? sender, CancelEventArgs e)
        {
            if (DataContext is WarViewModel vm)
            {
                vm.OnWindowClosing();
            }
        }

        
    }
}
