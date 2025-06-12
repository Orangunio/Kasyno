using Kasyno.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Kasyno.ViewModels.Commands.StatsCommands
{
    public class ExitCommand : ICommand
    {
        private StatsViewModel ViewModels;
        public event EventHandler? CanExecuteChanged;
        public ExitCommand(StatsViewModel vm)
        {
            ViewModels = vm;
        }

        public bool CanExecute(object? parameter)
        {
           return true;
        }

        public void Execute(object? parameter)
        {
            if (parameter is Window window && window.DataContext is StatsViewModel vm)
            {
                vm.OnWindowClosing();
                window.Close();
            }
        }
    }
}
