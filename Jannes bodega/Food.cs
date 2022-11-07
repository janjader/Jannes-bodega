using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Restaurant
{
    internal class Food
    {
        public string Name { get; set; }
        public double Price { get; set; }


        public Food(string name, double price)

        {
            Name = name;
            Price = price;
        }
        public Food()
        {

        }


        internal class Meat : Food
        {
            internal Meat(string name, double price) : base(name, price)
            {
            }
        }

        internal class Fish : Food
        {
            internal Fish(string name, double price) : base(name, price)
            { }
        }

        internal class Vego : Food
        {
            internal Vego(string name, double price) : base(name, price)
            {
            }
        }
        //internal void PrintMe()
        //{
        //    Console.WriteLine(Name + " " + Price + " kr");           
        //    Console.WriteLine();
        //}

    }
}