using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace Kasyno.Helpers
{
    public class DataHelper
    {
        private static readonly string databasePath = GetDatabasePath();

        private static string GetDatabasePath()
        {
            // Cofnij się z "bin/Debug/net8.0-windows" do katalogu głównego projektu
            string? projectRoot = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory)?.Parent?.Parent?.Parent?.FullName;

            if (projectRoot == null)
                throw new Exception("Nie można odnaleźć katalogu głównego projektu.");

            string dbFolder = Path.Combine(projectRoot, "Data");

            if (!Directory.Exists(dbFolder))
                Directory.CreateDirectory(dbFolder);

            return Path.Combine(dbFolder, "Kasyno.db");
        }
        public static bool Insert<T>(T item)
        {
            bool result = false;
            using (SQLiteConnection connection = new SQLiteConnection(databasePath))
            {
                connection.CreateTable<T>();
                int rows = connection.Insert(item);
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
