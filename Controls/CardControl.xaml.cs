using Kasyno.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Kasyno.Controls
{
    /// <summary>
    /// Logika interakcji dla klasy CardControl.xaml
    /// </summary>
    public partial class CardControl : UserControl
    {
        public CardControl()
        {
            InitializeComponent();
            Loaded += CardControl_Loaded;
        }
        private void CardControl_Loaded(object sender, RoutedEventArgs e)
        {
            // Znajdź okno główne (Blackjack)
            var window = Window.GetWindow(this) as Kasyno.Views.Games.Blackjack;
            if (window == null) return;

            // Pozycja stosu kart na ekranie
            var deckPile = window.DeckPile;
            if (deckPile == null) return;

            // Pozycja karty na ekranie
            var cardPosition = this.TransformToAncestor(window).Transform(new Point(0, 0));
            var deckPosition = deckPile.TransformToAncestor(window).Transform(new Point(0, 0));

            // Oblicz różnicę przesunięcia
            double deltaX = deckPosition.X - cardPosition.X;
            double deltaY = deckPosition.Y - cardPosition.Y;

            // Ustaw początkowe przesunięcie na miejsce stosu kart
            translateTransform.X = deltaX;
            translateTransform.Y = deltaY;

            // Animuj przesunięcie do 0,0 (czyli docelowej pozycji)
            var animX = new DoubleAnimation(0, new Duration(TimeSpan.FromSeconds(0.5)))
            {
                AccelerationRatio = 0.3,
                DecelerationRatio = 0.3
            };
            var animY = new DoubleAnimation(0, new Duration(TimeSpan.FromSeconds(0.5)))
            {
                AccelerationRatio = 0.3,
                DecelerationRatio = 0.3
            };

            translateTransform.BeginAnimation(TranslateTransform.XProperty, animX);
            translateTransform.BeginAnimation(TranslateTransform.YProperty, animY);
        }
        public Card Card
        {
            get => (Card)GetValue(CardProperty);
            set => SetValue(CardProperty, value);
        }

        public static readonly DependencyProperty CardProperty =
            DependencyProperty.Register("Card", typeof(Card), typeof(CardControl));

        public int CardIndex
        {
            get => (int)GetValue(CardIndexProperty);
            set => SetValue(CardIndexProperty, value);
        }

        public static readonly DependencyProperty CardIndexProperty =
            DependencyProperty.Register("CardIndex", typeof(int), typeof(CardControl));

        public bool IsDealerTurnOver
        {
            get => (bool)GetValue(IsDealerTurnOverProperty);
            set => SetValue(IsDealerTurnOverProperty, value);
        }

        public static readonly DependencyProperty IsDealerTurnOverProperty =
            DependencyProperty.Register("IsDealerTurnOver", typeof(bool), typeof(CardControl));
    }
}
