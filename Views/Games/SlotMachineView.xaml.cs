using Kasyno.ViewModels;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Animation;
using System.Windows.Threading;

namespace Kasyno.Views.Games
{
    public partial class SlotMachineView : Window
    {
        private readonly DispatcherTimer _spinTimer;
        private int _spinTicks;
        private const int MaxSpinTicks = 20;
        private SlotMachineViewModel ViewModel => DataContext as SlotMachineViewModel;

        public SlotMachineView()
        {
            InitializeComponent();
            DataContext = new SlotMachineViewModel(this); // Przekazujemy referencję do widoku

            _spinTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(100)
            };
            _spinTimer.Tick += SpinTimer_Tick;
        }

        public void StartSpin()
        {
            _spinTicks = 0;
            _spinTimer.Start();
        }

        private async void SpinTimer_Tick(object sender, EventArgs e)
        {
            _spinTicks++;

            // Symuluje obracające się bębny
            ViewModel.Icon1 = ViewModel.GetRandomIcon();
            ViewModel.Icon2 = ViewModel.GetRandomIcon();
            ViewModel.Icon3 = ViewModel.GetRandomIcon();

            if (_spinTicks >= MaxSpinTicks)
            {
                _spinTimer.Stop();

                // Ustaw finalne losowanie
                ViewModel.Icon1 = ViewModel.GetRandomIcon();
                ViewModel.Icon2 = ViewModel.GetRandomIcon();
                ViewModel.Icon3 = ViewModel.GetRandomIcon();

                // Odpal animację świecenia ikon
                var glowStoryboard = (Storyboard)FindResource("IconFlashStoryboard");
                glowStoryboard.Begin();

                // Odczekaj chwilę zanim pokażesz wynik
                await Task.Delay(700);

                // Sprawdź wynik
                ViewModel.ResolveGame();
            }
        }
    }
}
