using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;
using TradingCompanyDbApp;
using TradingCompanyDbApp.DAL.Interfaces;
using TradingCompanyDbApp.DAL.Models;
using TradingCompanyDbApp.DAL.Services;
using TradingCompanyDbApp.DTO.ModelsDTO;
using WPF.Commands;
using WPF.Interfaces;

namespace WPF.ViewModels
{
    public class RegistrationViewModel : INotifyPropertyChanged, ICloseable
    {
        private readonly IUserService userService;
        private string username;
        private string password;
        private string firstName;
        private string lastName;
        private string email;
        private string phone;
        private string address;
        private string gender;
        private string bankCardNumber;
        private string recoveryKeyword;

        public Action RegistrationFailed { get; set; }
        public Action RegistrationSuccessful { get; set; }

        public RegistrationViewModel(IUserService userService)
        {
            this.userService = userService ?? throw new ArgumentNullException(nameof(userService));
            RegisterCommand = new RegisterCommand(this);
            CancelCommand = new CancelCommand(this);
        }

        public async Task RegisterAsync()
        {
            try
            {
                var existingUser = await userService.GetUserByNicknameAsync(Username);
                if (existingUser != null)
                {
                    RegistrationFailed?.Invoke();
                    return;
                }

                // Example: Creating a new user
                var newUser = new UserDTO
                {
                    Nickname = Username,
                    Password = Hasher.HashPassword(Password),
                    FirstName = FirstName,
                    LastName = LastName,
                    Email = Email,
                    Phone = Phone,
                    Address = Address,
                    Gender = Gender,
                    BankCardNumber = BankCardNumber,
                    RecoveryKeyword = RecoveryKeyword,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                    // Add other properties as needed
                };

                await userService.CreateUserAsync(newUser);

                RegistrationSuccessful?.Invoke();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Registration failed: {ex.Message}");
                RegistrationFailed?.Invoke();
            }
        }

        public string Username
        {
            get { return username; }
            set
            {
                username = value;
                OnPropertyChanged(nameof(Username));
            }
        }

        public string Password
        {
            get { return password; }
            set
            {
                password = value;
                OnPropertyChanged(nameof(Password));
            }
        }

        public string FirstName
        {
            get { return firstName; }
            set
            {
                firstName = value;
                OnPropertyChanged(nameof(FirstName));
            }
        }

        public string LastName
        {
            get { return lastName; }
            set
            {
                lastName = value;
                OnPropertyChanged(nameof(LastName));
            }
        }

        public string Email
        {
            get { return email; }
            set
            {
                email = value;
                OnPropertyChanged(nameof(Email));
            }
        }

        public string Phone
        {
            get { return phone; }
            set
            {
                phone = value;
                OnPropertyChanged(nameof(Phone));
            }
        }

        public string Address
        {
            get { return address; }
            set
            {
                address = value;
                OnPropertyChanged(nameof(Address));
            }
        }

        public string Gender
        {
            get { return gender; }
            set
            {
                gender = value;
                OnPropertyChanged(nameof(Gender));
            }
        }

        public string BankCardNumber
        {
            get { return bankCardNumber; }
            set
            {
                bankCardNumber = value;
                OnPropertyChanged(nameof(BankCardNumber));
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

        public ICommand RegisterCommand { get; private set; }

        public ICommand CancelCommand { get; private set; }

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
