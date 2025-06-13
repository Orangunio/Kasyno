using System;
using System.Windows.Input;

namespace Kasyno.Helpers
{
    public class RelayCommand : ICommand
    {
        private readonly Action<object> _executeWithParam;
        private readonly Action _execute;
        private readonly Func<object, bool> _canExecuteWithParam;
        private readonly Func<bool> _canExecute;

        // Konstruktor dla akcji bez parametru
        public RelayCommand(Action execute, Func<bool> canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        // Konstruktor dla akcji z parametrem
        public RelayCommand(Action<object> execute, Func<object, bool> canExecute = null)
        {
            _executeWithParam = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecuteWithParam = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            if (_canExecuteWithParam != null)
                return _canExecuteWithParam(parameter);

            if (_canExecute != null)
                return _canExecute();

            return true;
        }

        public void Execute(object parameter)
        {
            if (_executeWithParam != null)
                _executeWithParam(parameter);
            else
                _execute();
        }

        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }
    }
}
