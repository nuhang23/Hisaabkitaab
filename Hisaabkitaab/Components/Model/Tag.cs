using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hisaabkitaab.Components.Model
{
    public class Tag
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string Name { get; set; }


        public DateTime CreatedAt { get; set; }
    }
}
