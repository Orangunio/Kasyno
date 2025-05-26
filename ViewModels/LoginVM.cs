using Kasyno.Helpers;
using Kasyno.Models;
using Kasyno.ViewModels.Commands;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kasyno.ViewModels
{
    public class LoginVM : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
		private void OnPropertyChanged(string propertyName)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        private User user;

		public User User
		{
			get { return user; }
			set { user = value;
                OnPropertyChanged(nameof(User));
            }
		}
		private string username;

		public string Username
		{
			get { return username; }
			set { username = value;
				User = new User(this.username, Password, 0, confrimPassword);
                OnPropertyChanged(nameof(Username));
            }
		}
		private string password;

		public string Password
		{
			get { return password; }
			set { password = value;
				User = new User(Username, this.password, 0, confrimPassword);
				OnPropertyChanged(nameof(Password));
            }
		}
		private string confrimPassword;

		public string ConfrimPassword
		{
			get { return confrimPassword; }
			set { confrimPassword = value;
				User = new User(Username, Password, 0, this.confrimPassword);
				OnPropertyChanged(nameof(ConfrimPassword));
            }
		}
		public RegisterCommand RegisterCommand { get; set; }
		public LoginCommand LoginCommand { get; set; }
		public LoginVM()
		{
			LoginCommand = new LoginCommand(this);
			RegisterCommand = new RegisterCommand(this);
			User = new User();
        }
        public Action? SwitchToLoginView { get; set; }
        public async Task<bool> RegisterAsync()
		{
            if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(confrimPassword) || ConfrimPassword != Password) return false;

            var existingUsers = DataHelper.GetAll<User>();
            if (existingUsers.Any(u => u.Username == Username))
                return false;

            var newUser = new User(Username, Password, 1000, confrimPassword); // startowy balans
            SwitchToLoginView?.Invoke();
            return DataHelper.Insert(newUser);
        }
		public async Task<bool> LoginAsync()
		{
            var users = DataHelper.GetAll<User>();
            var user = users.FirstOrDefault(u => u.Username == Username && u.Password == Password);
            if (user != null)
            {
                App.User = user;
                return true;
            }
            return false;
        }
        public void ClearFields()
        {
            Username = string.Empty;
            Password = string.Empty;
            ConfrimPassword = string.Empty;
        }

    }
}
