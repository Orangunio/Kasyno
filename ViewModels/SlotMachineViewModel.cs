using FontAwesome.WPF;
using Kasyno.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;

namespace Kasyno.ViewModels
{
    public class SlotMachineViewModel : INotifyPropertyChanged
    {
        private string _icon1;
        private string _icon2;
        private string _icon3;

        public string Icon1
        {
            get => _icon1;
            set { _icon1 = value; OnPropertyChanged(); }
        }

        public string Icon2
        {
            get => _icon2;
            set { _icon2 = value; OnPropertyChanged(); }
        }

        public string Icon3
        {
            get => _icon3;
            set { _icon3 = value; OnPropertyChanged(); }
        }

        public ICommand NewGameCommand { get; }
        public ICommand ExitCommand { get; }

        public List<string> AvailableIcons { get; } = new()
        {
            "Heart", "Star", "Bomb", "Globe", "Bug", "Key", "Plane", "UserSecret", "Rocket", "Hourglass", "Dollar"
        };

        public SlotMachineViewModel()
        {
            NewGameCommand = new RelayCommand(NewGame);
            ExitCommand = new RelayCommand(() => Application.Current.Shutdown());
            Icon1 = Icon2 = Icon3 = "Question"; // startowe ikony
        }

        private void NewGame()
        {
            var random = new Random();
            Icon1 = AvailableIcons[random.Next(AvailableIcons.Count)];
            Icon2 = AvailableIcons[random.Next(AvailableIcons.Count)];
            Icon3 = AvailableIcons[random.Next(AvailableIcons.Count)];

            if (Icon1 == Icon2 && Icon2 == Icon3)
            {
                MessageBox.Show("Wygrałeś!", "Gratulacje!", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
