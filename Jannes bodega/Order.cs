using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant
{
    internal class Order
    {
        public List<Food> Food = new List<Food>();
        public double Value { get; set; }
        public Table Table { get; set; }
        public Waiter Waiter { get; set; }
        public Chef Chef { get; set; }
        public int OrderedAt { get; set; }
        public int CookedAt { get; set; }


        public Order()
        {

        }
        public Order(List<Food> food, Table table, Waiter waiter, int timecounter)
        {
            Food = food;
            Table = table;
            Waiter = waiter;
            OrderedAt = timecounter;

        }


    }
}
