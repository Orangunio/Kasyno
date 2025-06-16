using Kasyno.Helpers;
//using Kasyno.Extensions;
using Kasyno.Models;
using Kasyno.ViewModels.Commands;
using Kasyno.Views;
using System;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Kasyno.ViewModels
{
    public class WarViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        private int _playerDeckCount;
        public int PlayerDeckCount
        {
            get => _playerDeckCount;
            set
            {
                _playerDeckCount = value;
                OnPropertyChanged(nameof(PlayerDeckCount));
            }
        }

        private int _dealerDeckCount;
        public int DealerDeckCount
        {
            get => _dealerDeckCount;
            set
            {
                _dealerDeckCount = value;
                OnPropertyChanged(nameof(DealerDeckCount));
            }
        }

        private Deck deck;
        public User User => App.User;

        public Card PlayerCard { get; set; }
        public Card DealerCard { get; set; }

        private int betAmount = 0;
        public int BetAmount
        {
            get => betAmount;
            set
            {
                betAmount = value;
                OnPropertyChanged(nameof(BetAmount));
            }
        }

        private string result = string.Empty;
        public string Result
        {
            get => result;
            set
            {
                result = value;
                OnPropertyChanged(nameof(Result));
                OnPropertyChanged(nameof(ResultText));
            }
        }

        public string ResultText
        {
            get
            {
                return Result switch
                {
                    "W" => "Wygrana!",
                    "L" => "Przegrana!",
                    _ => string.Empty
                };
            }
        }

        private bool isDealerTurnOver = true;
        public bool IsDealerTurnOver
        {
            get => isDealerTurnOver;
            set
            {
                isDealerTurnOver = value;
                OnPropertyChanged(nameof(IsDealerTurnOver));
            }
        }

        private bool isAnimating = false;
        public bool IsAnimating
        {
            get => isAnimating;
            set
            {
                if (isAnimating != value)
                {
                    isAnimating = value;
                    OnPropertyChanged(nameof(IsAnimating));
                    NewGameCommand.RaiseCanExecuteChanged();
                }
            }
        }

        // Komendy
        public ExitWarCommand ExitGameCommand { get; }
        public WarNewGameCommand NewGameCommand { get; }
        public WarPlayCommand PlayCommand { get; }
        public bool CanPlay { get; internal set; }

      

        public WarViewModel()
        {
            ExitGameCommand = new ExitWarCommand(this);
            NewGameCommand = new WarNewGameCommand(this);
            PlayCommand = new WarPlayCommand(this);
        }

        public async Task PlayRound()
        {
            IsAnimating = true;
            if (User.Balance < BetAmount)
            {
                MessageBox.Show("Nie masz wystarczających środków!", "Błąd", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Odejmujemy stawkę raz na początku
            User.Balance -= BetAmount;
            await DataHelper.UpdateAsync(User);

            deck = new Deck();
            deck.Shuffle();

            // Podział talii
            var playerDeck = new Queue<Card>();
            var dealerDeck = new Queue<Card>();

            var shuffledCards = deck.Cards.ToList();
            for (int i = 0; i < shuffledCards.Count; i++)
            {
                if (i % 2 == 0)
                    playerDeck.Enqueue(shuffledCards[i]);
                else
                    dealerDeck.Enqueue(shuffledCards[i]);
            }

            int roundCounter = 0;

            while (playerDeck.Count > 0 && dealerDeck.Count > 0)
            {
                roundCounter++;
                await Task.Delay(500);

                var playerCard = playerDeck.Dequeue();
                var dealerCard = dealerDeck.Dequeue();

                PlayerCard = playerCard;
                DealerCard = dealerCard;

                OnPropertyChanged(nameof(PlayerCard));
                OnPropertyChanged(nameof(DealerCard));

                var winnings = new List<Card> { playerCard, dealerCard };

                if (playerCard.Value > dealerCard.Value)
                {
                    foreach (var card in winnings)
                        playerDeck.Enqueue(card);
                }
                else if (dealerCard.Value > playerCard.Value)
                {
                    foreach (var card in winnings)
                        dealerDeck.Enqueue(card);
                }
                else
                {
                    // Remis: wojna!
                    var warPile = new List<Card>(winnings);
                    bool warResolved = false;

                    while (!warResolved)
                    {
                        if (playerDeck.Count < 4 || dealerDeck.Count < 4)
                        {
                            // Któryś z graczy nie ma wystarczająco kart
                            warResolved = true;
                            break;
                        }

                        for (int i = 0; i < 3; i++)
                        {
                            warPile.Add(playerDeck.Dequeue());
                            warPile.Add(dealerDeck.Dequeue());
                        }

                        var playerWarCard = playerDeck.Dequeue();
                        var dealerWarCard = dealerDeck.Dequeue();
                        warPile.Add(playerWarCard);
                        warPile.Add(dealerWarCard);

                        PlayerCard = playerWarCard;
                        DealerCard = dealerWarCard;
                        OnPropertyChanged(nameof(PlayerCard));
                        OnPropertyChanged(nameof(DealerCard));
                        await Task.Delay(500);

                        if (playerWarCard.Value > dealerWarCard.Value)
                        {
                            foreach (var card in warPile)
                                playerDeck.Enqueue(card);
                            warResolved = true;
                        }
                        else if (dealerWarCard.Value > playerWarCard.Value)
                        {
                            foreach (var card in warPile)
                                dealerDeck.Enqueue(card);
                            warResolved = true;
                        }
                    }
                }
                PlayerDeckCount = playerDeck.Count;
                DealerDeckCount = dealerDeck.Count;
            }

            bool playerWon = playerDeck.Count > 0;
            string summary = playerWon ? "Gracz wygrywa!" : "Krupier wygrywa!";
            Result = playerWon ? "W" : "L";

            if (playerWon)
            {
                User.Balance += BetAmount * 8;
            }

            // Aktualizacja UI i bazy danych
            OnPropertyChanged(nameof(Result));
            OnPropertyChanged(nameof(ResultText));
            OnPropertyChanged(nameof(User.Balance));
            await DataHelper.UpdateAsync(User);

            var session = new GameSession(User.Id, DateTime.Now, DateTime.Now);
            await DataHelper.InsertAsync(session);

            var resultEntry = new Result(Result, playerWon ? BetAmount * 8 : 0);
            await DataHelper.InsertAsync(resultEntry);

            var betEntry = new Bet(session.Id, BetAmount, resultEntry.Id, "War");
            await DataHelper.InsertAsync(betEntry);

            // Reset UI
            BetAmount = 0;

            OnPropertyChanged(nameof(PlayerCard));
            OnPropertyChanged(nameof(DealerCard));
            OnPropertyChanged(nameof(Result));
            OnPropertyChanged(nameof(ResultText));
            OnPropertyChanged(nameof(BetAmount));
            IsAnimating = false;
        }
        public void NewGame()
        {
            PlayerCard = null;
            DealerCard = null;
            Result = string.Empty;
            OnPropertyChanged(nameof(PlayerCard));
            OnPropertyChanged(nameof(DealerCard));
            OnPropertyChanged(nameof(Result));
            OnPropertyChanged(nameof(ResultText));
            PlayRound();
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
        private void OnPropertyChanged(string name) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        internal void PlayGame()
        {
            throw new NotImplementedException();
        }
    }
    public static class QueueExtensions
    {
        public static void EnqueueRange<T>(this Queue<T> queue, IEnumerable<T> items)
        {
            foreach (var item in items)
                queue.Enqueue(item);
        }
    }
}
