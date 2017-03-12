using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
namespace StudentsList
{
    class RelayCommand : ICommand
    {
        private Action<Object> act;
        private Predicate<Object> pred;

        public event EventHandler CanExecuteChanged;

        public RelayCommand(Action<Object> _act, Predicate<Object> _func)
        {
            act = _act;
            pred = _func;
        }

        public void RaiseCanExecuteChanged()
        {
            if (CanExecuteChanged != null)
                CanExecuteChanged(this, new EventArgs());
        }

        bool ICommand.CanExecute(object parameter)
        {
            return pred(parameter);
        }

        void ICommand.Execute(object parameter)
        {
            act(parameter);
        }
    }
}
