using System;
using LWTech.CSD228.TextMenus;
using System.Collections.Generic;
using static System.Console;

namespace Receipts
{
    class Program
    {
        public static List<Buyer> household = new List<Buyer>();

        static void Main(string[] args)
        {
            WriteLine("R E C E I P T");
            WriteLine("================================================");

            foreach (Roommate name in Enum.GetValues(typeof(Roommate)))
            {
                household.Add(new Buyer(name));
            }

            bool done = false;
            TextMenu<Buyer> mainMenu = new TextMenu<Buyer>();
            Buyer buyerSelect = null;
            foreach (Buyer b in household)
            {
                mainMenu.AddItem(new TextMenuItem<Buyer>($"Do {b.Name} receipts", (p)=>{ buyerSelect = b; }));
            }
            mainMenu.AddItem(new TextMenuItem<Buyer>("Show Totals", (p)=> { ShowTotals(); }));
            mainMenu.AddItem(new TextMenuItem<Buyer>("Quit", (p)=>{ done = true; }));

            while (!done)
            {
                int i = mainMenu.GetMenuChoiceFromUser() - 1;            
                mainMenu.Run(i, buyerSelect);
                if (i >= household.Count)
                    continue;
                else
                    Buyer.BuyerMenu(buyerSelect);
            }     
        }

        public static void ShowTotals()
        {
            WriteLine("FINAL TOTALS");
            WriteLine("=======================");
            foreach (Buyer name in household)
            {
                WriteLine(name.ToString());
            }
            WriteLine("=======================");
            WriteLine("Press enter to return to main menu...");
            ReadLine();
        }
    }
}
