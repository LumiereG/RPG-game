using Game2.Decorator;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using static Game2.Constants;

namespace Game2
{
    public class Game
    {
        // Define the starting point (origin) of the game.
        public static readonly Point Origin = new Point(1, 1);

        // Private fields for the game objects
        private Maze maze;
        private MazeBuilder builder;
        private InstructionsBuilder instructions_builder;
        private Player player;
        private Director director;
        private GameRendererS singleton;
        private int _stategy;

        // Constructor to initialize game components
        public Game()
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.CursorVisible = false;
            builder = new MazeBuilder();
            instructions_builder = new InstructionsBuilder();

            // Initialize director that will guide the maze building
            director = new Director();
            player = new Player(Origin);

        }

        // Main game loop where the game runs
        public void Run()
        {
            // Choose the maze generation strategy
            GameRendererS.ChooseStrategy(out _stategy);
            director.Builder = builder;
            switch (_stategy)
            {
                case 1:
                    director.BuildBasicMaze();
                    director.Builder = instructions_builder;
                    director.BuildBasicMaze();
                    break;
                case 2:
                    director.BuildBasikMazeWithItems();
                    director.Builder = instructions_builder;
                    director.BuildBasikMazeWithItems();
                    break;
                case 3:
                    director.BuildMazeWithCentralRoom();
                    director.Builder = instructions_builder;
                    director.BuildMazeWithCentralRoom();
                    break;
                case 4:
                    director.BuildMazeWithEnemies();
                    director.Builder = instructions_builder;
                    director.BuildMazeWithEnemies();
                    break;
                case 5:
                    director.BuildEmptyMaze();
                    director.Builder = instructions_builder;
                    director.BuildEmptyMaze();
                    break;
            }

            // Get the resulting maze from the builder
            maze = builder.GetResult();
            singleton = GameRendererS.GetInstance(instructions_builder.GetResult());
            singleton.RenderGame(player, maze);

            // Game loop for handling player input
            while (true)
            {
                if (Console.KeyAvailable)
                {
                    var key = Console.ReadKey(intercept: true).Key;
                    OnKeyPress(key);
                    singleton.RenderGame(player, maze);
                }
            }
        }

        // Method to handle key presses and perform corresponding actions
        public void OnKeyPress(ConsoleKey key)
        {
            string action = "";
            switch (key)
            {
                case ConsoleKey.W:
                    if (player.Move(Direction.Up, maze))
                        action = "Moved Up";
                    break;
                case ConsoleKey.S:
                    if (player.Move(Direction.Down, maze))
                        action = "Moved Down";
                    break;
                case ConsoleKey.A:
                    if(player.Move(Direction.Left, maze));
                        action = "Moved Left";
                    break;
                case ConsoleKey.D:
                    if(player.Move(Direction.Right, maze));
                        action = "Moved Right";
                    break;
                case ConsoleKey.E:
                    IItem? item = PickUp(player.position.X, player.position.Y);
                    if (item != null) action = "Picked up a " + item.Name;
                    break;
                case ConsoleKey.C:
                    item = Drop(player.position.X, player.position.Y);
                    if (item != null) action = "Dropped a " + item.Name;
                    break;
                case ConsoleKey.UpArrow:
                    player.CurrentChoosenItem = player.CurrentChoosenItem > 0 ? player.CurrentChoosenItem - 1 : player.CurrentChoosenItem;
                    break;
                case ConsoleKey.DownArrow:
                    player.CurrentChoosenItem = player.CurrentChoosenItem + 1 < player.PlecakCounter ? player.CurrentChoosenItem + 1 : player.CurrentChoosenItem;
                    break;
                case ConsoleKey.L:
                    (bool act, item) = player.Equip(0);
                    if (item == null) break;
                    else action = act ? "Equip a " + item.Name : "Unequip a " + item.Name;
                    break;
                case ConsoleKey.R:
                    (act, item) = player.Equip(1);
                    if (item == null) break;
                    else action = act ? "Equip a " + item.Name : "Unequip a " + item.Name;
                    break;
                case ConsoleKey.F:
                    player.DrinkPotion();
                    break;
            }
            // Update the game renderer with the last action performed
            singleton.UpdateLastAction(action);

        }

        // Method to pick up an item at the specified coordinates
        public IItem PickUp(int x, int y)
        {
            if (maze.Items.TryGetValue((x, y), out var itemList) && itemList.Count > 0)
            {
                IItem firstItem = itemList[0];
                itemList.RemoveAt(0);
                if (itemList.Count == 0)
                {
                    maze.Items.Remove((x, y));
                }
                player.PickUp(firstItem);
                return firstItem;
            }
            return null;
        }

        // Method to drop an item at the specified coordinates
        public IItem Drop(int x, int y)
        {
            IItem newItem = player.Drop();
            if (newItem != null)
            {
                if (maze.Items.ContainsKey((x, y))) maze.Items[(x, y)].Add(newItem);
                else maze.Items[(x, y)] = new List<IItem> { newItem };
            }
            return newItem;
        }

    }
    }
