using Kasyno.Models;
using Kasyno.ViewModels.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
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


        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        }
        public int PlayerValue => CalculateValue(PlayerCards);
        public int DealerValue => CalculateValue(DealerCards);

        public HitCommand HitCommand { get; }
        public StandCommand StandCommand { get; }
        public NewGameCommand NewGameCommand { get; }
        public BlackjackViewModel() 
        {
            HitCommand = new HitCommand(this);
            StandCommand = new StandCommand(this);
            NewGameCommand = new NewGameCommand(this);
            NewGame();
        }
        public void NewGame()
        {
            deck = new Deck();
            PlayerCards.Clear();
            DealerCards.Clear();
            Result = string.Empty;
            PlayerCards.Add(deck.DrawCard());
            PlayerCards.Add(deck.DrawCard());
            DealerCards.Add(deck.DrawCard());
            DealerCards.Add(deck.DrawCard());

            OnPropertyChanged(nameof(PlayerValue));
            OnPropertyChanged(nameof(DealerValue));
        }
        public void Hit()
        {
            PlayerCards.Add(deck.DrawCard());
            OnPropertyChanged(nameof(PlayerValue));

            if (PlayerValue > 21)
            {
                Result = "Przegrana";
                MessageBox.Show(Result); // potem zrobic aby ladnie sie wyswietlalo
            }
        }
        public void Stand()
        {
            while (DealerValue < 17)
            {
                DealerCards.Add(deck.DrawCard());
                OnPropertyChanged(nameof(DealerValue));
            }
            Result = PlayerValue > 21 ? "Przegrana"
                          : DealerValue > 21 ? "Wygrana"
                          : PlayerValue > DealerValue ? "Wygrana"
                          : "Przegrana";

            MessageBox.Show(Result);// tak samo jak wyzej
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
