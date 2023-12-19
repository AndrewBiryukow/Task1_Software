using System;
using System.Windows;
using System.Windows.Controls;
using TradingCompanyDbApp.DAL.Interfaces;
using WPF.Interfaces;
using WPF.Utilities;
using WPF.ViewModels;

namespace WPF.Windows
{
    public partial class RegistrationView : Window
    {
        private readonly IUserService userService;

        public RegistrationView(RegistrationViewModel viewModel, IUserService userService)
        {
            DataContext = viewModel;
            this.userService = userService;
            InitializeComponent();
            Loaded += Registration_Loaded;

            Height = 514;
            Width = 1012;
        }

        private async void Registration_Loaded(object sender, RoutedEventArgs e)
        {
            if (DataContext is ICloseable cviewModel)
            {
                cviewModel.Close += () =>
                {
                    DialogResult = false;
                    Close();
                };
            }

            if (DataContext is RegistrationViewModel rvm)
            {
                rvm.RegistrationSuccessful += async () =>
                {
                   
                    var registeredUser = await userService.GetUserByNicknameAsync(rvm.Username);

                    var displayUserInfoViewModel = new DisplayUserInfoViewModel(registeredUser);
                    var displayUserInfoView = new DisplayUserInfoView(displayUserInfoViewModel);
                    displayUserInfoView.ShowDialog();

                    DialogResult = true;
                    Close();
                };

                rvm.RegistrationFailed += () =>
                {
                    MessageBox.Show("Registration failed. Please check your inputs and try again.", "Error");
                };
            }
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext != null && DataContext is RegistrationViewModel rvm)
            {
                rvm.Password = ((PasswordBox)sender).Password;
            }
        }

    }
}
