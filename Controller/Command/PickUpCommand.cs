using Game2.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game2.Command
{
    public class PickUpCommand : ICommand
    {
        public int PlayerId { get; set; }

        public bool Execute(GameModelServer model)
        {
            Maze maze = model.Maze;
            var player = maze.GetPlayer(PlayerId);
            var item = PickUp(player.position.X, player.position.Y, maze, player);
            if (item == null) return false;
            // game.Singleton.UpdateLastAction(item != null ? $"Picked up {item.Name}" : "No item to pick up.");
            return true;

        }
        public IItem PickUp(int x, int y, Maze maze, Player player)
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
    }
}
