using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Kasyno.ViewModels.Commands
{
    public class HitCommand : ICommand
    {

        public event EventHandler? CanExecuteChanged;
        public BlackjackViewModel ViewModel { get; set; }
        public HitCommand(BlackjackViewModel viewModel)
        {
            ViewModel = viewModel;
        }

        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }

        public bool CanExecute(object? parameter)
        {
            if(ViewModel.Result == string.Empty && ViewModel.BetAmount != 0 && !ViewModel.IsAnimating)
            {
                return true;
            }
            else return false;
        }

        public void Execute(object? parameter)
        {
            ViewModel.Hit();
        }
    }
}
