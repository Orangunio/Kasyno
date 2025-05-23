using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Kasyno.ViewModels.Commands
{
    public class DoubleDownCommand : ICommand
    {
        public event EventHandler? CanExecuteChanged;
        public BlackjackViewModel ViewModel { get; set; }
        public DoubleDownCommand(BlackjackViewModel viewModel)
        {
            ViewModel = viewModel;
        }
        public bool CanExecute(object? parameter)
        {
            return true;
        }
        public void Execute(object? parameter)
        {
            ViewModel.DoubleDown();
        }
    }
}
