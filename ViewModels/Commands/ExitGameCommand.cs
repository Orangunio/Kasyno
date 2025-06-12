using Kasyno.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Kasyno.ViewModels.Commands
{
    public class ExitGameCommand : ICommand
    {
        private BlackjackViewModel ViewModel;
        public event EventHandler? CanExecuteChanged;
        public ExitGameCommand(BlackjackViewModel vm)
        {
            ViewModel = vm;
        }

        public bool CanExecute(object? parameter)
        {
            return true;
        }

        public void Execute(object? parameter)
        {
            if (parameter is Window window && window.DataContext is BlackjackViewModel vm)
            {
                vm.OnWindowClosing();
                window.Close(); 
            }

        }
    }
}
