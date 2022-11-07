namespace GUI
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string[] todoList = new string[5];
            todoList[0] = "Diska";
            todoList[1] = "Mata katten";
            todoList[2] = "Städa";
            todoList[3] = "Äta";
            todoList[4] = "Äta2";

            Window.Draw("Att göra", 5, 5, todoList);

        }
    }
}