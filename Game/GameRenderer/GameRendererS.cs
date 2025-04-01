using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Game2
{
    // Singleton class for rendering the game to the console
    public sealed class GameRendererS
    {
        private static GameRendererS _instance;
        private int width, height;
        private char[,] screenBuffer;
        private string _instructions;
        private string _last_action = "";
        private string _last_action_tite = "Last action:";

        // Private constructor to prevent direct instantiation
        private GameRendererS()
        {
            width = Console.WindowWidth;
            height = Console.WindowHeight;
            screenBuffer = new char[width, height];
        }

        // Public method to get the singleton instance of the renderer
        public static GameRendererS GetInstance(string instructions)
        {
            if (_instance == null)
            {
                _instance = new GameRendererS();
                _instance._instructions = instructions;
            }
            return _instance;
        }

        // Main method to render the game, including the map, player, items, and status
        public void RenderGame(Player player, Maze maze)
        {
            CheckSize();
            ClearBuffer();
            DrawMap(maze, player);
            DrawStatus(player);
            DrawItemDescription(player, maze.Items);
            DrawLastAction();
            DrawInstructions();
            DrawSurrnfo(player, maze);
            Render();
        }

        private void DrawMap(Maze maze, Player player)
        {
            for (int y = 0; y < Constants.MapHeight; y++)
            {
                for (int x = 0; x < Constants.MapWidth; x++)
                {
                    Draw(x, y, maze.MazeBuffer[x, y]);
                }
            }

            foreach (var item in maze.Items.Keys)
            {
                Draw(item.x, item.y, maze.Items[item].First().GetSymbol());
            }
            foreach(var enemy in maze.Enemies.Keys)
            {
                Draw(enemy.x, enemy.y, 'E');
            }

            Draw(player.position.X, player.position.Y, Constants.Player);
        }

        private void DrawStatus(Player player)
        {
            int statusX = Constants.MapWidth + 2;
            string[] lines = PlayerStat(player).Split('\n');
            for (int y = 0; y < lines.Length; y++)
            {
                DrawText(statusX, y, lines[y]);
            }
        }

        private void DrawLastAction()
        {
            int statusY = Constants.MapHeight + 1;
            int statusX = Constants.MapWidth + 22;
            DrawText(statusX, statusY, _last_action_tite);
            DrawText(statusX, statusY + 1, _last_action);
        }

        private void DrawInstructions()
        {
            int statusY = Constants.MapHeight + 1;
            string[] lines = _instructions.Split('\n');
            for (int y = 0; y < lines.Length; y++)
            {
                DrawText(0, statusY + y, lines[y]);
            }
        }

        private void DrawSurrnfo(Player player, Maze maze)
        {
            int statusY = Constants.MapHeight + 4;
            int statusX = Constants.MapWidth + 22;
            string[] lines = SurroundingOpponents(player, maze).Split('\n');
            for (int y = 0; y < lines.Length; y++)
            {
                DrawText(statusX , statusY + y, lines[y]);
            }
        }

        private void DrawItemDescription(Player player, Dictionary<(int x, int y), List<IItem>> items)
        {
            int statusX = Constants.MapWidth + 22;
            DrawText(statusX, 0, "Item");
            if (items.ContainsKey((player.position.X, player.position.Y)))
            {
                int i = 1;
                foreach (var item in items[(player.position.X, player.position.Y)])
                {
                    DrawText(statusX, i, item.ToString());
                    i++;
                }
            }
        }

        private void CheckSize()
        {
            while (Console.WindowWidth < Constants.MinimumWidth || Console.WindowHeight < Constants.MinimumHeight)
            {
                Console.Clear();
                Console.WriteLine($"[!] Your console size is too small.");
                Console.WriteLine("Please resize the console window and press any key to continue...");
                Console.ReadKey(true);
            }
            if (width != Console.WindowWidth || height != Console.WindowHeight)
            {
                width = Console.WindowWidth;
                height = Console.WindowHeight;
                screenBuffer = new char[width, height];
            }
        }

        private void ClearBuffer()
        {
            for (int y = 0; y < height; y++)
                for (int x = 0; x < width; x++)
                    screenBuffer[x, y] = ' ';
        }

        private void Draw(int x, int y, char c)
        {
            if (x >= 0 && x < width && y >= 0 && y < height)
                screenBuffer[x, y] = c;
        }

        private void DrawText(int x, int y, string text)
        {
            for (int i = 0; i < text.Length && (x + i) < width; i++)
            {
                screenBuffer[x + i, y] = text[i];
            }
        }

        private void Render()
        {
            Console.SetCursorPosition(0, 0);
            for (int y = 0; y < height; y++)
            {
                //Console.SetCursorPosition(0, y);
                for (int x = 0; x < width; x++)
                {
                    Console.Write(screenBuffer[x, y]);
                }
            }
        }

        public string PlayerStat(Player player)
        {
            string status = "Player stats:\n" + player.attributes.ToString() + $"\nCoins: {player.Coins}\nGold: {player.Gold}\n\n";
            status = status + $"Right Hand:\n";
            if (player.Hands[1] != null) status += $"{player.Hands[1].ToString()}\n";
            status = status + $"Left Hand:\n";
            if (player.Hands[0] != null) status += $"{player.Hands[0].ToString()}\n";
            status += "\n";
            status += "Inventory:\n";
            int i = 0;
            foreach (IItem item in player.items)
            {
                if (i == player.CurrentChoosenItem)
                {
                    status += "->";
                }
                status += item.ToString() + "\n";
                i++;
            }
            status += "Potions:\n";
            i = 0;
            foreach (IPotion potion in player.potions)
            {
                //if (i == player.CurrentChoosenItem)
                //{
                //    status += "->";
                //}
                status += potion.Name + "\n";
                i++;
            }
            return status;
        }

        // Method to return information about surrounding enemies
        public string SurroundingOpponents(Player player, Maze maze) 
        {
            string opponents = "";
            if (maze.Enemies.ContainsKey((player.position.X - 1, player.position.Y))) 
                opponents += "!!!To your left, there is an enemy.\n";

            if (maze.Enemies.ContainsKey((player.position.X + 1, player.position.Y)))
                opponents += "!!!To your right, there is an enemy.\n";

            if (maze.Enemies.ContainsKey((player.position.X, player.position.Y - 1)))
                opponents += "!!!Ahead of you, there is an enemy.\n";

            if (maze.Enemies.ContainsKey((player.position.X, player.position.Y + 1)))
                opponents += "!!!Behind you, there is an enemy.\n";
            return opponents;

        }

        public void UpdateLastAction(string action)
        {
            _last_action = action;
        }

        public static void ChooseStrategy(out int stategy)
        {
            Console.Clear();
            Console.WriteLine("Choose your maze strategy:");
            Console.WriteLine("1 - Basic Maze");
            Console.WriteLine("2 - Basic Maze with Items");
            Console.WriteLine("3 - Maze with Central Room");
            Console.WriteLine("4 - Maze with Enemies");
            Console.WriteLine("5 - Empty Maze");

            int choice;
            while (!int.TryParse(Console.ReadLine(), out choice) || choice < 1 || choice > 5)
            {
                Console.WriteLine("Invalid choice! Please enter a number between 1 and 5:");
            }
            stategy = choice;

        }
    }
}
