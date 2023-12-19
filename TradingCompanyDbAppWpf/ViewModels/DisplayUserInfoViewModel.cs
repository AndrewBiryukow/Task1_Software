using System.ComponentModel;
using TradingCompanyDbApp.DAL.Models;
using TradingCompanyDbApp.DTO.ModelsDTO;

public class DisplayUserInfoViewModel : INotifyPropertyChanged
{
    private UserDTO user;

    public UserDTO User
    {
        get { return user; }
        set
        {
            user = value;
            OnPropertyChanged(nameof(User));
        }
    }

    public DisplayUserInfoViewModel(UserDTO user)
    {
        this.user = user;
    }

    #region INotifyPropertyChanged
    public event PropertyChangedEventHandler PropertyChanged;
    public void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
    #endregion
}
