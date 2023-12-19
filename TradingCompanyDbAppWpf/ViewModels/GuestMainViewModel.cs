/*using System.Windows.Input;
using TradingCompanyDbApp.DAL.Models;
using TradingCompanyDbApp.DAL.Services;
using TradingCompanyDbAppWpf.ViewModels;
using TradingCompanyDbApp.DAL.Repositories;

namespace TradingCompanyDbApp.ViewModels
{
    public class GuestMainViewModel : BaseViewModel
    {
        private readonly Guest _guest;

        public GuestMainViewModel()
        {
            userRepository = new UserRepository(dbContext);
            userService = new UserService(userRepository);
            var userService = new UserService(*//*inject dependencies if needed*//*);
            _guest = new Guest(userService);
        }

        private ICommand _registerCommand;
        public ICommand RegisterCommand => _registerCommand ?? (_registerCommand = new RelayCommand(Register));

        private ICommand _loginCommand;
        public ICommand LoginCommand => _loginCommand ?? (_loginCommand = new RelayCommand(Login));

        private ICommand _forgotPasswordCommand;
        public ICommand ForgotPasswordCommand => _forgotPasswordCommand ?? (_forgotPasswordCommand = new RelayCommand(ForgotPassword));

        private string _nickname;
        public string Nickname
        {
            get => _nickname;
            set
            {
                _nickname = value;
                OnPropertyChanged(nameof(Nickname));
            }
        }

        private async void Register()
        {
            await _guest.Register();
        }

        private async void Login()
        {
            if (await _guest.Login(Nickname))
            {
                // Navigate to the next view or perform actions after successful login
            }
        }

        private async void ForgotPassword()
        {
            await _guest.ForgotPassword();
        }
    }
}
*/