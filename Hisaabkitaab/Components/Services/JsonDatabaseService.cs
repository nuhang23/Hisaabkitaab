using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.Maui.Storage;

using Hisaabkitaab.Components.Model;

namespace Hisaabkitaab.Services
{
    public class JsonDatabaseService
    {
        private readonly string _filePath;

        public JsonDatabaseService()
        {
            // Define the file path where the JSON data will be stored
            _filePath = Path.Combine(FileSystem.AppDataDirectory, "transactions.json");
            Console.WriteLine($"Transactions file path: {_filePath}");
        }

        // Method to read transactions from the JSON file
        public async Task<List<Transaction>> ReadTransactionsAsync()
        {
            if (File.Exists(_filePath))
            {
                var json = await File.ReadAllTextAsync(_filePath);
                return JsonSerializer.Deserialize<List<Transaction>>(json) ?? new List<Transaction>();
            }
            return new List<Transaction>();
        }

        // Method to save transactions to the JSON file
        public async Task SaveTransactionsAsync(List<Transaction> transactions)
        {
            var json = JsonSerializer.Serialize(transactions);
            await File.WriteAllTextAsync(_filePath, json);
        }
    }
}
