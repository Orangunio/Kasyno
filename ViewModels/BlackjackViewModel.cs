using Kasyno.Helpers;
using Kasyno.Models;
using Kasyno.ViewModels.Commands;
using Kasyno.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Security.RightsManagement;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Kasyno.ViewModels
{
    public class BlackjackViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<Card> PlayerCards { get; } = new();
        public ObservableCollection<Card> DealerCards { get; } = new();
        private Deck deck;
        public User User => App.User;
        private GameSession? currentSession;
        public string desc = "Blackjack";
        private string result = string.Empty;
        public string ResultText
        {
            get
            {
                string? key = Result switch
                {
                    "W" => "Win",
                    "L" => "Loss",
                    _ => null
                };

                return key != null && Application.Current.Resources.Contains(key)
                    ? Application.Current.Resources[key] as string ?? string.Empty
                    : string.Empty;
            }
        }

        public string Result
        {
            get => result;
            set
            {
                result = value;
                OnPropertyChanged(nameof(Result));
                HitCommand.RaiseCanExecuteChanged();
                StandCommand.RaiseCanExecuteChanged();
                OnPropertyChanged(nameof(ResultText));
            }
        }
        private int betAmount = 0;
        public int BetAmount
        {
            get => betAmount;
            set
            {
                betAmount = value;
                OnPropertyChanged(nameof(BetAmount));
                NewGameCommand.RaiseCanExecuteChanged();
                HitCommand.RaiseCanExecuteChanged();
                StandCommand.RaiseCanExecuteChanged();
            }
        }
        private Visibility doubleDownVisible;

        public Visibility DoubleDownVisible
        {
            get { return doubleDownVisible; }
            set {
                doubleDownVisible = value;
                OnPropertyChanged(nameof(DoubleDownVisible));
            }
        }
        public int piniondz;
        private bool isDealerTurnOver = false;
        public bool IsDealerTurnOver
        {
            get => isDealerTurnOver;
            set
            {
                isDealerTurnOver = value;
                OnPropertyChanged(nameof(IsDealerTurnOver));
                OnPropertyChanged(nameof(DealerValue));
            }
        }


        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        }
        public int PlayerValue => CalculateValue(PlayerCards);
        public int DealerValue
        {
            get
            {
                var cards = IsDealerTurnOver ? DealerCards : new ObservableCollection<Card> { DealerCards[0] };
                return CalculateValue(cards);
            }
        }
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

        public HitCommand HitCommand { get; }
        public StandCommand StandCommand { get; }
        public NewGameCommand NewGameCommand { get; }
        public ExitGameCommand ExitGameCommand { get; }
        public DoubleDownCommand DoubleDownCommand { get; }
        public BlackjackViewModel() 
        {
            HitCommand = new HitCommand(this);
            StandCommand = new StandCommand(this);
            NewGameCommand = new NewGameCommand(this);
            ExitGameCommand = new ExitGameCommand(this);
            DoubleDownCommand = new DoubleDownCommand(this);
            doubleDownVisible = Visibility.Collapsed;
        }
        public async void NewGame()
        {
            IsDealerTurnOver = false;
            User.Balance -= BetAmount;
            deck = new Deck();
            PlayerCards.Clear();
            DealerCards.Clear();
            Result = string.Empty;
            PlayerCards.Add(deck.DrawCard());
            DealerCards.Add(deck.DrawCard());
            PlayerCards.Add(deck.DrawCard());
            DealerCards.Add(deck.DrawCard());
            DoubleDownVisibility();
            OnPropertyChanged(nameof(PlayerValue));
            OnPropertyChanged(nameof(DealerValue));
            currentSession = new GameSession(User.Id, DateTime.Now, DateTime.Now);
            await DataHelper.InsertAsync(currentSession);
            if (PlayerValue == 21)
            {
                Stand();
            }
        }
        public async void Hit()
        {
            PlayerCards.Add(deck.DrawCard());
            OnPropertyChanged(nameof(PlayerValue));
            DoubleDownVisibility();
            if (PlayerValue > 21)
            {
                piniondz = 0;
                DataHelper.UpdateAsync(User);
                Result = "L";
                if (currentSession != null)
                {
                    currentSession.EndTime = DateTime.Now;
                    await DataHelper.UpdateAsync(currentSession);

                    var resultEntry = new Result(Result, piniondz);
                    await DataHelper.InsertAsync(resultEntry);

                    var betEntry = new Bet(currentSession.Id, BetAmount, resultEntry.Id, desc);
                    await DataHelper.InsertAsync(betEntry);
                }
                BetAmount = 0;
                OnPropertyChanged(nameof(BetAmount));
                NewGameCommand.RaiseCanExecuteChanged();
            }
            if (PlayerValue == 21)
            {
                Stand();
            }
        }
        public async void Stand()
        {
            DoubleDownVisibility();
            IsDealerTurnOver = true;
            while (DealerValue < 17 && DealerValue != PlayerValue)
            {
                DealerCards.Add(deck.DrawCard());
                OnPropertyChanged(nameof(DealerValue));
            }
            Result = PlayerValue > 21 ? "L"
                          : DealerValue > 21 ? "W"
                          : PlayerValue > DealerValue ? "W"
                          : "L";
            if (Result == "W")
            {
                piniondz = BetAmount;
                User.Balance += BetAmount * 2;
            }
            else
            {
                piniondz = 0;
            }
            if (currentSession != null)
            {
                currentSession.EndTime = DateTime.Now;
                await DataHelper.UpdateAsync(currentSession);

                var resultEntry = new Result(Result, piniondz);
                await DataHelper.InsertAsync(resultEntry);

                var betEntry = new Bet(currentSession.Id, BetAmount, resultEntry.Id, desc);
                await DataHelper.InsertAsync(betEntry);
            }
            BetAmount = 0;
            OnPropertyChanged(nameof(BetAmount));
            DataHelper.UpdateAsync(User);
            NewGameCommand.RaiseCanExecuteChanged();
        }
        public void DoubleDown()
        {
            desc = "Blackjack - Double Down";
            User.Balance -= BetAmount;
            BetAmount *= 2;
            Hit();
            Stand();
        }
        public void DoubleDownVisibility()
        {
            if (PlayerCards.Count == 2 && PlayerValue < 21 && Result == string.Empty && DoubleDownVisible == Visibility.Collapsed)
            {
                DoubleDownVisible = Visibility.Visible;
            }
            else
            {
                DoubleDownVisible = Visibility.Collapsed;
            }
        }
        private int CalculateValue(ObservableCollection<Card> cards)
        {
            int sum = cards.Sum(c => c.Value);
            int aceCount = cards.Count(c => c.Rank == "A");

            while (sum > 21 && aceCount-- > 0)
                sum -= 10;

            return sum;
        }


    }
}
