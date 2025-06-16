using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Kasyno.ViewModels.Commands.StatsCommands
{
    public class KantorCommand : ICommand
    {
        private readonly StatsViewModel viewModel;

        public KantorCommand(StatsViewModel viewModel)
        {
            this.viewModel = viewModel;
        }

        public event EventHandler? CanExecuteChanged;

        public bool CanExecute(object? parameter) => true;

        public void Execute(object? parameter)
        {
            viewModel.OpenKantorDialog();
        }
    }

}
