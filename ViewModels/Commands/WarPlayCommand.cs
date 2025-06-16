using System;
using System.Windows;
using System.Windows.Input;

namespace Kasyno.ViewModels.Commands
{
    public class WarPlayCommand : ICommand
    {
        private WarViewModel viewModel;

        public WarPlayCommand(WarViewModel viewModel)
        {
            this.viewModel = viewModel;
        }

        public event EventHandler? CanExecuteChanged;

        public bool CanExecute(object? parameter)
        {
            return !viewModel.IsAnimating && viewModel.BetAmount >= 10;
        }

        public void Execute(object? parameter)
        {
            //await viewModel.PlayRound();
            MessageBox.Show("Klik działa");
            viewModel.PlayGame();
        }

        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
