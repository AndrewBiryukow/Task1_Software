using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WPF.ViewModels;

namespace WPF.Commands
{
    internal class LoginCommand : ICommand
    {
        private LoginViewModel viewModel;
        public LoginCommand(LoginViewModel viewModel)
        {
            this.viewModel = viewModel;
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

        public bool CanExecute(object? parameter)
        {
            return true;
        }

        public async void Execute(object? parameter)
        {
            bool loginResult = await viewModel.LoginAsync();

            if (loginResult)
            {
                viewModel.LoginSuccessful?.Invoke();
            }
            else
            {
                viewModel.LoginFailed?.Invoke();
            }
        }
        #endregion
    }
}
