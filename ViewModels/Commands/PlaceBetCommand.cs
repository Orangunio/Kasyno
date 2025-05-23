using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Kasyno.ViewModels.Commands
{
    public class PlaceBetCommand : ICommand
    {
        public event EventHandler? CanExecuteChanged;
        public BlackjackViewModel ViewModel { get; set; }
        public PlaceBetCommand(BlackjackViewModel viewModel)
        {
            ViewModel = viewModel;
        }
        public bool CanExecute(object? parameter)
        {
            return true;
        }

        public void Execute(object? parameter)
        {
            if (parameter is string input && int.TryParse(input, out int bet))
            {
                ViewModel.PlaceBet(bet);
            }
            else
            {
                MessageBox.Show("Wprowadź poprawną wartość zakładu.");
            }
        }
    }
}
