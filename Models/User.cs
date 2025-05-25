using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Kasyno.Models
{
    public class User
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        [Unique, NotNull]
        public string Username { get; set; }
        public string Password { get; set; }
        public int balance { get; set; }
        public User() { }
        public User(string username, string password, int balance)
        {
            Username = username;
            Password = password;
            this.balance = balance;
        }
    }
}
