using FontAwesome.WPF;
using Kasyno.Helpers;
using Kasyno.Views;
using Kasyno.Views.Dialogs;
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

            // Ustawienie domyślnych ikon
            Icon1 = Icon2 = Icon3 = "Question";

            NewGameCommand = new RelayCommand(NewGame);
            ExitCommand = new RelayCommand(() => Application.Current.Shutdown());
        }

        private void NewGame()
        {
            var betDialog = new BetDialog();
            bool? result = betDialog.ShowDialog();

            if (result == true && betDialog.EnteredBetAmount >= 10)
            {
                _lastBetAmount = betDialog.EnteredBetAmount;

                // Rozpoczęcie animacji losowania
                _view.StartSpin();
            }
            else
            {
                MessageBox.Show("Wprowadź poprawną stawkę (min. 10).", "Błąd", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        public void ResolveGame()
        {
            if (Icon1 == Icon2 && Icon2 == Icon3)
            {
                int winAmount = _lastBetAmount * 25;
                var winDialog = new WinDialog($"🎉 Wygrałeś {winAmount} żetonów! 🎉");
                winDialog.ShowDialog();
            }
            else
            {
                var loseDialog = new LoseDialog();
                loseDialog.ShowDialog();
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
