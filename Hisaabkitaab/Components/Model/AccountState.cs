using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hisaabkitaab.Components.Model
{
    namespace Hisaabkitaab.Components.Model  // Adjust the namespace if needed
    {
        public class AccountState
        {
            public double TotalIncome { get; set; }
            public double TotalExpenses { get; set; }
            public List<Debt> Debts { get; set; } = new List<Debt>();
            public double BalanceAmount { get; set; }
        }
    }
}