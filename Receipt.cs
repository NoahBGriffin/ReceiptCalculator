using System;
using System.Collections.Generic;

namespace Receipts
{
    class Receipt
    {
        public decimal Total { get; private set; }
        public Dictionary<Roommate, decimal> SinglePurchases { get; private set; }
        private decimal salesTax;

        public Receipt(decimal total, decimal tip = 0, decimal tax = 7.80m) //default tax is for snohomish county
        {
            if (total < 0)
                throw new ArgumentException("Total can not be negative");
            if (tip < 0)
                throw new ArgumentException("Tip can not be negative");
            if (tax < 0)
                throw new ArgumentException("Tax can not be negative");
            if (tax > 100)
                throw new ArgumentException("Tax can not be more than 100%"); //i hope it's not 100% oof

            this.salesTax = tax;
            this.Total = total + tip;
            SinglePurchases = new Dictionary<Roommate, decimal>();
        }

        public void AddPurchase(Roommate name, decimal amount)
        {
            if (amount < 0)
                throw new ArgumentException("Amount cannot be negative");

            decimal totalSpent = amount + (amount * (salesTax / 100));    
            if (SinglePurchases.ContainsKey(name))
                SinglePurchases[name] += totalSpent;
            else
                SinglePurchases.Add(name, totalSpent);

            SubtractFromTotal(totalSpent);
        }

        public void SubtractFromTotal(decimal amount)
        {
            if (amount < 0)
                throw new ArgumentException("Can not subtract a negative, please add instead");
            Total -= amount;
        }

        public void AddToTotal(decimal amount)
        {
            if (amount < 0)
                throw new ArgumentException("Can not add a negative, please subtract instead");
            Total += amount;
        }

    }
}