using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BIRC
{
    public sealed class RelayCommand : ICommand
    {
        Action<object> _execute;
        Action _executeNoObj;
        Func<bool> _canExecute;

        public RelayCommand(Action<object> execute, Func<bool> canExecute)
        {
            if (execute != null)
            {
                _executeNoObj = null;
                _execute = execute;
                _canExecute = canExecute;
            }
        }

        public RelayCommand(Action execute, Func<bool> canExecute)
        {
            if (execute != null)
            {
                _execute = null;
                _executeNoObj = execute;
                _canExecute = canExecute;
            }
        }

        bool ICommand.CanExecute(object parameter)
        {
            if (_canExecute == null)
                return true;
            else
                return _canExecute();
        }

        public event EventHandler CanExecuteChanged;

        public void RaiseCanExecuteChanged()
        {
            if (CanExecuteChanged != null)
            {
                CanExecuteChanged(this, EventArgs.Empty);
            }
        }

        void ICommand.Execute(object parameter)
        {
            if (_executeNoObj != null)
                _executeNoObj();
            else
                _execute(parameter);
        }
    }
}
