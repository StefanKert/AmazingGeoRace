using System;
using System.Windows.Input;

namespace AmazingGeoRace.Commands
{
    public class RelayCommand: ICommand
    {
        private readonly Action<object> _action;
        private readonly Func<bool> _canExecute;


        public event EventHandler CanExecuteChanged;

        public RelayCommand(Action<object> action): this(action, () => true) {}

        public RelayCommand(Action<object> action, Func<bool> canExecute) {
            _action = action;
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter) {
            return _canExecute();
        }

        public void Execute(object parameter) {
            _action(parameter);
        }

        public void RaiseCanExecuteChanged() {
            var handler = CanExecuteChanged;
            handler?.Invoke(this, EventArgs.Empty);
        }
    }
}