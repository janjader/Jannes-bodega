using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant
{
    internal class Waiter : Person
    {
        public Order Order { get; set; }
        public int Busy { get; set; }
        public double Tip { get; set; }
        public int ServiceLevel { get; set; }
        public Waiter()
        {
            string[] waiterNames =
{
                "Blixten",
                "Snabbis",
                "Normal",
                "Sävlig",
                "Snigeln",
                "Manuel",
                "Lägg av",
            };
            Random rnd = new Random();
            ServiceLevel = rnd.Next(1, 7);
            Name = waiterNames[ServiceLevel-1];
            Tip = 0.0;
        }
        public static void DrawMe(Waiter me)
        {
            if (me.Order != null)
            {
                int orderSize = me.Order.Food.Count;
                string[] graphics = new string[orderSize + 1];
                for (int i = 0; i < orderSize; i++)
                    graphics[i] = me.Order.Food.ElementAt(i).Name;
                graphics[orderSize] = "Upptagen " + me.Busy;
                GUI.Window.Draw(me.Name, 80, me.ServiceLevel*8, graphics);
            }
            else
            {
                string[] graphics = new string[1];
                graphics[0] = "";
                GUI.Window.Draw(me.Name, 80 ,me.ServiceLevel*8, graphics);
            }
        }
        //public string TakeOrder(Guest guest)
        //{
        //    Random rnd = new Random();

        //    //if (guest.FoodChoice.Count > 0 && guest.Eating == false)
        //    //{
        //    int randomFoodChoice = rnd.Next(0, guest.FoeodChoi.Count); // Slumpmässigt index
        //    Thing stolenThing = victim.Belongings[randomThingIndex];

        //    Orders.AddRange(guest.FoodChoice); // Servitören tar allt från gästens menu

        //    guest.FoodChoice.Clear(); // Rensar gästens beställningslista

        //    // Console.WriteLine("Sällskapet beställde " + Menu.GetType().Name);

        //    //return guest.FoodChoice.Name;
        //    //}
        //    return "";
        //}
    }
}
