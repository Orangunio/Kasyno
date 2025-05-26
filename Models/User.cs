using SQLite;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Kasyno.Models
{
    public class User : INotifyPropertyChanged
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        [Unique, NotNull]
        public string Username { get; set; }
        public string Password { get; set; }
        [Ignore]
        public string ConfirmPassword { get; set; }
        private int balance;

        public int Balance
        {
            get => balance;
            set
            {
                if (balance != value) 
                {
                    balance = value; 
                    OnPropertyChanged(nameof(Balance));
                }
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        public User() { }
        public User(string username, string password, int balance, string confirmPassword)
        {
            Username = username;
            Password = password;
            this.balance = balance;
            ConfirmPassword = confirmPassword;
        }
    }
}
