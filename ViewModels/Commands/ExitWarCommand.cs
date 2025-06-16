using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Kasyno.ViewModels.Commands
{
    public class ExitWarCommand : ICommand
    {
        public event EventHandler? CanExecuteChanged;
        private WarViewModel viewModel;
        public ExitWarCommand(WarViewModel vm)
        {
            viewModel = vm;
        }
        public bool CanExecute(object? parameter)
        {
            return true;
        }

        public void Execute(object? parameter)
        {
            if (parameter is Window window && window.DataContext is WarViewModel vm)
            {
                vm.OnWindowClosing();
                window.Close();
            }
        }
    }
}
