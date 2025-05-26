using Kasyno.Models;
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
    public class LoginCommand : ICommand
    {
        public event EventHandler? CanExecuteChanged;
        public LoginVM LoginVM { get; set; }
        public LoginCommand(LoginVM loginVM)
        {
            LoginVM = loginVM;
        }
        public bool CanExecute(object? parameter)
        {
            return true;
        }

        public async void Execute(object? parameter)
        {
            bool success = await LoginVM.LoginAsync();
            if (success)
            {
                // Otwórz nowe okno
                var mainWindow = new MainMenuView();
                mainWindow.Show();

                // Zamknij okno logowania
                foreach (Window window in Application.Current.Windows)
                {
                    if (window is EntryWindow)
                    {
                        window.Close();
                        break;
                    }
                }
            }
            else
            {
                ErrorDialog.ShowDialog("Nieprawidłowy login lub hasło.", "Wystąpił błąd");
                
            }
        }
    }
}
