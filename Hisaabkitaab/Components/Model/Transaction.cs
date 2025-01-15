using System;

namespace Hisaabkitaab.Components.Model
{
    public class Transaction
    {
        public int TransactionId { get; set; } // Unique identifier for the transaction
        public string UserId { get; set; } // Identifier for the user (you can link it to the user profile or account)
        public int Id { get; set; } // A reference id, for example, a category or group id
        public string TransactionType { get; set; } // Type of transaction ("Income" or "Expense")
        public decimal Amount { get; set; } // Amount of money involved in the transaction
        public DateTime CreatedAt { get; set; } // The date and time the transaction was created
        public DateTime? UpdatedAt { get; set; } // The date and time the transaction was last updated (nullable)
    }
}
