using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;
using TradingCompanyDbApp;
using TradingCompanyDbApp.DAL.Services;
using WPF.Commands;
using WPF.Interfaces;
using WPF.Utilities;

namespace WPF.ViewModels
{
    public class PasswordRecoveryViewModel : ICloseable
    {
        private readonly UserService userService;
        private string nickname;
        private string recoveryKeyword;
        private string newPassword;

        public Action PasswordRecoverySuccessful { get; set; }
        public Action PasswordRecoveryFailed { get; set; }

        public PasswordRecoveryViewModel(UserService userService)
        {
            this.userService = userService ?? throw new ArgumentNullException(nameof(userService));
            RecoverPasswordCommand = new RelayCommand(async () => await RecoverPasswordAsync());
            CancelCommand = new RelayCommand(Cancel);
        }

        public string Nickname
        {
            get { return nickname; }
            set
            {
                nickname = value;
                OnPropertyChanged(nameof(Nickname));
            }
        }

        public string RecoveryKeyword
        {
            get { return recoveryKeyword; }
            set
            {
                recoveryKeyword = value;
                OnPropertyChanged(nameof(RecoveryKeyword));
            }
        }

        public string NewPassword
        {
            get { return newPassword; }
            set
            {
                newPassword = value;
                OnPropertyChanged(nameof(NewPassword));
            }
        }

        public ICommand RecoverPasswordCommand { get; private set; }

        public ICommand CancelCommand { get; private set; }

        private async Task RecoverPasswordAsync()
        {
            try
            {
                var user = await userService.GetUserByNicknameAsync(Nickname);

                if (user != null)
                {
                    if (RecoveryKeyword == user.RecoveryKeyword)
                    {
                        user.Password = Hasher.HashPassword(NewPassword);
                        user.UpdatedAt = DateTime.Now;

                        await userService.UpdateUserAsync(user);

                        PasswordRecoverySuccessful?.Invoke();
                    }
                    else
                    {
                        PasswordRecoveryFailed?.Invoke();
                    }
                }
                else
                {
                    PasswordRecoveryFailed?.Invoke();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Password recovery failed: {ex.Message}");
                PasswordRecoveryFailed?.Invoke();
            }
        }

        private void Cancel()
        {
            // Handle cancellation if needed
            Close?.Invoke();
        }


        #region ICloseable
        public Action Close { get; set; }
        #endregion

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
