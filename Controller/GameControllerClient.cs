using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Game2.Model
{
    public class GameControllerClient
    {
        public GameModelClient gameState;
        public ConsoleView _view;
        public int Strategy;
        public IKeyActions keyActions;
        public void InitializeGame(List<string> maze, int clientid, string instrutions, int strategy)
        {
            Console.Clear();
            Strategy = strategy;
            gameState = new GameModelClient(maze, clientid);
            _view = new ConsoleView(instrutions);
            BuildKeyChain();
            _view.RenderL(maze);
        }

        public PlayerAction? KeyToAction(ConsoleKey key)
        {
            return keyActions.Handle(key, gameState.Player_id);
        }

        public void UpdateMaze(List<string> maze)
        {
            gameState.maze = new List<string>(maze);
            _view.RenderL(maze);
        }

        public void BuildKeyChain()
        {
            Director director = new Director();
            KeyBuilder keyBuilder = new KeyBuilder();
            director.Builder = keyBuilder;
            director.ChooseStategy(Strategy);
            keyActions = keyBuilder.GetResult();
        }

        public GameControllerClient()
        {
            gameState = new GameModelClient();
        }

    }

    public class GameModelClient 
    {
        public int Player_id { get; set; } = -1;
        public List<string> maze{ get; set; }
       
        public GameModelClient()
        {
        }

        public GameModelClient(List<string> maze, int clientid)
        {
            this.maze = new List<string>(maze);
            Player_id = clientid;
        }
    }
}
