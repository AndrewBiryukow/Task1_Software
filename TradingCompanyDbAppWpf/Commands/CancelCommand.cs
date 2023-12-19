using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WPF.Interfaces;

namespace WPF.Commands
{
    public class CancelCommand : ICommand
    {
        private ICloseable vm;
        public CancelCommand(ICloseable vm)
        {
            this.vm = vm;
        }

        #region ICommand
        public event EventHandler CanExecuteChanged
        {
            add
            {
                CommandManager.RequerySuggested += value;
            }
            remove
            {
                CommandManager.RequerySuggested -= value;
            }
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            vm.Close?.Invoke();
        }
        #endregion
    }
}
