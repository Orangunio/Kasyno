using Kasyno.ViewModels.Commands.MainMenuCommands;
using Kasyno.Views.Games;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kasyno.ViewModels
{
    public class MainMenuViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        }
        public BlackJackCommand BlackjackCommand { get;}
        public MainMenuViewModel()
        {
            BlackjackCommand = new BlackJackCommand(this);
        }

    }
}
