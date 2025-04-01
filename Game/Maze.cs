using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;

namespace Game2
{
    public class Maze
    {
        public int Width { get; }
        public int Height { get; }
        public char[,] MazeBuffer { get; }
        public Dictionary<(int x, int y), List<IItem>> Items;
        public Dictionary<(int x, int y), IEnemy> Enemies;

        public Maze(int width, int height)
        {
            Width = width;
            Height = height;
            MazeBuffer = new char[width, height];
            Items = new Dictionary<(int x, int y), List<IItem>>();
            Enemies = new Dictionary<(int x, int y), IEnemy>();
        }
    }
}
