using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Reflection.PortableExecutable;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static Restaurant.Food;
using static Restaurant.Person;

namespace Restaurant
{
    internal class Restaurant
    {
        public int TotalGuests { get; set; }
        public int GuestAmount { get; set; }
        public int ChefAmount { get; set; }
        public int WaiterAmount { get; set; }
        public int DuoTableAmount { get; set; }
        public int QuadTableAmount { get; set; }
        public int TimeCounter { get; set; }
        public int GuestsInRestaurant { get; set; }
        public List<string> NewsFeed { get; set; }
        public List<Food> Menu { get; set; }
        public List<Chef> Chefs { get; set; }
        public List<List<Guest>> Companies { get; set; }//ska vi använda Queue av listor
        public Queue Guests { get; set; }
        public Dictionary<string, Table> Tables { get; set; }

        public List<Waiter> Waiters { get; set; }



        public Restaurant(int guestAmount, int chefAmount, int waiterAmount, int duoTableAmount, int quadTableAmount)
        {
            GuestAmount = guestAmount;
            ChefAmount = chefAmount;
            WaiterAmount = waiterAmount;
            DuoTableAmount = duoTableAmount;
            QuadTableAmount = quadTableAmount;

            MakeWaiters();
            MakeChefs();
            MakeMenu();
            MakeTables();
            MakeCompanies();

            TimeCounter = 0;
            TotalGuests = 0; //Counter. När 80 gäster kommit in så serveras inga fler gäster för kvällen.
            GuestsInRestaurant = 0; //Får vara max 30 i restaurangen samtidigt.
            NewsFeed = new List<string>();

        }
        public Restaurant()
        {

        }
        //
        // Skaparrutiner som körs en gång
        // Skapa sällskapen
        public void MakeCompanies()
        {
            Guests = new Queue();
            List<Guest> oneCompany = new List<Guest>();
            Companies = new List<List<Guest>>();

            Random rnd = new Random();

            for (int i = 0; i < GuestAmount; i++)
                Guests.Enqueue(new Guest());

            while (Guests.Count > 4)
            {
                oneCompany.Clear();
                //if (Guests.Count > 4)
                //{
                int companySize = rnd.Next(1, 4);

                for (int i = 0; i < companySize; i++)
                    oneCompany.Add(Guests.Dequeue() as Guest);

                Companies.Add(new List<Guest>(oneCompany));
                int companyIndex = 0;
            }
        }
        // Skapa kyparna
        private void MakeWaiters()
        {
            Waiters = new List<Waiter>();
            for (int i = 0; i < WaiterAmount; i++)
            {
                Waiter waiter = new Waiter();
                Waiters.Add(waiter);
            }
        }
        // Skapa kockarna
        private void MakeChefs()
        {
            Chefs = new List<Chef>();
            for (int i = 0; i < ChefAmount; i++)
            {
                Chef chef = new Chef();
                Chefs.Add(chef);
            }
        }
        // Skapa alla tomma bord
        public void MakeTables() 
        {
            Tables = new Dictionary<string, Table>();
            int tableCount = 1;
            int tableSize = 2;

            for (int i = 0; i < DuoTableAmount + QuadTableAmount; i++)
            {
                int tableNumber = tableCount++;
                string tableName = "Bord " + tableNumber;
                if (i >= DuoTableAmount) tableSize = 4;
                int tableXpos = (i % 3) * 18 + 2;  // (i%3) * 18 ger 0, 18, 36
                int tableYpos = (i % 5) * 8 + 12; // (i%5) * 10 ger 0, 5, 10, 15, 20

                // Lägg in bordet i stora listan med namnet som nyckel
                Tables.Add(tableName, new Table(tableName, tableSize, tableXpos, tableYpos, tableNumber));
            }
        }

        // Skapa meny
        public void MakeMenu()
        {
            Menu = new List<Food>
            {
                new Meat("Plankstek", 339),
                new Meat("Pasta Carbonara", 229.50),
                new Meat("Fläskfilé", 289.50),
                new Fish("Sushi", 198.90),
                new Fish("Fiskgratäng", 209.50),
                new Fish("Panerad rödspätta", 229),
                new Vego("Vegoschnitzel med pommes", 199),
                new Vego("Kronärtskockspizza", 179.50),
                new Vego("Vegetarisk bolognese (linser)", 239)
            };
        }


