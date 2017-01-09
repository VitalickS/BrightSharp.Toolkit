using System;
using System.Windows.Input;

namespace BrightSharp.Commands
{
    public class RelayCommand : ICommand
    {
        private readonly Action _methodToExecute;
        private readonly Action<object> _methodToExecuteWithParam;
        private readonly Func<object, bool> _canExecuteEvaluator;

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public RelayCommand(Action<object> methodToExecute, Func<object, bool> canExecuteEvaluator = null)
        {
            _methodToExecuteWithParam = methodToExecute;
            _canExecuteEvaluator = canExecuteEvaluator;
        }

        public bool CanExecute(object parameter)
        {
            if (_canExecuteEvaluator == null)
                return true;
            else
                return _canExecuteEvaluator.Invoke(parameter);
        }
        public void Execute(object parameter)
        {
            _methodToExecuteWithParam?.Invoke(parameter);
            _methodToExecute?.Invoke();
        }
    }

    public class RelayCommand<T> : ICommand
    {
        private readonly Action<T> _methodToExecuteWithParam;
        private readonly Func<T, bool> _canExecuteEvaluator;

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public RelayCommand(Action<T> methodToExecute, Func<T, bool> canExecuteEvaluator = null)
        {
            _methodToExecuteWithParam = methodToExecute;
            _canExecuteEvaluator = canExecuteEvaluator;
        }

        public bool CanExecute(object parameter)
        {
            if (_canExecuteEvaluator == null)
                return true;
            return _canExecuteEvaluator.Invoke((T)parameter);
        }
        public void Execute(object parameter)
        {
            _methodToExecuteWithParam?.Invoke((T)parameter);
        }
    }
}
