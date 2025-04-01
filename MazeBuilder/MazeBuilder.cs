using Game2;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game2
{
    public class MazeBuilder : IMazeBuilder
    {
        private Maze _maze;
        private Random _random = new Random();

        private static readonly (int dx, int dy)[] Directions =
        { (0, -2), (0, 2), (-2, 0), (2, 0) };

        public IMazeBuilder CreateEmptyMaze(int width, int height)
        {
            _maze = new Maze(width, height);
            for (int y = 0; y < height; y++)
                for (int x = 0; x < width; x++)
                    _maze.MazeBuffer[x, y] = ' ';
            for (int x = 0; x < width; x++) { _maze.MazeBuffer[x, 0] = Constants.Wall; _maze.MazeBuffer[x, height - 1] = Constants.Wall; }
            for (int y = 0; y < height; y++) { _maze.MazeBuffer[0, y] = Constants.Wall; _maze.MazeBuffer[width - 1, y] = Constants.Wall; }
            return this;
        }

        public IMazeBuilder CreateFilledMaze(int width, int height)
        {
            _maze = new Maze(width, height);
            for (int y = 0; y < height; y++)
                for (int x = 0; x < width; x++)
                    _maze.MazeBuffer[x, y] = Constants.Wall; 
            return this;
        }

        public IMazeBuilder GeneratePaths()
        {
            if (_maze == null) throw new InvalidOperationException("Najpierw stwórz podziemia!");

            int startX = _random.Next(_maze.Width / 2) * 2 + 1;
            int startY = _random.Next(_maze.Height / 2) * 2 + 1;

            _maze.MazeBuffer[startX, startY] = ' ';
            CarvePath(startX, startY);

            return this;
        }

        private void CarvePath(int x, int y)
        {
            if (_maze == null) throw new InvalidOperationException("Najpierw stwórz podziemia!");
            var directions = Directions.OrderBy(d => _random.Next()).ToArray();

            foreach (var (dx, dy) in directions)
            {
                int nx = x + dx, ny = y + dy;
                if (IsInBounds(nx, ny) && _maze.MazeBuffer[nx, ny] == Constants.Wall)
                {
                    _maze.MazeBuffer[x + dx / 2, y + dy / 2] = ' ';
                    _maze.MazeBuffer[nx, ny] = ' ';
                    CarvePath(nx, ny);
                }
            }
        }

        private bool IsInBounds(int x, int y) => x > 0 && x < _maze.Width && y > 0 && y < _maze.Height;

        public IMazeBuilder AddRooms()
        {
            if (_maze == null) throw new InvalidOperationException("Najpierw stwórz podziemia!");
            int roomCount = 3;
            for (int i = 0; i < roomCount; i++)
            {
                int x = _random.Next(1, _maze.Width - 5);
                int y = _random.Next(1, _maze.Height - 5);

                for (int dx = 0; dx < 4; dx++)
                    for (int dy = 0; dy < 4; dy++)
                        _maze.MazeBuffer[x + dx, y + dy] = ' ';
            }
            return this;
        }

        public IMazeBuilder AddCentralRoom()
        {
            if (_maze == null) throw new InvalidOperationException("Najpierw stwórz podziemia!");
            int cx = _maze.Width / 2 - 2;
            int cy = _maze.Height / 2 - 2;
            for (int dx = 0; dx < 5; dx++)
                for (int dy = 0; dy < 5; dy++)
                    _maze.MazeBuffer[cx + dx, cy + dy] = ' ';
            return this;
        }

        public IMazeBuilder AddItems(int count)
        {
            if (_maze == null) throw new InvalidOperationException("Najpierw stwórz podziemia!");
            for (int i = 0; i < count; i++)
            {
                int x, y;
                do
                {
                    x = 1 + _random.Next(_maze.Width - 2);
                    y = 1 + _random.Next(_maze.Height - 2);
                } while (_maze.MazeBuffer[x, y] == Constants.Wall);

                IItem newItem = ItemFactory.GenerateRandomItem();
                if (_maze.Items.ContainsKey((x, y)))
                    _maze.Items[(x, y)].Add(newItem);
                else
                    _maze.Items[(x, y)] = new List<IItem> { newItem };
            }
            return this;
        }
        public IMazeBuilder AddWeapons()
        {
            if (_maze == null) throw new InvalidOperationException("Najpierw stwórz podziemia!");
            int count = 15 + _random.Next(10);
            for (int i = 0; i < count; i++)
            {
                int x, y;
                do
                {
                    x = 1 + _random.Next(_maze.Width - 2);
                    y = 1 + _random.Next(_maze.Height - 2);
                } while (_maze.MazeBuffer[x, y] == Constants.Wall);

                IItem newItem = ItemFactory.GenerateWeapons();
                if (_maze.Items.ContainsKey((x, y)))
                    _maze.Items[(x, y)].Add(newItem);
                else
                    _maze.Items[(x, y)] = new List<IItem> { newItem };
            }
            return this;
        }
        public IMazeBuilder AddModifiedWeapons()
        {
            if (_maze == null) throw new InvalidOperationException("Najpierw stwórz podziemia!");
            int count = 5 + _random.Next(10);
            for (int i = 0; i < count; i++)
            {
                int x, y;
                do
                {
                    x = 1 + _random.Next(_maze.Width - 2);
                    y = 1 + _random.Next(_maze.Height - 2);
                } while (_maze.MazeBuffer[x, y] == Constants.Wall);

                IItem newItem = ItemFactory.GenerateRandomWeapons();
                if (_maze.Items.ContainsKey((x, y)))
                    _maze.Items[(x, y)].Add(newItem);
                else
                    _maze.Items[(x, y)] = new List<IItem> { newItem };
            }
            return this;
        }
        public IMazeBuilder AddElixirs()
        {
            if (_maze == null) throw new InvalidOperationException("Najpierw stwórz podziemia!");
            int count = 5 + _random.Next(10);
            for (int i = 0; i < count; i++)
            {
                int x, y;
                do
                {
                    x = 1 + _random.Next(_maze.Width - 2);
                    y = 1 + _random.Next(_maze.Height - 2);
                } while (_maze.MazeBuffer[x, y] == Constants.Wall);

                IItem newItem = ItemFactory.GenerateRandomPotions();
                if (_maze.Items.ContainsKey((x, y)))
                    _maze.Items[(x, y)].Add(newItem);
                else
                    _maze.Items[(x, y)] = new List<IItem> { newItem };
            }
            return this;

        }
        public IMazeBuilder AddEnemies()
        {
            if (_maze == null) throw new InvalidOperationException("Najpierw stwórz podziemia!");
            int count = 5 + _random.Next(10);
            for (int i = 0; i < count; i++)
            {
                int x, y;
                do
                {
                    x = 1 + _random.Next(_maze.Width - 2);
                    y = 1 + _random.Next(_maze.Height - 2);
                } while (_maze.MazeBuffer[x, y] == Constants.Wall && _maze.Enemies.ContainsKey((x,y)));

                IEnemy enemy = new Enemy();
                _maze.Enemies[(x,y)] = enemy;
 
            }
            return this;

        }

        private IMazeBuilder AddRandomObjects(char symbol, int count)
        {
            if (_maze == null) throw new InvalidOperationException("Najpierw stwórz podziemia!");
            int placed = 0;
            while (placed < count)
            {
                int x = _random.Next(_maze.Width);
                int y = _random.Next(_maze.Height);
                if (_maze.MazeBuffer[x, y] == '.')
                {
                    _maze.MazeBuffer[x, y] = symbol;
                    placed++;
                }
            }
            return this;
        }

        public Maze GetResult() => _maze;

    }
}
