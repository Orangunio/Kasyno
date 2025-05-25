using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace Kasyno.Helper
{
    public class DataHelper
    {
        private static string? databasePath = Path.Combine(Environment.CurrentDirectory, "Kasyno.db");
        public static bool Insert<T>(T item)
        {
            bool result = false;
            using (SQLiteConnection connection = new SQLiteConnection(databasePath))
            {
                connection.CreateTable<T>();
                int rows= connection.Insert(item);
                if (rows > 0)
                {
                    result = true;
                }
            }
            return result;
        }
        public static bool Update<T>(T item)
        {
            bool result = false;
            using (SQLiteConnection connection = new SQLiteConnection(databasePath))
            {
                connection.CreateTable<T>();
                int rows = connection.Update(item);
                if (rows > 0)
                {
                    result = true;
                }
            }
            return result;
        }
        public static bool Delete<T>(T item)
        {
            bool result = false;
            using (SQLiteConnection connection = new SQLiteConnection(databasePath))
            {
                connection.CreateTable<T>();
                int rows = connection.Delete(item);
                if (rows > 0)
                {
                    result = true;
                }
            }
            return result;
        }
        public static List<T> GetAll<T>() where T : new()
        {
            using (SQLiteConnection connection = new SQLiteConnection(databasePath))
            {
                connection.CreateTable<T>();
                return connection.Table<T>().ToList();
            }
        }
    }
}
