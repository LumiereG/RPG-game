using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game2.Command
{
    public class DropCommand : ICommand
    {
        public int PlayerId { get; set; }
        public bool Execute(GameModelServer model)
        {
            Maze maze = model.Maze;
            var player = maze.GetPlayer(PlayerId);
            var item = Drop(player.position.X, player.position.Y, player, maze);
            player.LastAction = item != null ? $"Dropped {item.Name}" : "No item to drop.";
            if (item == null) return false;
            return true;
        }
        public IItem Drop(int x, int y, Player player, Maze maze)
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

    public class DropAllCommand : ICommand
    {
        public int PlayerId { get; set; }
        public bool Execute(GameModelServer model)
        {
            Maze maze = model.Maze;
            var player = maze.GetPlayer(PlayerId);
            DropAllItems(player.position.X, player.position.Y, player, maze);
            player.LastAction = "Dropped all items";
            return true;
        }
        public void DropAllItems(int x, int y, Player player, Maze maze)
        {
            IItem newItem;
            do
            {
                newItem = player.Drop();
                if (newItem != null)
                {
                    if (maze.Items.ContainsKey((x, y))) maze.Items[(x, y)].Add(newItem);
                    else maze.Items[(x, y)] = new List<IItem> { newItem };
                }
            } while (newItem != null);
        }
    }
}
