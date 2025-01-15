using Hisaabkitaab.Components.Model;
using Microsoft.Maui.Controls;
using SQLite;
using System.IO;
using System.Threading.Tasks;

namespace Hisaabkitaab.Components.Services
{
    public class DatabaseServices
    {
        private const string DB_NAME = "Hisaabkitaab.db";
        private readonly SQLiteAsyncConnection connection;

        public DatabaseServices()
        {
            SQLitePCL.Batteries.Init();

            // Define the path for the database file
            string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Hisaabkitaab", "Hisaabkitaab.db");

            // Create directory if it doesn't exist
            Directory.CreateDirectory(Path.GetDirectoryName(dbPath));

            // Initialize SQLite connection with ReadWrite flags
            connection = new SQLiteAsyncConnection(dbPath, SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create);

            // Create the User table if it doesn't already exist
            connection.CreateTableAsync<User>().Wait();
            // You can uncomment and create additional tables if required, e.g., Transactions, Debt, etc.
            // connection.CreateTableAsync<Transaction>().Wait();
            // connection.CreateTableAsync<Debt>().Wait();
            // connection.CreateTableAsync<TransactionTag>().Wait();
            // connection.CreateTableAsync<Tag>().Wait();
        }

        // Asynchronous method to add a user to the database
        public async Task AddUserAsync(User user)
        {
            try
            {
                // Insert user asynchronously into the User table
                await connection.InsertAsync(user);
            }
            catch (Exception ex)
            {
                // Handle any errors (e.g., database connectivity issues)
                Console.WriteLine($"Error inserting user: {ex.Message}");
            }
        }

        // Method to create a user (this can be kept if you prefer synchronous operations)
        public async Task CreateUser(User user)
        {
            await connection.InsertAsync(user);
        }
    }
}
