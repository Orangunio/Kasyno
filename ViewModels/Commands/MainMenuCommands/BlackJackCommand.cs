using Kasyno.Views;
using Kasyno.Views.Games;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Kasyno.ViewModels.Commands.MainMenuCommands
{
    public class BlackJackCommand : ICommand
    {
        public event EventHandler? CanExecuteChanged;
        public MainMenuViewModel ViewModel { get; set; }
        public BlackJackCommand(MainMenuViewModel viewModel)
        {
            ViewModel = viewModel;
        }

        public bool CanExecute(object? parameter)
        {
            return true;
        }

        public void Execute(object? parameter)
        {
            var blackjackWindow = new Blackjack();
            blackjackWindow.Show();

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
