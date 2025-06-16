using Kasyno.Helpers;
using Kasyno.Models;
using Kasyno.ViewModels.Commands.StatsCommands;
using Kasyno.Views;
using LiveCharts;
using LiveCharts.Wpf;
using Newtonsoft.Json;
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
        public ObservableCollection<string> AvailableCurrencies { get; set; } = new();
        private string selectedCurrency;
        public string SelectedCurrency
        {
            get => selectedCurrency;
            set
            {
                selectedCurrency = value;
                OnPropertyChanged(nameof(SelectedCurrency));
                OnPropertyChanged(nameof(ConvertedAmount));
                OnPropertyChanged(nameof(SelectedCurrencySymbol));
            }
        }

        public Dictionary<string, decimal> CurrencyRates { get; set; } = new();

        public double ConvertedAmount =>
            CurrencyRates.TryGetValue(SelectedCurrency, out var rate) && rate > 0
                ? User.Balance / (double)rate
                : 0;

        public string SelectedCurrencySymbol =>
            SelectedCurrency switch
            {
                "USD" => "$",
                "EUR" => "€",
                "GBP" => "£",
                "CHF" => "₣",
                "JPY" => "¥",
                _ => SelectedCurrency // fallback: symbol = kod waluty
            };
        public ObservableCollection<StatsHelper> Bets { get; set; } = new();
        public User User => App.User;
        public int TotalGames => Bets.Count;
        public int Wins => Bets.Count(b => b.Outcome == "W");
        public int Losses => Bets.Count(b => b.Outcome == "L");
        public int TotalWon => Bets.Where(b => b.Outcome == "W").Sum(b => b.ResultAmount);
        public int TotalLost => Bets.Where(b => b.Outcome == "L").Sum(b => b.Amount);
        public SeriesCollection PieSeries { get; set; }
        public ExitCommand ExitCommand { get; }
        public KantorCommand KantorCommand { get; }
        public AddMoneyCommand AddMoneyCommand { get; }
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
            KantorCommand = new KantorCommand(this);
            ExitCommand = new ExitCommand(this);
            AddMoneyCommand = new AddMoneyCommand(this);
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
        public void AddMoney(int amount)
        {
            User.Balance += amount;
            DataHelper.UpdateAsync(User);
            OnPropertyChanged(nameof(User));
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

        public async void OpenKantorDialog()
        {
            try
            {
                using var httpClient = new System.Net.Http.HttpClient();
                string url = "https://api.nbp.pl/api/exchangerates/tables/a/?format=json";
                var response = await httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
                dynamic data = JsonConvert.DeserializeObject(json);

                CurrencyRates.Clear();
                AvailableCurrencies.Clear();

                foreach (var rate in data[0].rates)
                {
                    string code = rate.code;
                    decimal mid = rate.mid;
                    CurrencyRates[code] = mid;
                    AvailableCurrencies.Add(code);
                }

                SelectedCurrency = AvailableCurrencies.FirstOrDefault();

                OnPropertyChanged(nameof(CurrencyRates));
                OnPropertyChanged(nameof(AvailableCurrencies));
                OnPropertyChanged(nameof(SelectedCurrency));
                OnPropertyChanged(nameof(ConvertedAmount));

                var dialog = new Views.KantorDialog
                {
                    DataContext = this
                };
                dialog.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Błąd pobierania kursów walut: " + ex.Message);
            }
        }



    }
}
