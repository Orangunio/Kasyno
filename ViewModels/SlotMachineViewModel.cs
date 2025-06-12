using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kasyno.Views; 
using Kasyno.Helpers;
using System.Windows;
using System.Windows.Input;

namespace Kasyno.ViewModels
{
    internal class SlotMachineViewModel
    {
        public ICommand ExitCommand { get; }

        public SlotMachineViewModel()
        {
            ExitCommand = new RelayCommand(ExecuteExit);
        }

        private void ExecuteExit(object? obj)
        {
            foreach (Window window in Application.Current.Windows)
            {
                if (window is Window current && current.Title == "Automaty")
                {
                    var mainMenu = new MainMenuView(); // <-- dostosuj jeśli nazywa się inaczej
                    mainMenu.Show();
                    current.Close();
                    break;
                }
            }
        }
    }
}
