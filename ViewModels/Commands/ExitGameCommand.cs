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
        public event EventHandler? CanExecuteChanged;
        public BlackjackViewModel ViewModel { get; set; }
        public ExitGameCommand(BlackjackViewModel viewModel)
        {
            ViewModel = viewModel;
        }

        public bool CanExecute(object? parameter)
        {
            return true;
        }

        public void Execute(object? parameter)
        {
            var mainMenuWindow = new MainMenuView();
            mainMenuWindow.Show();
            foreach (Window window in Application.Current.Windows)
            {
                if (window is Views.Games.Blackjack)
                {
                    window.Close();
                    break;
                }
            }

        }
    }
}
