using FontAwesome.WPF;
using Kasyno.Helpers;
using Kasyno.Views;
using Kasyno.Views.Games;
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

        private readonly Random _random = new();
        private readonly SlotMachineView _view;
        private int _lastBetAmount = 0;

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

        public SlotMachineViewModel(SlotMachineView view)
        {
            _view = view;

            Icon1 = Icon2 = Icon3 = "Question";

            NewGameCommand = new RelayCommand(NewGame);
            ExitCommand = new RelayCommand(() => Application.Current.Shutdown());
        }

        private void NewGame()
        {
            var betDialog = new BetDialog();
            bool? result = betDialog.ShowDialog();

            if (result == true)
            {
                _lastBetAmount = betDialog.EnteredBetAmount;

                // Uruchom animację losowania w widoku
                _view.StartSpin();
            }
        }

        public void ResolveGame()
        {
            if (Icon1 == Icon2 && Icon2 == Icon3)
            {
                int winAmount = _lastBetAmount * 25;
                MessageBox.Show($"🎉 Wygrałeś {winAmount} żetonów! 🎉", "Gratulacje!", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Brak wygranej. Spróbuj ponownie!", "Powodzenia następnym razem!", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        public string GetRandomIcon()
        {
            int index = _random.Next(AvailableIcons.Count);
            return AvailableIcons[index];
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
