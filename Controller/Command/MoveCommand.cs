using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game2;
using static Game2.Constants;

namespace Game2.Command
{
    public class MoveCommand : ICommand
    {
        public int PlayerId { get; set; }

        public string direction { get; set; }
        public bool Execute(GameModelServer model)
        {
            Maze maze = model.Maze;
            var player = maze.GetPlayer(PlayerId);
            if (direction == "Up")
            {
                if (player.Move(Direction.Up, maze))
                { 
                    player.LastAction = "Moved Up";
                    return true;
                }
                return false;
            }
            if (direction == "Down")
            {
                if (player.Move(Direction.Down, maze))
                {
                    player.LastAction = "Moved Down";
                    return true;
                }
                return false;
            }
            if (direction == "Left")
            {
                if (player.Move(Direction.Left, maze))
                {
                    player.LastAction = "Moved Left";
                    return true;
                }
                return false;
            }
            if (direction == "Right")
            {
                if (player.Move(Direction.Right, maze))
                {
                    player.LastAction = "Moved Right";
                    return true;
                }
                return false;
            }
            return false;
        }
    }
}
