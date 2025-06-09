using Game2.Decorator;
using Game2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game2
{
    static public class GameGenerator
    {
        static public char[,] GenerateMap(int mapHeight, int mapWidth)
        {
            Random rand = new Random();
            char[,] mapBuffer = new char[mapHeight, mapWidth];
            for (int y = 0; y < mapHeight; y++)
            {
                for (int x = 0; x < mapWidth; x++)
                {
                    mapBuffer[y, x] = (rand.Next(20) < 2) || x == 0 || y == 0 || x == mapWidth - 1 || y == mapHeight - 1 ? Constants.Wall : ' ';
                }
            }
            return mapBuffer;
        }
        static public Dictionary<(int x, int y), List<IItem>> GenerateItems(int count, char[,] mapBuffer, int mapHeight, int mapWidth)
        {
            Dictionary<(int x, int y), List<IItem>> items = new Dictionary<(int x, int y), List<IItem>> ();
            Random rand = new Random();
            for (int i = 0; i < count; i++)
            {
                int x, y;
                do
                {
                    x = 1 + rand.Next(mapWidth - 2);
                    y = 1 + rand.Next(mapHeight - 2);
                } while (mapBuffer[y, x] == Constants.Wall);

                IItem newItem = ItemFactory.GenerateRandomItem();
                if (items.ContainsKey((x, y)))
                    items[(x, y)].Add(newItem);
                else
                    items[(x, y)] = new List<IItem> { newItem };
            }
            return items;
        }

    }
}
