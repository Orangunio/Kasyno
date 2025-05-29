using Kasyno.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Kasyno.ViewModels.Commands.MainMenuCommands
{
    public class StatsCommand : ICommand
    {
        public event EventHandler? CanExecuteChanged;
        public StatsCommand()
        {
        }

        public bool CanExecute(object? parameter)
        {
            return true;
        }

        public void Execute(object? parameter)
        {
            var statsWindow = new StatsView();
            statsWindow.Show();

            // Zamknięcie aktualnego okna (menu głównego)
            foreach (Window window in Application.Current.Windows)
            {
                if (window is MainMenuView)
                {
                    window.Close();
                    break;
                }
            }
        }
    }
}
