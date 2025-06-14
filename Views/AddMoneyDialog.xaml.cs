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
    /// Logika interakcji dla klasy AddMoneyDialog.xaml
    /// </summary>
    public partial class AddMoneyDialog : Window
    {
        public int AmountToAdd { get; private set; }
        public AddMoneyDialog()
        {
            InitializeComponent();
        }
        private void Add_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(AmountTextBox.Text, out int amount) && amount > 0)
            {
                AmountToAdd = amount;
                DialogResult = true;
                Close();
            }
            else
            {
                ErrorDialog.ShowDialog("Wprowadź poprawną dodatnią kwotę. (liczby całkowite)", "Błąd");
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
