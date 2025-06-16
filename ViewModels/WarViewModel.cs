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

        private Deck deck;
        public User User => App.User;

        public Card PlayerCard { get; set; }
        public Card DealerCard { get; set; }

        private int betAmount;
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

        public bool IsAnimating { get; internal set; }

        // Komendy
        public ExitGameCommand ExitGameCommand { get; }
        public WarNewGameCommand NewGameCommand { get; }
        public WarPlayCommand PlayCommand { get; }
        public bool CanPlay { get; internal set; }

      

        public WarViewModel()
        {
            ExitGameCommand = new ExitGameCommand(this);
            NewGameCommand = new WarNewGameCommand(this);
            PlayCommand = new WarPlayCommand(this);
        }

        public async Task PlayRound()
        {
            deck = new Deck();
            deck.Shuffle();

            // Podzial talii
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

            while (playerDeck.Count > 0 && dealerDeck.Count > 0 && User.Balance >= BetAmount)
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
                    //playerDeck.EnqueueRange(winnings);
                    foreach (var card in winnings)
                    {
                        playerDeck.Enqueue(card);
                    }
                    Result = "W";
                    User.Balance += BetAmount;
                }
                else if (dealerCard.Value > playerCard.Value)
                {
                    //dealerDeck.EnqueueRange(winnings);
                    foreach (var card in winnings)
                        playerDeck.Enqueue(card);
                    Result = "L";
                    User.Balance -= BetAmount;
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

                        // 3 karty zakryte
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
                            //playerDeck.EnqueueRange(warPile);
                            foreach (var card in winnings)
                                playerDeck.Enqueue(card);
                            Result = "W";
                            User.Balance += BetAmount;
                            warResolved = true;
                        }
                        else if (dealerWarCard.Value > playerWarCard.Value)
                        {
                            //dealerDeck.EnqueueRange(warPile);
                            foreach (var card in winnings)
                                dealerDeck.Enqueue(card);
                            Result = "L";
                            User.Balance -= BetAmount;
                            warResolved = true;
                        }
                    }
                }

                OnPropertyChanged(nameof(Result));
                OnPropertyChanged(nameof(ResultText));
                await DataHelper.UpdateAsync(User);

                var session = new GameSession(User.Id, DateTime.Now, DateTime.Now);
                await DataHelper.InsertAsync(session);

                var resultEntry = new Result(Result, Result == "W" ? BetAmount : 0);
                await DataHelper.InsertAsync(resultEntry);

                var betEntry = new Bet(session.Id, BetAmount, resultEntry.Id, "War");
                await DataHelper.InsertAsync(betEntry);
            }

            string summary = playerDeck.Count == 0 ? "Krupier wygrywa!" : "Gracz wygrywa!";
            MessageBox.Show($"Gra zakończona! {summary}\nRund rozegranych: {roundCounter}", "Koniec gry", MessageBoxButton.OK, MessageBoxImage.Information);

            PlayerCard = null;
            DealerCard = null;
            Result = string.Empty;
            BetAmount = 0;

            OnPropertyChanged(nameof(PlayerCard));
            OnPropertyChanged(nameof(DealerCard));
            OnPropertyChanged(nameof(Result));
            OnPropertyChanged(nameof(ResultText));
            OnPropertyChanged(nameof(BetAmount));
        }

        // public static class QueueExtensions
        // {
        //public static void EnqueueRange<T>(this Queue<T> queue, IEnumerable<T> items)
        //{
        //    foreach (var item in items)
        //        queue.Enqueue(item);
        //}
        // }

        


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

        public void ExitGame(Window window)
        {
            var mainMenu = new MainMenuView();
            mainMenu.Show();
            window?.Close();
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
