using SQLite;
using System;

namespace Hisaabkitaab.Components.Model
{
    public class Debt
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }


        public int UserId { get; set; }


        public string Title { get; set; }


        public double Amount { get; set; }


        public string Source { get; set; }


        public DateTime DueDate { get; set; }
       

        public DateTime CreatedAt { get; set; }


        public DateTime UpdatedAt { get; set; }


        public string Status { get; set; }
    }
}
