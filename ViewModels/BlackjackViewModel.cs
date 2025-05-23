using Kasyno.Models;
using Kasyno.ViewModels.Commands;
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
        private string result = string.Empty;
        public string Result
        {
            get => result;
            set
            {
                result = value;
                OnPropertyChanged(nameof(Result));
                NewGameCommand.RaiseCanExecuteChanged();
                HitCommand.RaiseCanExecuteChanged();
                StandCommand.RaiseCanExecuteChanged();
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


        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        }
        public int PlayerValue => CalculateValue(PlayerCards);
        public int DealerValue => CalculateValue(DealerCards);
        public PlaceBetCommand PlaceBetCommand { get; }
        public HitCommand HitCommand { get; }
        public StandCommand StandCommand { get; }
        public NewGameCommand NewGameCommand { get; }
        public ExitGameCommand ExitGameCommand { get; }
        public BlackjackViewModel() 
        {
            HitCommand = new HitCommand(this);
            StandCommand = new StandCommand(this);
            NewGameCommand = new NewGameCommand(this);
            PlaceBetCommand = new PlaceBetCommand(this);
            ExitGameCommand = new ExitGameCommand(this);
            doubleDownVisible = Visibility.Collapsed;
        }
        public void NewGame()
        {
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
            if (PlayerValue == 21)
            {
                Stand();
            }
        }
        public void Hit()
        {
            PlayerCards.Add(deck.DrawCard());
            OnPropertyChanged(nameof(PlayerValue));

            if (PlayerValue > 21)
            {
                Result = "Przegrana";
            }
            if (PlayerValue == 21)
            {
                Stand();
            }
        }
        public void Stand()
        {
            while (DealerValue < 17 && DealerValue != PlayerValue)
            {
                DealerCards.Add(deck.DrawCard());
                OnPropertyChanged(nameof(DealerValue));
            }
            Result = PlayerValue > 21 ? "Przegrana"
                          : DealerValue > 21 ? "Wygrana"
                          : PlayerValue > DealerValue ? "Wygrana"
                          : "Przegrana";

        }
        public void PlaceBet(int bet)
        {
            if (BetAmount < 10)
            {
                MessageBox.Show("Minimalna stawka to 10.");
                return;
            }
            if (BetAmount > 100)// zmienic potem na saldo uzytkownika
            {
                MessageBox.Show("Nie masz wystarczającej ilości pieniędzy.");
                return;
            }
            BetAmount = bet;
            NewGame();
        }
        public void DoubleDown()
        {
            BetAmount *= 2;
            Hit();
            Stand();
        }
        public void DoubleDownVisibility()
        {
            if (PlayerCards.Count == 2 && PlayerValue < 21 && Result == string.Empty)
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
            int aceCount = cards.Count(c => c.Rank == "Ace");

            while (sum > 21 && aceCount-- > 0)
                sum -= 10;

            return sum;
        }


    }
}
