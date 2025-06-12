using System;
using System.Collections.Generic;
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

namespace Kasyno.Views
{
    /// <summary>
    /// Logika interakcji dla klasy BetDialog.xaml
    /// </summary>
    public partial class BetDialog : Window
    {
        public int EnteredBetAmount { get; private set; } = 0;

        public BetDialog()
        {
            InitializeComponent();
        }
        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(BetTextBox.Text, out int bet) && bet >= 10)
            {
                if (bet > App.User.Balance)
                {
                    ErrorDialog.ShowDialog("Nie masz wystarczających środków na koncie.", "Błąd");
                    return;
                }
                EnteredBetAmount = bet;
                DialogResult = true;
                Close();
            }
            else
            {
                ErrorDialog.ShowDialog("Wprowadź poprawną kwotę (min. 10), tylko liczby całkowite.", "Błąd");
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
