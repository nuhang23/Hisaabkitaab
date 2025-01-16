using SQLite;
using System;

namespace Hisaabkitaab.Components.Model
{
    public class Transactions
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public int UserId { get; set; }

        public string Title { get; set; }

        public double Amount { get; set; }

        public string TransactionType { get; set; }

        public DateTime CreatedAt { get; set; }  // Ensure this is public

        public DateTime UpdatedAt { get; set; }  // Ensure this is public

        public string? Note { get; set; }  // Nullable string for optional note

        public Transactions() { }

        public Transactions(int userId, string title, double amount, string transactionType, DateTime createdAt, DateTime updatedAt, string? note = null)
        {
            UserId = userId;
            Title = title;
            Amount = amount;
            TransactionType = transactionType;
            CreatedAt = createdAt;
            UpdatedAt = updatedAt;
            Note = note;  // This can be null
        }
    }
}

