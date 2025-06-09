using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Game2.Model
{
    public class GameControllerServer
    {
        public GameModelServer gameState;
        public ConsoleView _view;

        private IActionHandler actionHandler;

        private MazeBuilder builder;
        private InstructionsBuilder instructions_builder;
        private KeyBuilder key_builder;
        private Director director;
        private HandlerBuilder handlerBuilder;

        public int strategy;

        int current_log = 0;
        public List<string> logs = new List<string>();
        public List<string> errors = new List<string>();

        public string instruction {  get; private set; }

        public GameControllerServer(int strategy)
        {
            this.strategy = strategy;
            builder = new MazeBuilder();
            instructions_builder = new InstructionsBuilder();
            director = new Director();
            handlerBuilder = new HandlerBuilder();

            InitializeGame();
        }

        public void AddLog(string log)
        {
            logs.Add(log);
            PrintServerMaze();
        }

        public void InitializeGame()
        {
            director.Builder = builder;
            director.ChooseStategy(strategy);
            director.Builder = instructions_builder;
            director.ChooseStategy(strategy);
            director.Builder = handlerBuilder;
            director.ChooseStategy(strategy);

            Maze maze = builder.GetResult();
            instruction = instructions_builder.GetResult();
            actionHandler = handlerBuilder.GetResult();

            gameState = new GameModelServer(maze);
            _view = new ConsoleView(instruction);
        }

        public bool HandlePlayerAction(int clientId, string action_type, string direction = null)
        {
            var context = new PlayerActionContext
            {
                PlayerId = clientId,
                Model = gameState,
                ActionType = action_type,
                Direction = direction
            };

            Command.ICommand? command = actionHandler.Handle(context);
            return command.Execute(gameState);
        }

        public void PrintServerMaze()
        {
            _view.RenderServer(gameState.Maze, logs, errors);
        }
    }

    public class PlayerActionContext
    {

        public int PlayerId { get; set; }
        public GameModelServer Model { get; set; }
        public string ActionType { get; set; }
        public string Direction { get; set; }
    }
}
