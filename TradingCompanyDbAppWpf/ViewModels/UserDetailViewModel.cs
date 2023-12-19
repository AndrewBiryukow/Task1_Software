/*using BusinessLogic.BL;
using BusinessLogic.IBL;
using DTO_Core.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using WPF.Utilities;
using WPF.Windows;

namespace WPF.ViewModels
{
    public class UserDetailViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<Cart> carts; // Add this line
        private ICartBL cartBL;
        private string _userName;
        private decimal _cartTotal;

        public string UserName
        {
            get { return _userName; }
            set
            {
                if (_userName != value)
                {
                    _userName = value;
                    OnPropertyChanged(nameof(UserName));
                }
            }
        }

        public ObservableCollection<Cart> CartList
        {
            get { return carts; }
            set
            {
                carts = value;
                OnPropertyChanged(nameof(CartList));
            }
        }

        public decimal CartTotal
        {
            get { return _cartTotal; }
            set
            {
                if (_cartTotal != value)
                {
                    _cartTotal = value;
                    OnPropertyChanged(nameof(CartTotal));
                }
            }
        }

        public ICommand GoBackCommand { get; }

        public UserDetailViewModel(ICartBL cartBL, IUserBL userBL)
        {
            this.cartBL = cartBL;
            _userName = userBL.GetUser(UserContext.UserId).UserName;
            _cartTotal = this.cartBL.GetCartTotalSumByUserId(UserContext.UserId);
            Update();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void Update()
        {
            var carts = cartBL.GetUserCarts(UserContext.UserId);
            CartList = new ObservableCollection<Cart>(carts);
        }
    }
}
*/