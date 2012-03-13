using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace PhoneConnector.Core
{
    public class ActionCommand : ICommand
    {
        private readonly Action _action;

        public ActionCommand(Action action)
        {
            if (action == null) throw new ArgumentNullException("action");
            _action = action;
        }

        public void Execute(object parameter)
        {
            if (_action != null)
                _action();
        }

        public bool CanExecute(object parameter)
        {
            return _action != null;
        }

        public event EventHandler CanExecuteChanged;
    }
}
