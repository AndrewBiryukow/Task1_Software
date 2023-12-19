using System.Windows;
using System.Windows.Controls;
using TradingCompanyDbApp.DAL.Interfaces;
using TradingCompanyDbAppWpf;
using WPF.Interfaces;
using WPF.Utilities;
using WPF.ViewModels;

namespace WPF.Windows
{
    /// <summary>
    /// Interaction logic for LoginView.xaml
    /// </summary>
    public partial class LoginView : Window
    {
        private readonly IUserService userService; 

        public LoginView(LoginViewModel viewModel, IUserService userService) 
        {
            DataContext = viewModel;
            this.userService = userService; 
            InitializeComponent();
            Loaded += Login_Loaded;
        }

        private async void Login_Loaded(object sender, RoutedEventArgs e)
        {
            if (DataContext is ICloseable cviewModel)
            {
                cviewModel.Close += () =>
                {
                    DialogResult = false;
                    Close();
                };
            }

            if (DataContext is LoginViewModel lvm)
            {
                lvm.LoginSuccessful += async () =>
                {
                    // Load user information
                    var user = await userService.GetUserByNicknameAsync(lvm.Username);

                    // Create a new DisplayUserInfoViewModel with the user information
                    var displayUserInfoViewModel = new DisplayUserInfoViewModel(user);

                    // Create and show the DisplayUserInfoView
                    var displayUserInfoView = new DisplayUserInfoView(displayUserInfoViewModel);
                    displayUserInfoView.ShowDialog();

                    DialogResult = true;
                    Close();
                };

                lvm.LoginFailed += () =>
                {
                    MessageBox.Show("Invalid credentials", "Error");
                };
            }
        }
        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext != null && DataContext is LoginViewModel lvm)
            {
                lvm.Password = ((PasswordBox)sender).Password;
            }
        }
    }
}
