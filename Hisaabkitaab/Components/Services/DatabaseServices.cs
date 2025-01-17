using SQLite;
using Hisaabkitaab.Components.Model;

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
            string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Hisaabkitaab", DB_NAME);

            // Create directory if it doesn't exist
            Directory.CreateDirectory(Path.GetDirectoryName(dbPath));

            // Initialize SQLite connection with ReadWrite flags
            connection = new SQLiteAsyncConnection(dbPath, SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create);

            // Create necessary tables if they don't already exist
            connection.CreateTableAsync<User>().Wait();
            connection.CreateTableAsync<Transactions>().Wait();
            connection.CreateTableAsync<Debt>().Wait();
            connection.CreateTableAsync<Tag>().Wait(); // Ensure the Tag table is created
            connection.CreateTableAsync<TransactionTag>().Wait(); // Ensure the TransactionTag table is created
        }

        // Asynchronous method to create a user
        public async Task CreateUser(User user)
        {
            await connection.InsertAsync(user);
        }

        // Method to get user by email
        public async Task<User> GetUserByEmailAsync(string email)
        {
            return await connection.Table<User>().FirstOrDefaultAsync(u => u.Email == email);
        }

        // Asynchronous method to update user
        public async Task UpdateUser(User user)
        {
            await connection.UpdateAsync(user);
        }

        // Asynchronous method to create a new transaction
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
        public async Task<List<Transactions>> GetTransactionsAsync()
        {
            // Fetch all transactions, sorted by date, without filtering by userId
            return await connection.Table<Transactions>()
                                   .OrderByDescending(t => t.CreatedAt)  // Sorting by date
                                   .ToListAsync();  // Asynchronously fetching the data
        }


        // Asynchronous method to update an existing transaction
        public async Task UpdateTransaction(Transactions updatedTransaction)
        {
            try
            {
                // Update the existing transaction in the database
                await connection.UpdateAsync(updatedTransaction);
            }
            catch (Exception ex)
            {
                // Handle any errors (e.g., database connectivity issues)
                Console.WriteLine($"Error updating transaction: {ex.Message}");
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


        // Asynchronous method to save debt to the database
        public async Task SaveDebtAsync(Debt debt)
        {
            await connection.InsertAsync(debt);
        }

        // Asynchronous method to update debt in the database
        public async Task UpdateDebtAsync(Debt debt)
        {
            await connection.UpdateAsync(debt); // Update existing debt (for transferring funds)
        }

        // Method to calculate the balance by fetching income, expenses, and debts
        public async Task<double> GetBalanceAsync(int userId)
        {
            try
            {
                // Fetch total income and total expenses for the specific user
                double totalIncome = await GetTotalIncomeAsync(userId);
                double totalExpenses = await GetTotalExpensesAsync(userId);

                // Fetch total debt for the specific user
                double totalDebt = await GetTotalDebtAsync(userId);

                // Calculate balance (Income - Expenses + Debts)
                double balance = totalIncome - totalExpenses + totalDebt;

                return balance;
            }
            catch (Exception ex)
            {
                // Handle errors (e.g., database connectivity issues)
                Console.WriteLine($"Error calculating balance: {ex.Message}");
                return 0;  // Return 0 if any error occurs
            }
        }


        // Method to get total income for a specific user
        public async Task<double> GetTotalIncomeAsync(int userId)
        {
            try
            {
                // Fetch transactions of type "Income" for the specific user and sum the amounts
                var incomeTransactions = await connection.Table<Transactions>()
                    .Where(t => t.TransactionType == "Income" && t.UserId == userId)
                    .ToListAsync();

                return incomeTransactions.Sum(t => t.Amount);  // Return the sum of amounts
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching total income: {ex.Message}");
                return 0;  // Return 0 if any error occurs
            }
        }

        // Method to get total expenses for a specific user
        public async Task<double> GetTotalExpensesAsync(int userId)
        {
            try
            {
                // Fetch transactions of type "Expense" for the specific user and sum the amounts
                var expenseTransactions = await connection.Table<Transactions>()
                    .Where(t => t.TransactionType == "Expense" && t.UserId == userId)
                    .ToListAsync();

                return expenseTransactions.Sum(t => t.Amount);  // Return the sum of amounts
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching total expenses: {ex.Message}");
                return 0;  // Return 0 if any error occurs
            }
        }

        // Method to get total debt for a specific user
        public async Task<double> GetTotalDebtAsync(int userId)
        {
            try
            {
                // Fetch all debts for the specific user and sum the amounts
                var debts = await connection.Table<Debt>()
                    .Where(d => d.UserId == userId)
                    .ToListAsync();

                // Return the sum of all debts
                return debts.Sum(d => d.Amount);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching total debt: {ex.Message}");
                return 0;  // Return 0 if any error occurs
            }
        }

        // Method to get all debts for a specific user
        public async Task<List<Debt>> GetDebtsAsync(int userId)
        {
            try
            {
                // Fetch all debts for the specific user
                var debts = await connection.Table<Debt>()
                    .Where(d => d.UserId == userId)
                    .ToListAsync();

                return debts;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching debts: {ex.Message}");
                return new List<Debt>();  // Return an empty list if there's an error
            }
        }

        // Method to get all tags from the database
        public async Task<List<Tag>> GetAllTagsAsync()
        {
            return await connection.Table<Tag>().ToListAsync();
        }


        // Method to get a tag by its name
        public async Task<Tag> GetTagByNameAsync(string name)
        {
            // Use the LIKE operator for case-insensitive comparison in SQLite
            return await connection.Table<Tag>()
                                   .FirstOrDefaultAsync(t => t.Name.ToLower() == name.ToLower());
        }


        // Asynchronous method to create a new tag
        public async Task CreateTagAsync(Tag newTag)
        {
            await connection.InsertAsync(newTag);
        }

        // Method to link a transaction with a tag
        public async Task LinkTransactionWithTag(int transactionId, int tagId)
        {
            var transactionTag = new TransactionTag
            {
                TransactionId = transactionId,
                TagId = tagId
            };
            await connection.InsertAsync(transactionTag);
        }

        public async Task<List<Transactions>> GetFilteredTransactionsAsync(
    string title = null,
    string transactionType = null,
    string tag = null,
    DateTime? startDate = null,
    DateTime? endDate = null,
    bool sortDescending = false)
        {
            // Fetch all transactions from the database
            var transactions = await connection.Table<Transactions>().ToListAsync();

            // Filter by Title
            if (!string.IsNullOrEmpty(title))
            {
                transactions = transactions.Where(t => t.Title.Contains(title, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            // Filter by TransactionType (Income/Expense)
            if (!string.IsNullOrEmpty(transactionType))
            {
                transactions = transactions.Where(t => t.TransactionType.Contains(transactionType, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            // Filter by Tag
            if (!string.IsNullOrEmpty(tag))
            {
                // Retrieve all tags and transaction-tag relations
                var tags = await connection.Table<Tag>().ToListAsync();
                var transactionTags = await connection.Table<TransactionTag>().ToListAsync();

                // Get transaction IDs related to the specified tag
                var tagTransactionIds = transactionTags
                    .Where(tt => tags.Any(t => t.Id == tt.TagId && t.Name.Contains(tag, StringComparison.OrdinalIgnoreCase)))
                    .Select(tt => tt.TransactionId)
                    .ToList();

                // Filter transactions by these tag IDs
                transactions = transactions.Where(t => tagTransactionIds.Contains(t.Id)).ToList();
            }

            // Filter by Date Range (start date and/or end date)
            if (startDate.HasValue)
            {
                transactions = transactions.Where(t => t.CreatedAt >= startDate.Value).ToList();
            }
            if (endDate.HasValue)
            {
                transactions = transactions.Where(t => t.CreatedAt <= endDate.Value).ToList();
            }

            // Sort by Date (ascending or descending)
            if (sortDescending)
            {
                transactions = transactions.OrderByDescending(t => t.CreatedAt).ToList(); // Sort descending
            }
            else
            {
                transactions = transactions.OrderBy(t => t.CreatedAt).ToList(); // Sort ascending
            }

            return transactions;
        }




        public async Task<List<Transactions>> SearchTransactionsAsync(string searchTerm)
        {
            // Perform title search using LIKE (case-insensitive by default in SQLite)
            var titleMatches = await connection.Table<Transactions>()
                                                .Where(t => t.Title.Contains(searchTerm)) // LIKE search for title
                                                .ToListAsync();

            // Search for tags by name using LIKE
            var tagMatches = await connection.Table<Tag>()
                                              .Where(tag => tag.Name.Contains(searchTerm)) // LIKE search for tag name
                                              .ToListAsync();

            // Get the TransactionIds that match the tags
            var tagTransactionIds = new List<int>();

            foreach (var tag in tagMatches)
            {
                var transactionTags = await connection.Table<TransactionTag>()
                                                      .Where(tt => tt.TagId == tag.Id)
                                                      .ToListAsync();
                tagTransactionIds.AddRange(transactionTags.Select(tt => tt.TransactionId));
            }

            // Combine the matching transaction IDs from title and tag searches, ensuring no duplicates
            var matchingTransactionIds = titleMatches.Select(t => t.Id).Concat(tagTransactionIds).Distinct().ToList();

            // Fetch the transactions based on the matching IDs
            var matchingTransactions = await connection.Table<Transactions>()
                                                       .Where(t => matchingTransactionIds.Contains(t.Id))
                                                       .ToListAsync();

            return matchingTransactions;
        }

        public async Task<List<Transactions>> GetFilteredTransactionsAsync(DateTime startDate, DateTime endDate, string searchTerm)
        {
            try
            {
                // Perform title search using LIKE (case-insensitive by default in SQLite)
                var titleMatches = await connection.Table<Transactions>()
                                                    .Where(t => t.Title.Contains(searchTerm)
                                                            && t.CreatedAt >= startDate
                                                            && t.CreatedAt <= endDate)
                                                    .ToListAsync();

                // Search for tags by name using LIKE
                var tagMatches = await connection.Table<Tag>()
                                                  .Where(tag => tag.Name.Contains(searchTerm)) // LIKE search for tag name
                                                  .ToListAsync();

                // Get the TransactionIds that match the tags
                var tagTransactionIds = new List<int>();

                foreach (var tag in tagMatches)
                {
                    var transactionTags = await connection.Table<TransactionTag>()
                                                          .Where(tt => tt.TagId == tag.Id)
                                                          .ToListAsync();
                    tagTransactionIds.AddRange(transactionTags.Select(tt => tt.TransactionId));
                }

                // Combine the matching transaction IDs from title and tag searches, ensuring no duplicates
                var matchingTransactionIds = titleMatches.Select(t => t.Id)
                                                          .Concat(tagTransactionIds)
                                                          .Distinct()
                                                          .ToList();

                // Fetch the transactions based on the matching IDs
                var matchingTransactions = await connection.Table<Transactions>()
                                                           .Where(t => matchingTransactionIds.Contains(t.Id))
                                                           .ToListAsync();

                // Filter by date range
                matchingTransactions = matchingTransactions
                                       .Where(t => t.CreatedAt >= startDate && t.CreatedAt <= endDate)
                                       .ToList();

                return matchingTransactions;
            }
            catch (Exception ex)
            {
                // Handle any errors (e.g., database connectivity issues)
                Console.WriteLine($"Error filtering transactions: {ex.Message}");
                return new List<Transactions>();
            }
        }
        public async Task<List<int>> GetTransactionsByTagIdAsync(int tagId)
        {
            var transactionTags = await connection.Table<TransactionTag>()
                                                   .Where(tt => tt.TagId == tagId)
                                                   .ToListAsync();

            return transactionTags.Select(tt => tt.TransactionId).ToList();
        }



    }
} 