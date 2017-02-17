using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BrightSharp.Commands
{
    public class AsyncCommand : ICommand
    {
        private readonly Func<object, Task> _execute;
        private readonly Func<object, bool> _canExecute;
        private Action _onComplete;
        private Action<Exception> _onFail;
        private bool _isExecuting;

        public AsyncCommand(Func<Task> execute) : this(p => execute?.Invoke())
        {
        }

        public AsyncCommand(Func<object, Task> execute) : this(execute, o => true)
        {
        }

        public AsyncCommand(Func<object, Task> execute, Func<object, bool> canExecute)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        public AsyncCommand OnComplete(Action onComplete)
        {
            _onComplete = onComplete;
            return this;
        }
        public AsyncCommand OnFail(Action<Exception> onFail)
        {
            _onFail = onFail;
            return this;
        }

        public bool CanExecute(object parameter)
        {
            return !_isExecuting && _canExecute(parameter);
        }

        public event EventHandler CanExecuteChanged;

        public async void Execute(object parameter)
        {
            _isExecuting = true;
            OnCanExecuteChanged();
            try
            {
                await _execute(parameter);
                _onComplete?.Invoke();
            }
            catch (Exception ex)
            {
                while (ex.InnerException != null)
                    ex = ex.InnerException;
                _onFail?.Invoke(ex);
            }
            finally
            {
                _isExecuting = false;
                OnCanExecuteChanged();
            }
        }

        protected virtual void OnCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, new EventArgs());
        }
    }
}
