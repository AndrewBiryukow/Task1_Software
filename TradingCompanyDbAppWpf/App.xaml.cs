
using TradingCompanyDbApp.DAL.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Windows;
using TradingCompanyDbApp.DAL.Interfaces;
using TradingCompanyDbApp.DAL;
using TradingCompanyDbApp.DAL.Models;
using TradingCompanyDbApp.DAL.Repositories;
using Unity;
using Unity.Injection;
using Unity.Lifetime;
using WPF.Windows;
using WPF.ViewModels;
using TradingCompanyDbApp.TradeApp.DAL.Repositories;
using TradingCompanyDbApp.Services;
using TradingCompanyDbApp.DAL.Services;
using TradingCompanyDbAppWpf;


namespace WPF
{
    public partial class App : Application
    {
        public IUnityContainer Container;
        /*protected override void OnStartup(StartupEventArgs e)
        {
            Current.ShutdownMode = ShutdownMode.OnExplicitShutdown;

            base.OnStartup(e);
            RegisterUnity();
            LoginView lf = Container.Resolve<LoginView>();
            bool? result = lf.ShowDialog();

            if (result.HasValue && result.Value)
            {
                DisplayUserInfoView pl = Container.Resolve<DisplayUserInfoView>();
                Current.ShutdownMode = ShutdownMode.OnMainWindowClose;
                Current.MainWindow = pl;
                pl.Show();
            }

        }*/

        protected override void OnStartup(StartupEventArgs e)
        {
            Current.ShutdownMode = ShutdownMode.OnExplicitShutdown;

            base.OnStartup(e);
            RegisterUnity();

            bool isRegistration = true;  // Set to true for registration, false for login

            switch (isRegistration)
            {
                case true:
                    RegistrationView registrationView = Container.Resolve<RegistrationView>();
                    bool? registrationResult = registrationView.ShowDialog();

                    if (registrationResult.HasValue && registrationResult.Value)
                    {
                        DisplayUserInfoView displayUserInfoView = Container.Resolve<DisplayUserInfoView>();
                        Current.ShutdownMode = ShutdownMode.OnMainWindowClose;
                        Current.MainWindow = displayUserInfoView;
                        displayUserInfoView.Show();
                    }
                    break;

                case false:
                    LoginView loginView = Container.Resolve<LoginView>();
                    bool? loginResult = loginView.ShowDialog();

                    if (loginResult.HasValue && loginResult.Value)
                    {
                        DisplayUserInfoView displayUserInfoView = Container.Resolve<DisplayUserInfoView>();
                        Current.ShutdownMode = ShutdownMode.OnMainWindowClose;
                        Current.MainWindow = displayUserInfoView;
                        displayUserInfoView.Show();
                    }
                    break;

                default:
                    LoginView defaultLoginView = Container.Resolve<LoginView>();
                    bool? defaultLoginResult = defaultLoginView.ShowDialog();

                    if (defaultLoginResult.HasValue && defaultLoginResult.Value)
                    {
                        DisplayUserInfoView defaultDisplayUserInfoView = Container.Resolve<DisplayUserInfoView>();
                        Current.ShutdownMode = ShutdownMode.OnMainWindowClose;
                        Current.MainWindow = defaultDisplayUserInfoView;
                        defaultDisplayUserInfoView.Show();
                    }
                    break;
            }

            // Add the password recovery case
            bool isPasswordRecovery = true;  

            switch (isPasswordRecovery)
            {
                case true:
                    PasswordRecoveryView passwordRecoveryView = Container.Resolve<PasswordRecoveryView>();
                    bool? passwordRecoveryResult = passwordRecoveryView.ShowDialog();

                    if (passwordRecoveryResult.HasValue && passwordRecoveryResult.Value)
                    {
                        // Handle the result as needed
                    }
                    break;
            }
        }


        /*protected override void OnStartup(StartupEventArgs e)
        {
            Current.ShutdownMode = ShutdownMode.OnExplicitShutdown;

            base.OnStartup(e);
            RegisterUnity();

            // Show the Welcome view
            WPF.Windows.WelcomeView welcomeView = Container.Resolve<WPF.Windows.WelcomeView>();
            WelcomeViewModel welcomeViewModel = Container.Resolve<WelcomeViewModel>();
            welcomeView.DataContext = welcomeViewModel;

            bool? welcomeResult = welcomeView.ShowDialog();

            // Based on user selection, show either the login or registration view
            if (welcomeResult.HasValue && welcomeResult.Value)
            {
                // Show the Login view
                WPF.Windows.LoginView loginView = Container.Resolve<WPF.Windows.LoginView>();
                LoginViewModel loginViewModel = Container.Resolve<LoginViewModel>();
                loginView.DataContext = loginViewModel;

                bool? loginResult = loginView.ShowDialog();

                if (loginResult.HasValue && loginResult.Value)
                {
                    // Show the DisplayUserInfoView after successful login
                    var displayUserInfoView = Container.Resolve<DisplayUserInfoView>();
                    Current.ShutdownMode = ShutdownMode.OnMainWindowClose;
                    Current.MainWindow = displayUserInfoView;
                    displayUserInfoView.Show();
                }
            }
            else
            {
                // Show the Registration view
                WPF.Windows.RegistrationView registrationView = Container.Resolve<WPF.Windows.RegistrationView>();
                RegistrationViewModel registrationViewModel = Container.Resolve<RegistrationViewModel>();
                registrationView.DataContext = registrationViewModel;

                bool? registrationResult = registrationView.ShowDialog();

                if (registrationResult.HasValue && registrationResult.Value)
                {
                    // Show the DisplayUserInfoView after successful registration
                    var displayUserInfoView = Container.Resolve<DisplayUserInfoView>();
                    Current.ShutdownMode = ShutdownMode.OnMainWindowClose;
                    Current.MainWindow = displayUserInfoView;
                    displayUserInfoView.Show();
                }
            }
        }*/




