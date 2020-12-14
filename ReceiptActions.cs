using System;
using static System.Console;
using LWTech.CSD228.TextMenus;

namespace Receipts
{
    static class ReceiptActions
    {

        public static void BasicReceipt(Buyer buyer)
        {
            ReceiptMath(buyer, false);
        }

        public static void ReceiptWithTip(Buyer buyer)
        {
            ReceiptMath(buyer, true);
        }
        private static void ReceiptMath(Buyer buyer, bool didTip)
        {
            decimal receiptTotal = GetMoneyAmountInputs("Enter total for this receipt: $", true);
            decimal tipAmount = 0;
            if (didTip)
            {
                tipAmount = GetMoneyAmountInputs("Please enter tip amount: $", true);
            }
            buyer.HoldReceipt(new Receipt(receiptTotal, tipAmount));

            TextMenu<Buyer> menu = new TextMenu<Buyer>();
            Roommate roomieSelection = buyer.Name;
            bool done = false;

            foreach (Roommate r in Enum.GetValues(typeof(Roommate)))
            {
                menu.AddItem(new TextMenuItem<Buyer>($"{r} bought stuff", (p)=>{ roomieSelection = r; }));
            }
            menu.AddItem(new TextMenuItem<Buyer>($"Receipt complete: Add to {buyer.Name} total", (p)=>{ done = true; }));
            while (!done)
            {
                int i = menu.GetMenuChoiceFromUser() - 1;
                menu.Run(i, buyer);
                if (done)
                    break;
                GetIndividualPurchases(buyer, roomieSelection);
            }

            buyer.AddReceiptToTotals();
        }

        private static void GetIndividualPurchases(Buyer buyer, Roommate roomieSelection)
        {
            WriteLine($"Enter amounts of each item {roomieSelection} purchased here. Amounts will be added to totals before quitting.");
            WriteLine("Enter \"Q\" to quit. Enter a price with \"/\" after to divide the value in half (ie. 3.14/ )"); //this might be worth changing/adding more options later to make more flexible
            WriteLine("-----------------------------------");
            string input = "";
            decimal cost = 0;
            bool divide = false;
            while (input.ToLower() != "q")
            {
                Write("$");
                input = ReadLine();
                input.Trim();
                
                if (input.ToLower() == "q")
                    break;
                else if (input[input.Length - 1] == '/')
                {
                    divide = true;
                    input = input.Substring(0, input.Length - 1);
                }
                if (decimal.TryParse(input, out cost))
                {
                    if (divide && cost != 0)
                        cost /= 2;
                    
                    buyer.CurrentReceipt.AddPurchase(roomieSelection, cost);
                    
                }
            }
        }

        private static decimal GetMoneyAmountInputs(string inputPrompt, bool mustBePositive)
        {
            decimal moneyAmount = 0;
            bool validInput = false;
            Write(inputPrompt);
            while (!validInput)
            {
                string input = ReadLine();
                if (decimal.TryParse(input, out moneyAmount))
                {
                    if (mustBePositive && moneyAmount < 0)
                        WriteLine("Error: value must be positive");
                    else
                        validInput = true;
                }
                else
                {
                    WriteLine("Error: must enter a number");
                }
            }

            return moneyAmount;
        }
    }
}