using Kasyno.Helpers;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Kasyno.ViewModels
{
    internal class SlotMachineViewModel : INotifyPropertyChanged
    {
        private static readonly string[] _availableIcons =
        {
            "Heart", "Star", "WebAwesome", "Bomb", "Ghost", "Lemon", "Snowflake", "Fish", "UserSecret"
        };

        private readonly Random _random = new();

        private string _icon1 = "QuestionCircle";
        private string _icon2 = "QuestionCircle";
        private string _icon3 = "QuestionCircle";

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

        public SlotMachineViewModel()
        {
            NewGameCommand = new RelayCommand(_ => StartNewGame());
            ExitCommand = new RelayCommand(ExecuteExit);
        }

        private async void StartNewGame()
        {
            // Na początek ustaw znaki zapytania
            Icon1 = "QuestionCircle";
            Icon2 = "QuestionCircle";
            Icon3 = "QuestionCircle";

            // Małe opóźnienie, aby UI się odświeżył przed losowaniem
            await Task.Delay(300);

            Icon1 = GetRandomIcon();
            await Task.Delay(400);

            Icon2 = GetRandomIcon();
            await Task.Delay(400);

            Icon3 = GetRandomIcon();
        }

        private string GetRandomIcon()
        {
            int index = _random.Next(_availableIcons.Length);
            return _availableIcons[index];
        }

        private void ExecuteExit(object? obj)
        {
            foreach (Window window in Application.Current.Windows)
            {
                if (window is Window current && current.Title == "Automaty")
                {
                    var mainMenu = new Views.MainMenuView();
                    mainMenu.Show();
                    current.Close();
                    break;
                }
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string? name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