        // -----------------------------------------------------
        // Själva körningen som går cykliskt
        public void Run()
        {
            while (true)
            {
                // Hitta en gäst i kön, leta efter bord och sedan en kypare
                FetchAndPlaceCompany();

                // Ta upp en order från bord som väntar på att beställa
                TakeOrder();

                // Placera order i köket
                PlaceOrder();


                Console.Clear();
                PrintStatus();
                PrintNews();
                PrintTables();
                PrintWaiters();
                Console.ReadLine();
                TimeCounter++;

                // Uppdatera räknare
                UpdateCounters();
            }

        }

        //
        // Sökrutiner
        // Hitta ett sällskap i kön
        public List<Guest>FindNextCompany(List<List<Guest>> Companies)
        {
            if (Companies.Count > 0)
                return Companies[0];
            else
                return null;
        }

        // Hitta ett ledigt bord
        public Table FindFreeTable(List<Guest> company)
        {
            int companySize = company.Count;
             for (int i = 0; i < Tables.Count; i++)
                 if (Tables.ElementAt(i).Value.Empty && Tables.ElementAt(i).Value.Size >= companySize)
                     return Tables.ElementAt(i).Value;
            return null;
        }

        // Hitta en ledig kypare
        internal Waiter FindFreeWaiter(List<Waiter> waiters)
        {
            foreach (Waiter w in waiters)
                if (w.Busy == 0 && w.Orders.Count == 0)
                    return w;
            return null;

        }
        // Hitta en ledig kock
        internal Chef FindFreeChef(List<Chef> chefs)
        {
            foreach (Chef c in chefs)
                if (c.Busy == 0 && c.Orders.Count == 0)
                    return c;
            return null;

        }
        // Hitta en kypare som har en order till köket
        internal Waiter FindWaiterWithOrder(List<Waiter> waiters)
        {
            foreach (Waiter w in waiters)
                if (w.Busy == 0 && w.Orders.Count > 0)
                    return w;
            return null;

        }
        // Hitta ett bord som vill beställa
        public Table FindTableWaitingToOrder()
        {
            for (int i = 0; i < Tables.Count; i++)
                if (Tables.ElementAt(i).Value.WaitingToOrder && Tables.ElementAt(i).Value.OrderAt == 0)
                    return Tables.ElementAt(i).Value;
            return null;
        }
        internal Food PickRandomFood(List <Food>Menu)
        {
            Random rnd = new Random();
            int foodIndex = rnd.Next(1, Menu.Count);
            return Menu.ElementAt(foodIndex);

        }

        //
        // Utföranderutiner
        // Placera ett sällskap vid ett bord
        internal void SeatCompanyAtTable(List<Guest> company,Table table, Waiter waiter)
        {
            Table.SeatCompany(table, company,waiter,TimeCounter);
            waiter.Busy = waiter.ServiceLevel; // Kyparen tar olika lång tid på sig beroende på ServiceLevel
            GuestsInRestaurant += company.Count;
            return;
        }

        // Ta upp en beställning
        internal void TakeOrder()
        {
            Table waitingTable = FindTableWaitingToOrder();
            if (waitingTable != null)
            {
                waitingTable.OrderAt = TimeCounter;
                waitingTable.WaitingToOrder = false;
                waitingTable.WaitingForFood = true;
                for (int i = 0; i< waitingTable.GuestsAtTable.Count; i++)
                    waitingTable.Waiter.Orders.Add(PickRandomFood(Menu));

                waitingTable.Waiter.Busy = waitingTable.Waiter.ServiceLevel;
                NewsFeed.Add("Ta upp en order");
            }
        }

