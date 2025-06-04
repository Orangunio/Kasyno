using Kasyno.Helpers;
using Kasyno.Models;
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
        public string SelectedLanguage
        {
            get => Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName;
            set
            {
                if (value != null)
                    LocalizationManager.ChangeLanguage(value);
            }
        }
        public User User => App.User;
        public BlackJackCommand BlackjackCommand { get;}
        public StatsCommand StatsCommand { get; set; }
        public MainMenuViewModel()
        {
            BlackjackCommand = new BlackJackCommand();
            StatsCommand = new StatsCommand();
        }

    }
}
