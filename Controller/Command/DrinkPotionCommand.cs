using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game2.Command
{
    public class DrinkPotionCommand : ICommand
    {
        public int PlayerId { get; set; }
        public bool Execute(GameModelServer model)
        {
            Maze maze = model.Maze;
            var player = maze.GetPlayer(PlayerId);
            player?.DrinkPotion(model);
            player.LastAction = $"Player {PlayerId} drank a potion.";
            return true;
        }
    }
}
