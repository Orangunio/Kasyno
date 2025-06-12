using Kasyno.Helpers;
using Kasyno.Models;
using Kasyno.ViewModels.Commands.StatsCommands;
using Kasyno.Views;
using LiveCharts;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace Kasyno.ViewModels
{
    public class StatsViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<StatsHelper> Bets { get; set; } = new();
        public User User => App.User;
        public int TotalGames => Bets.Count;
        public int Wins => Bets.Count(b => b.Outcome == "W");
        public int Losses => Bets.Count(b => b.Outcome == "L");
        public int TotalWon => Bets.Where(b => b.Outcome == "W").Sum(b => b.ResultAmount);
        public int TotalLost => Bets.Where(b => b.Outcome == "L").Sum(b => b.Amount);
        public SeriesCollection PieSeries { get; set; }
        public ExitCommand ExitCommand { get; }
        private bool isClosingHandled = false;

        public void OnWindowClosing()
        {
            if (isClosingHandled) return;
            isClosingHandled = true;

            bool isMainMenuOpen = Application.Current.Windows
                .OfType<MainMenuView>()
                .Any(w => w.IsVisible);

            if (!isMainMenuOpen)
            {
                var mainMenu = new MainMenuView();
                mainMenu.Show();
            }
        }
        public StatsViewModel()
        {
            ExitCommand = new ExitCommand(this);
            _ = LoadHistoryAsync();
            PieSeries = new SeriesCollection
        {
            new PieSeries
            {
                Title = "W",
                Values = new ChartValues<int> { Wins },
                DataLabels = true,
                Fill = Brushes.Green
            },
            new PieSeries
            {
                Title = "L",
                Values = new ChartValues<int> { Losses },
                DataLabels = true,
                Fill = Brushes.Red
            }
        };
        }

        private async Task LoadHistoryAsync()
        {
            var allBets = await DataHelper.GetAllAsync<Bet>();
            var allResults = await DataHelper.GetAllAsync<Result>();
            var allSessions = await DataHelper.GetAllAsync<GameSession>();

            var userId = App.User.Id;
            var userSessions = allSessions.Where(s => s.UserId == userId).ToList();

            var filteredBets = allBets.Where(b => userSessions.Any(s => s.Id == b.GameSessionId)).ToList();

            foreach (var bet in filteredBets)
            {
                var result = allResults.FirstOrDefault(r => r.Id == bet.ResultId);
                var session = allSessions.FirstOrDefault(s => s.Id == bet.GameSessionId);
                if (result != null && session != null)
                {
                    Bets.Add(new StatsHelper
                    {
                        Amount = bet.Amount,
                        Description = bet.Description,
                        Outcome = result.Outcome,
                        ResultAmount = result.Amount,
                        GameDate = session.EndTime
                    });
                }
            }

            OnPropertyChanged(nameof(TotalGames));
            OnPropertyChanged(nameof(Wins));
            OnPropertyChanged(nameof(Losses));
            OnPropertyChanged(nameof(TotalWon));
            OnPropertyChanged(nameof(TotalLost));
            PieSeries[0].Values = new ChartValues<int> { Wins };
            PieSeries[1].Values = new ChartValues<int> { Losses };
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
