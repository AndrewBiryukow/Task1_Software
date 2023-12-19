using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;
using WPF.Commands;
using WPF.Interfaces;
using WPF.Utilities;
using TradingCompanyDbApp.DAL.Models;
using TradingCompanyDbApp.DAL.Services;
using TradingCompanyDbApp.DAL.Interfaces;
using TradingCompanyDbApp;

namespace WPF.ViewModels
{
    public class LoginViewModel : INotifyPropertyChanged, ICloseable
    {
        private readonly IUserService userService; 
        private string userName;

        public Action LoginFailed { get; set; }
        public Action LoginSuccessful { get; set; }

        public LoginViewModel(IUserService userService)
        {
            this.userService = userService ?? throw new ArgumentNullException(nameof(userService));
            LoginCommand = new LoginCommand(this);
            CancelCommand = new CancelCommand(this);
        }

        public async Task<bool> LoginAsync()
        {
            try
            {
                var user = await userService.GetUserByNicknameAsync(Username);

                if (user != null && Hasher.VerifyPassword(Password, user.Password))
                {
                    UserContext.UserId = user.Id;
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions, log, or notify the user accordingly
                Console.WriteLine($"Login failed: {ex.Message}");
                return false;
            }
        }

        public string Username
        {
            get
            {
                return userName;
            }
            set
            {
                userName = value;
                OnPropertyChanged(nameof(Username));
            }
        }

        public string Password { private get; set; }

        public ICommand LoginCommand { get; private set; }

        public ICommand CancelCommand { get; private set; }

        private void Cancel()
        {
            // Handle cancellation if needed
            Close?.Invoke();
        }

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        #region ICloseable
        public Action Close { get; set; }
        #endregion
    }
}
