using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game2.Command
{
    public class EquipCommand : ICommand
    {
        public int PlayerId { get; set; }

        public string Hand {  get; set; }

        public bool Execute(GameModelServer model)
        {
            Maze maze = model.Maze;
            var player = maze.GetPlayer(PlayerId);
            int index_of_Hand = Hand == "left" ? 0 : 1;
            (bool act, IItem? item) = player.Equip(index_of_Hand);
            if (item == null) player.LastAction = "Player {PlayerId}: No item to equip.";
            else player.LastAction = act ? $"Player {PlayerId} equip a {item.Name}" : $"unequip a {item.Name}";
            if (item == null) return true;
            return true;
        }
    }

    public class NavigateInventoryCommand : ICommand
    {
        public int PlayerId { get; set; }

        public string InventoryAction { get; set; }

        public bool Execute(GameModelServer model)
        {
            Maze maze = model.Maze;
            var player = maze.GetPlayer(PlayerId);

            if (InventoryAction == "Up")
            {
                player.CurrentChoosenItem = player.CurrentChoosenItem > 0 ? player.CurrentChoosenItem - 1 : player.CurrentChoosenItem;
                return true;
            }
            if (InventoryAction == "Down")
            {
                player.CurrentChoosenItem = player.CurrentChoosenItem + 1 < player.PlecakCounter ? player.CurrentChoosenItem + 1 : player.CurrentChoosenItem;
                return true;
            }
            return false;
        }
    }
}