        /*protected override void OnStartup(StartupEventArgs e)
        {
            Current.ShutdownMode = ShutdownMode.OnExplicitShutdown;

            base.OnStartup(e);
            RegisterUnity();

            // Show the Welcome view
            WPF.Windows.WelcomeView welcomeView = Container.Resolve<WPF.Windows.WelcomeView>();
            WelcomeViewModel welcomeViewModel = Container.Resolve<WelcomeViewModel>();
            welcomeView.DataContext = welcomeViewModel;

            bool? welcomeResult = welcomeView.ShowDialog();

            // Based on user selection, show either the login or registration view
            if (welcomeResult.HasValue && welcomeResult.Value)
            {
                // Show the Login view
                WPF.Windows.LoginView loginView = Container.Resolve<WPF.Windows.LoginView>();
                LoginViewModel loginViewModel = Container.Resolve<LoginViewModel>();
                loginView.DataContext = loginViewModel;

                bool? loginResult = loginView.ShowDialog();

                if (loginResult.HasValue && loginResult.Value)
                {
                    // Show the DisplayUserInfoView after successful login
                    var displayUserInfoView = Container.Resolve<DisplayUserInfoView>();
                    Current.ShutdownMode = ShutdownMode.OnMainWindowClose;
                    Current.MainWindow = displayUserInfoView;
                    displayUserInfoView.Show();
                }
            }
            else
            {
                // Show the Registration view
                WPF.Windows.RegistrationView registrationView = Container.Resolve<WPF.Windows.RegistrationView>();
                RegistrationViewModel registrationViewModel = Container.Resolve<RegistrationViewModel>();
                registrationView.DataContext = registrationViewModel;

                bool? registrationResult = registrationView.ShowDialog();

                if (registrationResult.HasValue && registrationResult.Value)
                {
                    // Show the DisplayUserInfoView after successful registration
                    var displayUserInfoView = Container.Resolve<DisplayUserInfoView>();
                    Current.ShutdownMode = ShutdownMode.OnMainWindowClose;
                    Current.MainWindow = displayUserInfoView;
                    displayUserInfoView.Show();
                }
            }
        }*/



        private void RegisterUnity()
        {
            Container = new UnityContainer();
            var configuration = new ConfigurationBuilder()
                   .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../../../"))
                   .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                   .Build();

            var connectionString = configuration.GetConnectionString("myconn");
            // Register DbContext
            Container.RegisterType<TradeDbContext>(
               new HierarchicalLifetimeManager(),
               new InjectionConstructor(
                   new DbContextOptionsBuilder<TradeDbContext>()
                       .UseSqlServer("Server=.\\SQLEXPRESS;Database=TradingCompanyDB;Trusted_Connection=True;TrustServerCertificate=True;")
                       .Options
               )
           );


            // Register repositories
            Container.RegisterType<IUserRepository, UserRepository>(new HierarchicalLifetimeManager());
            Container.RegisterType<IProductRepository, ProductRepository>(new HierarchicalLifetimeManager());
            Container.RegisterType<IOrderRepository, OrderRepository>(new HierarchicalLifetimeManager());          

            // Register business logic services
            Container.RegisterType<IUserService, UserService>(new HierarchicalLifetimeManager());
            Container.RegisterType<UserService>(new HierarchicalLifetimeManager());
            Container.RegisterType<IProductService, ProductService>(new HierarchicalLifetimeManager());
            Container.RegisterType<IOrderService, OrderService>(new HierarchicalLifetimeManager());

            // Register view models
            Container.RegisterType<LoginViewModel>(new HierarchicalLifetimeManager());
            Container.RegisterType<DisplayUserInfoViewModel>(new HierarchicalLifetimeManager());
            // Add other registrations as needed

           
        }

        private void OnExit(object sender, ExitEventArgs e)
        {
            Container?.Dispose();
        }
    }
}