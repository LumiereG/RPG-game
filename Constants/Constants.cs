using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game2
{
    public static class Constants
    {
        static public readonly int MapWidth = 43;
        static public readonly int MapHeight = 23;
        static public readonly char Wall = '█';
        static public readonly char Player = '¶';

        static public readonly int MinimumWidth = MapWidth + 35;
        static public readonly int MinimumHeight = MapHeight + 10;

        public enum Direction
        {
            Up,
            Down,
            Left,
            Right
        }
    }
}
