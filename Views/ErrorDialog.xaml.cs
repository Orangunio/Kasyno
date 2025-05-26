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
    /// Logika interakcji dla klasy ErrorDialog.xaml
    /// </summary>
    public partial class ErrorDialog : Window
    {
        private static ErrorDialog _instance;
        private static bool _isOpen = false;
        public ErrorDialog()
        {
            InitializeComponent();

        }
        public static void ShowDialog(string message, string caption)
        {
            if (_isOpen)
                return;

            _instance = new ErrorDialog();
            _instance.MessageText.Text = message;
            _instance.CaptionText.Text = caption;
            _isOpen = true;
            _instance.ShowDialog();
        }
        protected override void OnClosed(System.EventArgs e)
        {
            _isOpen = false;
            base.OnClosed(e);
        }
        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
