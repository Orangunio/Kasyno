using Kasyno.ViewModels;
using System;
using System.ComponentModel;
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
            DataContext = new SlotMachineViewModel(this);

            _spinTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(100)
            };
            _spinTimer.Tick += SpinTimer_Tick;

            this.Closing += SlotMachineView_Closing;
        }

        private void SlotMachineView_Closing(object? sender, CancelEventArgs e)
        {
            if (ViewModel != null)
            {
                e.Cancel = true; 
                ViewModel.OnWindowClosing(); 
            }
        }

        public void StartSpin()
        {
            _spinTicks = 0;
            _spinTimer.Start();
        }

        private async void SpinTimer_Tick(object sender, EventArgs e)
        {
            _spinTicks++;

            ViewModel.Icon1 = ViewModel.GetRandomIcon();
            ViewModel.Icon2 = ViewModel.GetRandomIcon();
            ViewModel.Icon3 = ViewModel.GetRandomIcon();

            if (_spinTicks >= MaxSpinTicks)
            {
                _spinTimer.Stop();

                ViewModel.Icon1 = ViewModel.GetRandomIcon();
                await Task.Delay(150);

                ViewModel.Icon2 = ViewModel.GetRandomIcon();
                await Task.Delay(150);

                ViewModel.Icon3 = ViewModel.GetRandomIcon();

                if (SlotMachineControlElement.Resources["IconFlashStoryboard"] is Storyboard glowStoryboard)
                {
                    glowStoryboard.Begin();
                }

                await Task.Delay(700);

                ViewModel.ResolveGame();
            }
        }
    }
}
