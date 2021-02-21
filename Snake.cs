using System;
using System.Collections.Generic;
using System.Threading;

namespace Snake
{
    class Snake
    {
        private static DateTime userInputTimeout;

        const int lengthField = 10;
        const int widthField = 25;
        static int px, py;
        static string[,] gameField = new string[lengthField, widthField];
        static int[] appleCoords = new int[] {(int)(widthField / 2), (int)(lengthField / 2)};

        static string butt = "";

        static List<List<int>> coords = new List<List<int>>();

        static void Main(string[] args)
        {

            coords.Add(new List<int>() { 5, 0 });
            coords.Add(new List<int>() { 4, 0 });
            coords.Add(new List<int>() { 3, 0 });
            coords.Add(new List<int>() { 2, 0 });
            coords.Add(new List<int>() { 1, 0 });
            coords.Add(new List<int>() { 0, 0 });
            
            while (true)
            {

                userInputTimeout = DateTime.Now.AddMilliseconds(500);
                Thread userInputThread = new Thread(new ThreadStart(DoUserInput));
                userInputThread.Start();

                while (DateTime.Now < userInputTimeout)
                    Thread.Sleep(100);

                userInputThread.Interrupt();

                DoAutomatedProcedures();
            }
        }

        private static void DoUserInput()
        {
            try
            {
                string command = string.Empty;
                while ((command = Console.ReadKey().Key.ToString()) != string.Empty)
                    ProcessUserCommand(command);
            }
            catch (ThreadInterruptedException) {}
        }

        private static void ProcessUserCommand(string button)
        {
            butt = button;
            DoAutomatedProcedures();
        }

        private static void DoAutomatedProcedures()
        {
            if (butt != "")
            {
                int x = coords[0][0], y = coords[0][1];
                for (int i = 1; i < coords.Count; i++)
                {
                    px = coords[i][0];
                    py = coords[i][1];

                    coords[i][0] = x;
                    coords[i][1] = y;

                    x = px;
                    y = py;
                }
            }

            if (butt == "UpArrow")
            {
                coords[0][1] = coords[0][1] - 1;
            } else if (butt == "DownArrow")
            {
                coords[0][1] = coords[0][1] + 1;
            } else if (butt == "RightArrow")
            {
                coords[0][0] = coords[0][0] + 1;
            } else if (butt == "LeftArrow")
            {
                coords[0][0] = coords[0][0] - 1;
            }

            for (int i = 1; i < coords.Count; i++)
            {
                if (coords[i][0] == coords[0][0] && coords[i][1] == coords[0][1])
                {
                    Thread.Sleep(2000);

                    Console.Clear();

                    Console.WriteLine("You Lose!");
                    Console.WriteLine($"Score: {coords.Count - 6}");

                    coords.RemoveAll(item => item.Count == 2);
                    coords.Add(new List<int>() { 5, 0 });
                    coords.Add(new List<int>() { 4, 0 });
                    coords.Add(new List<int>() { 3, 0 });
                    coords.Add(new List<int>() { 2, 0 });
                    coords.Add(new List<int>() { 1, 0 });
                    coords.Add(new List<int>() { 0, 0 });

                    butt = "";

                    Thread.Sleep(5000);
                }
            }

            for (int i = 0; i < lengthField; i++)
            {
                for (int z = 0; z < widthField; z++)
                {
                    gameField[i, z] = ".";
                }
            }

            try
            {
                foreach (List<int> el in coords)
                {
                    gameField[el[1], el[0]] = "#";
                }
            }
            catch (IndexOutOfRangeException)
            {
                Thread.Sleep(3000);

                Console.Clear();

                Console.WriteLine("You Lose!");
                Console.WriteLine($"Score: {coords.Count - 6}");

                coords.RemoveAll(item  => item.Count == 2);
                coords.Add(new List<int>() { 5, 0 });
                coords.Add(new List<int>() { 4, 0 });
                coords.Add(new List<int>() { 3, 0 });
                coords.Add(new List<int>() { 2, 0 });
                coords.Add(new List<int>() { 1, 0 });
                coords.Add(new List<int>() { 0, 0 });

                butt = "";

                Thread.Sleep(5000);
            }

            Console.Clear();

            if (coords[0][0] == appleCoords[0] && coords[0][1] == appleCoords[1])
            {
                Random rnd = new Random((int)DateTime.Now.ToBinary());

                while (true)
                {
                    appleCoords[0] = rnd.Next(0, widthField);
                    appleCoords[1] = rnd.Next(0, lengthField);

                    if (gameField[appleCoords[1], appleCoords[0]] == ".")
                    {
                        break;
                    }
                }

                coords.Add(new List<int>() { px, py });
            }

            gameField[appleCoords[1], appleCoords[0]] = "O";

            for (int i = 0; i < lengthField; i++)
            {
                for (int z = 0; z < widthField; z++)
                {
                    Console.Write(gameField[i, z]);
                }
                Console.WriteLine();
            }
            Console.WriteLine($"Score: {coords.Count - 6}");
        }
    }
}
