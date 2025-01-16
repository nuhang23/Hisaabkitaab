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
        public async Task CreateUser(User user)
        {
            await connection.InsertAsync(user);
        }
        public async Task<User> GetUserByEmailAsync(string email)
        {
            return await connection.Table<User>().FirstOrDefaultAsync(u => u.Email == email);
        }
        public async Task UpdateUser(User user)
        {
            await connection.UpdateAsync(user);
        }


        public async Task CreateTransaction(Transactions newTransaction)
        {
            try
            {
                // Insert the new transaction into the database
                await connection.InsertAsync(newTransaction);
            }
            catch (Exception ex)
            {
                // Handle any errors (e.g., database connectivity issues)
                Console.WriteLine($"Error inserting transaction: {ex.Message}");
            }
        }

        // Method to get all transactions of a specific type (e.g., "Income" or "Expense")
        public async Task<List<Transactions>> GetTransactionsByTypeAsync(string transactionType)
        {
            try
            {
                // Fetch transactions that match the given type
                var transactions = await connection.Table<Transactions>()
                    .Where(t => t.TransactionType == transactionType)
                    .ToListAsync();

                return transactions;
            }
            catch (Exception ex)
            {
                // Handle any errors (e.g., database connectivity issues)
                Console.WriteLine($"Error fetching transactions: {ex.Message}");
                return new List<Transactions>();
            }

        }
    }
}
