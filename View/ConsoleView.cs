using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game2
{
    public class ConsoleView
    {
        public GameRendererS renderer;

        public ConsoleView(string instructions)
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.CursorVisible = false;
            renderer = GameRendererS.GetInstance(instructions);
        }

        public void RenderL(List<string> maze)
        {
            renderer.RenderL(maze);
        }

        public void RenderServer(Maze maze, List<string> logs, List<string> errors)
        {
            renderer.RenderServer(maze, logs, errors);
        }

        public void ShowMessage(string msg)
        {
            renderer.ShowMessage(msg);
        }
    }

}
