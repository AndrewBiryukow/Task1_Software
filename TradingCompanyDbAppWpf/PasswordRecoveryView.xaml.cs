using System.Windows;
using WPF.Interfaces;
using WPF.ViewModels;

namespace WPF.Windows
{
    public partial class PasswordRecoveryView : Window
    {
        private readonly PasswordRecoveryViewModel viewModel;

        public PasswordRecoveryView(PasswordRecoveryViewModel viewModel)
        {
            DataContext = viewModel;
            this.viewModel = viewModel;
            InitializeComponent();
            Loaded += PasswordRecovery_Loaded;
        }

        private void PasswordRecovery_Loaded(object sender, RoutedEventArgs e)
        {
            if (DataContext is ICloseable cviewModel)
            {
                cviewModel.Close += () =>
                {
                    DialogResult = false;
                    Close();
                };
            }
        }
    }
}
