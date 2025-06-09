using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Game2
{
    public class GameModelServer : ISubject
    {
        private Maze _maze;

        public Maze Maze { get { return _maze; } }
        public string GameMessages { get; set; } = "";

        private List<IPotionObserver> observers = new();

        private readonly object _boardLock = new object();

        public GameModelServer(Maze maze)
        {
            observers = new List<IPotionObserver>();
            _maze = maze;
        }

        public void Attach(IPotionObserver observer) => observers.Add(observer);
        public void Detach(IPotionObserver observer) => observers.Remove(observer);

        public void Notify()
        {
            foreach (var observer in observers.ToList())
            {
                observer.Update(this);
            }
        }

        public Player AddPlayer(int clientid)
        {
            lock (_boardLock)
            {
                if (_maze.Players.Count >= 9) return null;

                int startX = 1, startY = 1;
                bool foundStart = false;
                for (int r = 0; r < _maze.Height; r++)
                {
                    for (int c = 0; c < _maze.Width; c++)
                    {
                        if (_maze.MazeBuffer[c, r] == ' ' && _maze.GetEnemyfromPosition(c, r) == null)
                        {
                            startX = c;
                            startY = r;
                            foundStart = true;
                            foreach (Player player in _maze.Players)
                            {
                                if (player.position.X == startX && player.position.Y == startY)
                                {
                                    foundStart = false;
                                    break;
                                }
                            }
                            break;
                        }
                    }
                    if (foundStart) break;
                }

                Player newPlayer = new Player(new Point(startX, startY), clientid);
                _maze.Players.Add(newPlayer);
                GameMessages = $"{clientid} joined the game!";
                return newPlayer;
            }
        }

        public void RemovePlayer(int playerId)
        {
            lock (_boardLock)
            {
                for (int i = 0; i < _maze.Players.Count; i++)
                {
                    if (_maze.Players[i].ID == playerId)
                    {
                        GameMessages = $"{_maze.Players[i].ID} left the game.";
                        _maze.Players.RemoveAt(i);
                        break;
                    }
                }
            }
        }

    }


    public enum MessageType
    {
        PlayerAction,
        GameStateUpdate,
        InitialState,
        PlayerDisconnected,
        ChatMessage
    }

    public class PlayerAction
    {
        public int PlayerId { get; set; } 
        public string Type { get; set; }
        public string? Payload { get; set; }
    }
}
