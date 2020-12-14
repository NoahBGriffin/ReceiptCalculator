using System;
using static System.Console;
using System.Collections.Generic;
using LWTech.CSD228.TextMenus;


namespace Receipts
{

    public enum Roommate {Noah, Chris, Liz}

    class Buyer
    {
        public decimal TotalSpent { get; private set; }
        private Dictionary<Roommate, decimal>  othersIndividualSpending;
        public Roommate Name { get; private set; }
        public Receipt CurrentReceipt { get; private set; }
        public decimal PersonalSpending { get; private set; }

        public Buyer(Roommate name)
        {
            this.Name = name;
            TotalSpent = 0;
            PersonalSpending = 0;
            othersIndividualSpending = new Dictionary<Roommate, decimal>();
            this.CurrentReceipt = new Receipt(0);

            foreach (Roommate roomie in Enum.GetValues(typeof(Roommate)))
            {
                if (roomie != Name)
                    othersIndividualSpending.Add(roomie, 0); 
            }
        }

        public void HoldReceipt(Receipt receipt)
        {
            if (receipt == null)
                throw new ArgumentNullException("Receipt can not be null");

            this.CurrentReceipt = receipt;
        }

        public void AddReceiptToTotals()
        {
            TotalSpent += CurrentReceipt.Total;
            foreach (Roommate r in CurrentReceipt.SinglePurchases.Keys)
            {
                if (r == this.Name)
                    PersonalSpending += CurrentReceipt.SinglePurchases[r];
                else
                    othersIndividualSpending[r] += CurrentReceipt.SinglePurchases[r];
            }
        }

        public static void BuyerMenu(Buyer buyer)
        {
            if (buyer == null)
                throw new ArgumentNullException("Buyer can not be null");
                
            bool done = false;
            TextMenu<Buyer> menu = new TextMenu<Buyer>();

            menu.AddItem(new TextMenuItem<Buyer>($"{buyer.Name}: Normal receipt", ReceiptActions.BasicReceipt));
            menu.AddItem(new TextMenuItem<Buyer>($"{buyer.Name}: Receipt with tip", ReceiptActions.ReceiptWithTip));
            //DELETE ME: potential add - receipt with only one other person?
            // also receipt with alcohol tax ?? or with alcohol taxed purchases
            menu.AddItem(new TextMenuItem<Buyer>("Back to main menu", (p)=>{ done = true; }));

            while (!done)
            {
                int i = menu.GetMenuChoiceFromUser() - 1;
                menu.Run(i, buyer);
            }
        }

        public override string ToString()
        {
            decimal dividedTotal = Decimal.Round(TotalSpent / Enum.GetNames(typeof(Roommate)).Length, 2);

            string s = $"{Name} spent: ${Decimal.Round(TotalSpent, 2)}\n";
            s += $"They also spent ${Decimal.Round(PersonalSpending, 2)} on themselves.\n";
            s += $"Each roommate owes them ${dividedTotal}\n";
            foreach (Roommate spender in othersIndividualSpending.Keys)
            {
                s += $"{spender} spent ${Decimal.Round(othersIndividualSpending[spender], 2)} ";
                s += $"and owes {Name} a total of: ${Decimal.Round(othersIndividualSpending[spender] + dividedTotal, 2)}\n";
            }
            return s;
        }

    }

}