        // Placera ordern i köket
        internal void PlaceOrder()
        {
            Waiter waiter = FindWaiterWithOrder(Waiters);
            if (waiter != null)
            {
                Chef chef = FindFreeChef(Chefs);
                if (chef != null)
                {
                    foreach (Food f in waiter.Orders)
                        chef.WorkOrder.Add(f);
                    waiter.Orders.Clear();
                    chef.Busy = 20 + chef.Skills;
                    NewsFeed.Add("Kocken fick ordern");
                }
            }
        }
        internal void PickupFood()
        {
            Chef chef = FindReadyChef(Chefs);
            if (chef != null)
            {
                Waiter waiter = FindFreeChef(Chefs);
                if (chef != null)
                {
                    foreach (Food f in waiter.Orders)
                        chef.WorkOrder.Add(f);
                    waiter.Orders.Clear();
                    chef.Busy = 20 + chef.Skills;
                    NewsFeed.Add("Kocken fick ordern");
                }
            }
        }

        // Hämta och placera ett sällskap vid ett bord
        internal void FetchAndPlaceCompany()
        {
            List<Guest> nextCompany = FindNextCompany(Companies);
            if (nextCompany != null)
            {
                Table freeTable = FindFreeTable(nextCompany);
                if (freeTable != null)
                {
                    Waiter freeWaiter = FindFreeWaiter(Waiters);
                    if (freeWaiter != null)
                    {
                        SeatCompanyAtTable(nextCompany, freeTable, freeWaiter);
                        Companies.Remove(nextCompany);
                        NewsFeed.Add("Gäst fått bord");
                    }
                    else
                        NewsFeed.Add("Ingen ledig kypare");
                }
                else
                    NewsFeed.Add("Inget ledigt bord");
            }
        }
        //
        // Servicerutiner
        // Uppdatera räknare
        internal void UpdateCounters()
        {
            foreach (Waiter w in Waiters)
                if (w.Busy > 0) w.Busy--;
            foreach (Chef c in Chefs)
                if (c.Busy > 0) c.Busy--;
        }

        // Skriv ut status
        internal void PrintStatus()
        {
            string[] graphics = new string[3];
            graphics[0] = "Antal gäster:  " + GuestsInRestaurant;
            graphics[1] = "Sällskap i kö: " + Companies.Count;
            graphics[2] = "Tid:           " + TimeCounter;

            GUI.Window.Draw("Status", 2, 6, graphics);

        }
        // Skriv ut nyhetsrutan
        internal void PrintNews()
        {
            int i = 0;
            string[] graphics = new string[NewsFeed.Count];
            foreach (String s in NewsFeed)
                graphics[i++] = s;
            GUI.Window.Draw("Nyheter", 58, 6, graphics);

        }
        // Skriv ut borden
        internal void PrintTables()
        {
            for (int i = 0; i < Tables.Count; i++)
                Table.DrawMe(Tables.ElementAt(i).Value);
        }

        // Skriv ut kyparna
        internal void PrintWaiters()
        {
            for (int i = 0; i < Waiters.Count; i++)
                Waiter.DrawMe(Waiters.ElementAt(i));
        }



        // Skriv ut maten i menyn
        internal void PrintFood()
        {
            Console.WriteLine("**KÖTTRÄTTER**");
            foreach (Food d in Menu)
                if (d is Meat)
                {
                    Console.WriteLine(d.Name + " " + d.Price + " kr");
                    //Console.WriteLine();
                }
            Console.WriteLine("\n**FISKRÄTTER**");
            foreach (Food d in Menu)
                if (d is Fish)
                {
                    Console.WriteLine(d.Name + " " + d.Price + " kr");
                    //Console.WriteLine();
                }
            Console.WriteLine("\n**VEGETARISKA RÄTTER**");
            foreach (Food d in Menu)
                if (d is Vego)
                {
                    Console.WriteLine(d.Name + " " + d.Price + " kr");
                    //Console.WriteLine();
                }


            Console.WriteLine();
            Console.ReadLine();



        }

        // Skriv ut en lista
        public static void DrawAnyList<T>(string header, int fromLeft, int fromTop, List<T> anyList)
        {
            string[] graphics = new string[anyList.Count];

            for (int i = 0; i < anyList.Count; i++)
            {

                if (anyList[i] is Person)
                {
                    graphics[i] = (anyList[i] as Person).Name;
                }



            }
            GUI.Window.Draw(header, fromLeft, fromTop, graphics);
        }
    }
}








