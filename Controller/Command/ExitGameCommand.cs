using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Game2.Command
{
    public class ExitGamenCommand : ICommand
    {
        public int PlayerId { get; set; }
        public bool Execute(GameModelServer model)
        {
            Maze maze = model.Maze;
            var player = maze.GetPlayer(PlayerId);
            player.LastAction = "Exiting the game...";
            player.isDead = true;
            return true;
        }
    }
    public class InvalidKeyActionCommand : ICommand
    {
        public int PlayerId { get; set; }
        public bool Execute(GameModelServer model)
        {
            Maze maze = model.Maze;
            var player = maze.GetPlayer(PlayerId);
            player.LastAction = "Invalid key pressed. Try again.";
            // game.Singleton.UpdateLastAction("Invalid key pressed. Try again.");
            return false;
        }
    }
}
