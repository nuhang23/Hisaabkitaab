using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maui.Controls.PlatformConfiguration;
using Hisaabkitaab.Components.Model;
namespace Hisaabkitaab.Components.Services

{
    public class AccountStateService
    {
        // Store the account state
        public Model.Hisaabkitaab.Components.Model.AccountState AccountState { get; private set; } = new Model.Hisaabkitaab.Components.Model.AccountState();

        // Optionally, add a flag to track if data is already fetched
        public bool IsDataLoaded { get; set; } = false;

        // You can provide methods to update the state if needed, like for balance calculations, etc.

        public void SetAccountState(double totalIncome, double totalExpenses, List<Debt> debts)
        {
            AccountState.TotalIncome = totalIncome;
            AccountState.TotalExpenses = totalExpenses;
            AccountState.Debts = debts;
            AccountState.BalanceAmount = totalIncome - totalExpenses;
        }
    }
}
