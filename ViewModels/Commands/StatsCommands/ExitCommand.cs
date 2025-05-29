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
        public event EventHandler? CanExecuteChanged;
        public ExitCommand()
        {
        }

        public bool CanExecute(object? parameter)
        {
           return true;
        }

        public void Execute(object? parameter)
        {
            var mainMenu = new MainMenuView();
            mainMenu.Show();

            // Zamknięcie aktualnego okna (menu głównego)
            foreach (Window window in Application.Current.Windows)
            {
                if (window is StatsView)
                {
                    window.Close();
                    break;
                }
            }
        }
    }
}
