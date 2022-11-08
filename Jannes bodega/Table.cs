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
        public int SeatedAt { get; set; } = 0;
        public int OrderAt { get; set; } = 0;
        public int ServedAt { get; set; } = 0;
        public int EatingTime { get; set; }
        public int Cleaning { get; set; }
        public int FinishedAt { get; set; }
        public bool WaitingToOrder { get; set; }
        public bool WaitingForFood { get; set; }
        public bool Eating { get; set; }
        public Waiter Waiter { get; set; }
        public Order Order { get; set; }

        public List<Guest> GuestsAtTable { get; set; }

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
            if (me.Empty && me.IsClean)
            {
                string[] graphics = new string[1];
                graphics[0] = "";
                GUI.Window.Draw(me.Name, me.Xpos, me.Ypos, graphics);
            }
            else
            {
                int companySize = me.GuestsAtTable.Count;
                string[] graphics = new string[companySize + 5];
                for (int i = 0; i < companySize; i++)
                {
                    if (me.GuestsAtTable[i] is Person)
                    {
                        graphics[i] = (me.GuestsAtTable[i] as Person).Name;
                    }
                }
                graphics[companySize]   = "Satt ned: " + me.SeatedAt;
                graphics[companySize+1] = "Beställde: " + me.OrderAt;
                graphics[companySize+2] = "Serverad:" + me.ServedAt;
                graphics[companySize+3] = "Ättid: " + me.EatingTime;
                graphics[companySize+4] = "Städas: " + me.Cleaning;

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
            me.Waiter.Busy = 0; // me.Waiter.ServiceLevel;
            me.Waiter.Order = me.Order;

        }
        public static void ReceiveFood(Table me, int timecounter)
        {
            me.ServedAt = timecounter;
            me.WaitingToOrder = false;
            me.WaitingForFood = false;
            me.Eating = true;
            me.EatingTime = 20;
        }
        public static double CalculateTip(Table me)
        {
            double tip = 0.0;

            if ((me.FinishedAt - me.SeatedAt) < 20)
                tip = me.Order.Value * 0.15;
            else
                tip = me.Order.Value * 0.1;

            if (me.Waiter.ServiceLevel > 3)
                tip = tip * 0.9;

            if (me.Quality < 3)
                tip = tip * 0.9;

            return tip;

        }
        public static void PayAndLeave(Table me, int timecounter)
        {
            me.FinishedAt = timecounter;
            me.WaitingToOrder = false;
            me.WaitingForFood = false;
            me.Eating = false;
            me.Cleaning = 3;
            me.IsClean = false;
            me.Empty = true;
            me.Waiter.Tip = CalculateTip(me);
        }
        public static void Clear(Table me)
        {
            me.GuestsAtTable = null;
            me.SeatedAt = 0;
            me.OrderAt = 0;
            me.FinishedAt = 0;
            me.ServedAt = 0;
            me.Cleaning = 0;
            me.FinishedAt = 0;
            me.WaitingToOrder = false;
            me.WaitingForFood = false;
            me.Eating = false;
            me.IsClean = true;
            me.Empty = true;
            me.Waiter = null;
            me.Order.Food.Clear();
            me.Order = null;
        }
    }
}
