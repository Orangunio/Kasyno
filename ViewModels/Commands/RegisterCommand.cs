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
    public class RegisterCommand : ICommand
    {
        public event EventHandler? CanExecuteChanged;
        public LoginVM LoginVM { get; set; }
        public RegisterCommand(LoginVM loginVM)
        {
            LoginVM = loginVM;
        }
        public bool CanExecute(object? parameter)
        {
            return true;
        }
        public async void Execute(object? parameter)
        {
            bool success = await LoginVM.RegisterAsync();
            if(success)
            {
                ErrorDialog.ShowDialog("Rejestracja powiodła się.", "Zarejestrowano");
            }
            else
            {
                ErrorDialog.ShowDialog("Rejestracja nie powiodła się. Spróbuj ponownie.", "Błąd rejestracji");
            }
        }
    }
}
