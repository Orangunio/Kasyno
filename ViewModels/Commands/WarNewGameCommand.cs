using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Kasyno.ViewModels.Commands
{
    public class WarNewGameCommand : ICommand
    {
        public event EventHandler? CanExecuteChanged;

        public WarViewModel ViewModel { get; set; }

        public WarNewGameCommand(WarViewModel viewModel)
        {
            ViewModel = viewModel;
        }

        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }

        public bool CanExecute(object? parameter)
        {
            return true;
        }

        public void Execute(object? parameter)
        {
            if (ViewModel.BetAmount <= 0)
            {
                var betDialog = new Views.BetDialog();
                bool? result = betDialog.ShowDialog();

                if (result == true)
                {
                    ViewModel.BetAmount = betDialog.EnteredBetAmount;
                }
                else
                {
                    return;
                }
            }

            ViewModel.NewGame();
        }
    }
}
