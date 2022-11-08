using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using static Restaurant.Person;

namespace Restaurant
{
//    internal class Table : Restaurant
    internal class Table
    {
        public string Name { get; set; } = "Template";
        public int Xpos { get; set; } = 0;
        public int Ypos { get; set; } = 0;
        public int Size { get; set; } = 2;
        public int Number { get; set; } = 0;
        public int Quality { get; set; } = 1;
        public bool Empty { get; set; } = true;
        public bool IsClean { get; set; } = true;
        public int TipCounter { get; set; } = 0;
        public int SeatedAt { get; set; } = 0;
        public int OrderAt { get; set; } = 0;
        public int ServedAt { get; set; } = 0;
        public int FinishedAt { get; set; }
        public bool WaitingToOrder { get; set; }
        public bool WaitingForFood { get; set; }
        public bool Eating { get; set; }
        public Waiter Waiter { get; set; }
        public Order Order { get; set; }

        public List<Guest> GuestsAtTable { get; set; }
        public List<Food> FoodChoice { get; set; }

        //osäkert vart denna ska ligga.

        //Dictionary med personer som sitter vid det

        //public List<string> ThingsOnTable { get; set; }  //inväntar onsdagens lektion innan vi går vidare med detta.




        public Table()
        {

        }
        public Table(string name, int size, int xpos, int ypos, int number)
        {
            Name = name;
            Xpos = xpos;
            Ypos = ypos;
            Size = size;
            Number = number;
        }

        public static void DrawMe(Table me)
        {
            if (!me.Empty)
            {
                int companySize = me.GuestsAtTable.Count;
                string[] graphics = new string[companySize + 3];
                for (int i = 0; i < companySize; i++)
                {
                    if (me.GuestsAtTable[i] is Person)
                    {
                        graphics[i] = (me.GuestsAtTable[i] as Person).Name;
                    }
                }
                graphics[companySize] = "Seated at " + me.SeatedAt;
                graphics[companySize+1] = "Ordered at " + me.OrderAt;
                graphics[companySize + 2] = "Waiter " + me.Waiter.Name;


                GUI.Window.Draw(me.Name, me.Xpos, me.Ypos, graphics);
            }
            else
            {
                string[] graphics = new string[1];
                graphics[0] = "";
                GUI.Window.Draw(me.Name, me.Xpos, me.Ypos, graphics);
            }
        }
        public static void SeatCompany(Table me, List<Guest> company,Waiter waiter, int timecounter)
        {
            me.GuestsAtTable = company;
            me.Empty = false;
            me.SeatedAt = timecounter;
            me.Waiter = waiter;
            me.WaitingToOrder = true;
        }
        public static void TakeOrder(Table me, List<Food> food, int timecounter)
        {
            me.Order = new Order(food, me, me.Waiter, timecounter);
            me.OrderAt = timecounter;
            me.WaitingToOrder = false;
            me.WaitingForFood = true;
            me.Waiter.Busy = me.Waiter.ServiceLevel;
            me.Waiter.Order = me.Order;

        }
    }
}
