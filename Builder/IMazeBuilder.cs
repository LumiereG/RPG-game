using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game2
{
    public interface IMazeBuilder
    {
        IMazeBuilder CreateEmptyMaze(int width, int height);
        IMazeBuilder CreateFilledMaze(int width, int height);
        IMazeBuilder GeneratePaths();
        IMazeBuilder AddRooms();
        IMazeBuilder AddCentralRoom();
        IMazeBuilder AddItems(int count);
        IMazeBuilder AddWeapons();
        IMazeBuilder AddModifiedWeapons();
        IMazeBuilder AddElixirs();
        IMazeBuilder AddEnemies();
    }
}
