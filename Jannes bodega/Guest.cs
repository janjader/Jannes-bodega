using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant
{
    internal class Guest : Person
    {
        public List<Food> FoodChoice = new List<Food>();
        public double AmountOfMoney { get; set; }
        public int Satisfaction { get; set; }


        public bool Eating { get; set; }

        public int EatingTime { get; set; }
        public bool FinishedEating { get; set; }


        public Guest()
        {
            Random rnd = new Random();
            AmountOfMoney = rnd.Next(100, 2000);

        }

   

        public void EatingGuest()
        {
            if (Eating == false && FinishedEating == false)
            {
                Eating = true;
            }
        }

        public void TimeToEat()
        {
            if (Eating == true)
            {

                EatingTime++;
            }
            if (EatingTime == 20)
            {
                FinishedEating = true;
                Eating = false;
            }
        }

    }
}
