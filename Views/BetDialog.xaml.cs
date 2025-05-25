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
        public int? BetAmount { get; set; }
        public BetDialog()
        {
            InitializeComponent();
        }
        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(BetTextBox.Text, out int value) && value >= 10)
            {
                BetAmount = value;
                DialogResult = true;
            }
            else
            {
                MessageBox.Show("Wprowadź prawidłową stawkę (minimum 10).");
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
