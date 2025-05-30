using Kasyno.Helpers;
using Kasyno.Models;
using Kasyno.ViewModels.Commands.StatsCommands;
using LiveCharts;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Kasyno.ViewModels
{
    public class StatsViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<StatsHelper> Bets { get; set; } = new();
        public User User => App.User;
        public int TotalGames => Bets.Count;
        public int Wins => Bets.Count(b => b.Outcome == "Wygrana");
        public int Losses => Bets.Count(b => b.Outcome == "Przegrana");
        public int TotalWon => Bets.Where(b => b.Outcome == "Wygrana").Sum(b => b.ResultAmount);
        public int TotalLost => Bets.Where(b => b.Outcome == "Przegrana").Sum(b => b.Amount);
        public SeriesCollection PieSeries { get; set; }
        public ExitCommand ExitCommand { get; }
        public StatsViewModel()
        {
            ExitCommand = new ExitCommand();
            _ = LoadHistoryAsync();
            PieSeries = new SeriesCollection
        {
            new PieSeries
            {
                Title = "Wygrane",
                Values = new ChartValues<int> { 12 },
                DataLabels = true,
                Fill = Brushes.Green
            },
            new PieSeries
            {
                Title = "Przegrane",
                Values = new ChartValues<int> { 8 },
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
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
