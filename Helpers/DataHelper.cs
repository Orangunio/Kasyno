using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Kasyno.Helpers
{
    public class DataHelper
    {
        private static readonly string databasePath = GetDatabasePath();
        private static readonly SQLiteAsyncConnection asyncConnection = new SQLiteAsyncConnection(databasePath);

        private static string GetDatabasePath()
        {
            string? projectRoot = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory)?.Parent?.Parent?.Parent?.FullName;
            if (projectRoot == null)
                throw new Exception("Nie można odnaleźć katalogu głównego projektu.");

            string dbFolder = Path.Combine(projectRoot, "Data");

            if (!Directory.Exists(dbFolder))
                Directory.CreateDirectory(dbFolder);

            return Path.Combine(dbFolder, "Kasyno.db");
        }

        public static async Task<bool> InsertAsync<T>(T item) where T : class, new()
        {
            await asyncConnection.CreateTableAsync<T>();
            int rows = await asyncConnection.InsertAsync(item);
            return rows > 0;
        }

        public static async Task<bool> UpdateAsync<T>(T item) where T : class, new()
        {
            await asyncConnection.CreateTableAsync<T>();
            int rows = await asyncConnection.UpdateAsync(item);
            return rows > 0;
        }

        public static async Task<bool> DeleteAsync<T>(T item) where T : class, new()
        {
            await asyncConnection.CreateTableAsync<T>();
            int rows = await asyncConnection.DeleteAsync(item);
            return rows > 0;
        }

        public static async Task<List<T>> GetAllAsync<T>() where T : class, new()
        {
            await asyncConnection.CreateTableAsync<T>();
            return await asyncConnection.Table<T>().ToListAsync();
        }
        public static bool Insert<T>(T item) where T : class, new()
        {
            using var connection = new SQLiteConnection(databasePath);
            connection.CreateTable<T>();
            int rows = connection.Insert(item);
            return rows > 0;
        }

        public static bool Update<T>(T item) where T : class, new()
        {
            using var connection = new SQLiteConnection(databasePath);
            connection.CreateTable<T>();
            int rows = connection.Update(item);
            return rows > 0;
        }
    }
}
