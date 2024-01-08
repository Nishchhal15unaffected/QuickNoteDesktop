using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using QuickNote.Model;
using QuickNote.ViewModel.Commands;
using QuickNote.ViewModel.Helper;
using static SQLite.SQLite3;

namespace QuickNote.ViewModel
{
    public class LoginVM : INotifyPropertyChanged
    {
        private User user;
        private bool isShowingRegister = false;
        public User User
        {
            get { return user; }
            set {
                user = value;
                OnPropertyChanged("User");
            }
        }
        private string userName;

        public string UserName
        {
            get { return userName; }
            set { 
                userName = value;
                User = new User
                {
                    UserName = userName,
                    Password = this.Password
                };
                OnPropertyChanged("UserName");
            }
        }

        private string password;

        public string Password
        {
            get { return password; }
            set { 
                password = value;
                User = new User
                {
                    UserName = this.userName,
                    Password = password
                };
                OnPropertyChanged("Password");
            }
        }
        private string name;
        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                User = new User
                {
                    
                    UserName = this.userName,
                    Name = name,
                    Password = this.Password
                };
                OnPropertyChanged("Name");
            }
        }
        private string lastName;
        public string LastName
        {
            get { return lastName; }
            set
            {
                lastName = value;
                User = new User
                {
                    Name = this.Name,
                    LastName = lastName,
                    UserName = this.userName,
                    Password = this.Password
                };
                OnPropertyChanged("LastName");
            }
        }
        private string confirmPassword;
        public string ConfirmPassword
        {
            get { return confirmPassword; }
            set
            {
                confirmPassword = value;
                User = new User
                {
                    Name = this.Name,
                    LastName= this.LastName,
                    UserName = this.userName,
                    Password = this.Password,
                    ConfirmPassword = confirmPassword
                };
                OnPropertyChanged("ConfirmPassword");
            }
        }

        private Visibility logInVis;

        public Visibility LogInVis
        {
            get { return logInVis; }
            set {
                logInVis = value;
                OnPropertyChanged("LogInVis");
                }
        }

        private Visibility regInVis;
        public Visibility RegInVis
        {
            get { return regInVis; }
            set
            {
                regInVis = value;
                OnPropertyChanged("regInVis");
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        public event EventHandler Authenticate;
        public RegisterCommand RegisterCommand { get; set; }
        public LoginCommand LoginCommand { get; set; }
        public ShowRegisterCommand ShowRegisterCommand { get; set; }
        public LoginVM()
        {
            LogInVis= Visibility.Visible;
            RegInVis= Visibility.Collapsed;
            RegisterCommand = new RegisterCommand(this);
            LoginCommand= new LoginCommand(this);
            ShowRegisterCommand= new ShowRegisterCommand(this);
            User = new User();
        }
        public void SwitchView()
        {
            isShowingRegister= !isShowingRegister;
            if(isShowingRegister)
            {
                LogInVis = Visibility.Collapsed;
                RegInVis = Visibility.Visible;
            }
            else
            {
                LogInVis = Visibility.Visible;
                RegInVis = Visibility.Collapsed;
            }
        }
        public async void Login()
        {
            bool result = await FirebaseAuthHelper.Login(User);
            if(result) 
            {
                Authenticate(this,new EventArgs());
            }
        }
        public async void Register()
        {
            bool result = await FirebaseAuthHelper.Register(User);
            if (result)
            {
                Authenticate(this, new EventArgs());
            }
        }
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
