using Kasyno.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Kasyno.ViewModels.Commands.StatsCommands
{
    public class AddMoneyCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;
        private StatsViewModel ViewModels;
        public AddMoneyCommand(StatsViewModel vm)
        {
            ViewModels = vm;
        }
        public bool CanExecute(object parameter)
        {
            return true;
        }
        public void Execute(object parameter)
        {
            var dialog = new AddMoneyDialog();
            if (dialog.ShowDialog() == true)
            {
                ViewModels.AddMoney(dialog.AmountToAdd);
                ErrorDialog.ShowDialog($"Dodano {dialog.AmountToAdd} do konta {ViewModels.User.Username}.", "Sukces");
            }
        }
    }
}
