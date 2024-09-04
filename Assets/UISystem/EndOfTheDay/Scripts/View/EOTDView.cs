using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Economy;
using System;

namespace UISystem
{
    public class EOTDView : CustomUIComponent
    {
        [SerializeField] Text ExpensesOfDay;
        [SerializeField] Text ProfitsOfDay;
        [SerializeField] Text IncomeOfDay;

        public GameObject FirstPanel;

        protected override void Setup() { }

        protected override void Configure() { }

        private void SetExpenses(WalletData wallet)
        {
            foreach (var transaction in wallet.Transactions)
            {
                if (transaction.Receiver == wallet)
                    continue;

                string text = ExpensesOfDay.GetText();
                string value = String.Format("{0:N2}BRL", transaction.Value);
                text = $"{text}\n{transaction.Name} : -{value}";
                ExpensesOfDay.SetText(text);
            }
        }

        private void SetProfits(WalletData wallet)
        {
            foreach (var transaction in wallet.Transactions)
            {
                if (transaction.Sender == wallet)
                    continue;

                string text = ProfitsOfDay.GetText();
                string value = String.Format("{0:N2}BRL", transaction.Value);
                text = $"{text}\n{transaction.Name} : {value}";
                ProfitsOfDay.SetText(text);
            }
        }

        private void SetIncome(WalletData wallet)
        {
            string text = String.Format("{0:N2}BRL", wallet.CurrentMoney);
            IncomeOfDay.SetText(text);
        }

        public void ClearText()
        {
            ExpensesOfDay.SetText("");
            ProfitsOfDay.SetText("");
            IncomeOfDay.SetText("");
        }

        public void SetData(WalletData wallet)
        {
            SetExpenses(wallet);
            SetProfits(wallet);
            SetIncome(wallet);
        }
    }
}