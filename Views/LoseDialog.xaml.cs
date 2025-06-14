using System.Windows;

namespace Kasyno.Views.Dialogs
{
    public partial class LoseDialog : Window
    {
        public string Message { get; }

        public LoseDialog(string message = "Tym razem się nie udało. Spróbuj ponownie!")
        {
            InitializeComponent();
            Message = message;
            DataContext = this;
        }

        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
