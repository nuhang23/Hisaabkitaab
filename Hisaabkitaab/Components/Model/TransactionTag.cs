using SQLite;


        namespace Hisaabkitaab.Components.Model
        {
            public class TransactionTag
            {
                [PrimaryKey, AutoIncrement]
                public int Id { get; set; }


                public int TransactionId { get; set; }


                public int TagId { get; set; }
            }
        }
