using System;
using System.Collections.Generic;
using System.Drawing;
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
        public char[,] MazeBuffer { get; set; }
        public Dictionary<(int x, int y), List<IItem>> Items;
        public List<Enemy> Enemies;
        public List<Player> Players { get; set; } = new List<Player>();

        public Maze(int width, int height)
        {
            Width = width;
            Height = height;
            MazeBuffer = new char[width, height];
            Items = new Dictionary<(int x, int y), List<IItem>>();
            Enemies = new List<Enemy>();
        }

        public Player GetPlayer(int clientid)
        {
            foreach (var player in Players)
            {
                if (player.ID == clientid)
                {
                    return player;
                }
            }
            return null;
        }

        public Point GetEnemyPosition(Enemy enemy)
        {
            return enemy.Position;
        }

        public bool IsCellOpen(int x, int y)
        {
            if (MazeBuffer[x, y] == ' ' && !Items.ContainsKey((x, y))) return true;
            return false;
        }

        public bool IsCellOccupiedByEnemy(int x, int y)
        {
            foreach(var enemy in Enemies)
            {
                if (enemy.Position.X == x && enemy.Position.Y == y) return true;
            }
            return false;
        }
        public bool IsCellOccupiedByPlayer(int x, int y)
        {
           foreach(Player player in Players)
            {
                if (player.position.X == x && player.position.Y == y) return true;
            }
           return false;
        }

        public Enemy GetEnemyfromPosition(int x, int y)
        {
            foreach (var enemy in Enemies)
            {
                if (enemy.Position.X == x && enemy.Position.Y == y) return enemy;
            }
            return null;
        }

        public Player? FindClosestPlayer(Point currentPosition)
        {
            Player? closestPlayer = null;
            double minDistanceSq = double.MaxValue;

            foreach (var player in Players)
            {
                if (player.isDead) continue;

                double dx = player.position.X - currentPosition.X;
                double dy = player.position.Y - currentPosition.Y;
                double distanceSq = dx * dx + dy * dy; // Use squared distance to avoid sqrt

                if (distanceSq < minDistanceSq)
                {
                    minDistanceSq = distanceSq;
                    closestPlayer = player;
                }
            }
            return closestPlayer;
        }

        public List<Point> FindShortestPath(Point start, Point end)
        {
            Queue<Point> queue = new Queue<Point>();
            Dictionary<Point, Point?> cameFrom = new Dictionary<Point, Point?>();
            queue.Enqueue(start);
            cameFrom[start] = null;

            int[] dx = { 0, 0, -1, 1 };
            int[] dy = { -1, 1, 0, 0 };

            while (queue.Count > 0)
            {
                Point current = queue.Dequeue();

                if (current == end)
                {
                    List<Point> path = new List<Point>();
                    while (current != start)
                    {
                        path.Add(current);
                        current = cameFrom[current].Value;
                    }
                    path.Reverse();
                    return path;
                }

                for (int i = 0; i < 4; i++)
                {
                    Point neighbor = new Point(current.X + dx[i], current.Y + dy[i]);

                    if (neighbor.X < 0 || neighbor.X >= Width || neighbor.Y < 0 || neighbor.Y >= Height)
                    {
                        continue;
                    }

                    if (cameFrom.ContainsKey(neighbor))
                    {
                        continue;
                    }


                    if (neighbor != end) // Только для промежуточных ячеек
                    {
                        if (MazeBuffer[neighbor.X, neighbor.Y] == '█') // Если это стена или заблокировано предметом (согласно вашему IsCellOpen)
                        {
                            continue;
                        }
                        // Ваша существующая проверка на других врагов хороша:
                        if (IsCellOccupiedByEnemy(neighbor.X, neighbor.Y))
                        {
                            continue;
                        }
                    }

                    if (IsCellOccupiedByEnemy(neighbor.X, neighbor.Y)) continue;

                    cameFrom[neighbor] = current;
                    queue.Enqueue(neighbor);
                }
            }

            return new List<Point>(); // Brak ścieżki
        }


        public void MoveEnemy(Enemy enemy, Point newPosition) 
        {
           enemy.Position = newPosition;
        }
    }

    public class MazeDto
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public List<string> MazeBuffer { get; set; }
    }

    public static class MazeConverter
    {

        public static MazeDto ToDto(this Maze maze, int clientid, string instructions)
        {
            Player player = maze.GetPlayer(clientid);
            char[,] maze_p = new GameRenderer(instructions).RenderGame(maze, player);

            var buffer = new List<string>();
            for (int y = 0; y < maze_p.GetLength(1); y++)
            {
                var sb = new StringBuilder();
                for (int x = 0; x < maze_p.GetLength(0); x++)
                {
                    sb.Append(maze_p[x, y]);
                }
                buffer.Add(sb.ToString());
            }

            return new MazeDto
            {
                Width = maze.Width,
                Height = maze.Height,
                MazeBuffer = new List<string>(buffer)
            };
        }


        public static char[,] FromDto(this MazeDto dto)
        {
            var array = new char[dto.Height, dto.Width];
            for (int y = 0; y < dto.Height; y++)
            {
                for (int x = 0; x < dto.Width; x++)
                {
                    array[x, y] = dto.MazeBuffer[x][y];
                }
            }

            return array;
        }
    }
}
