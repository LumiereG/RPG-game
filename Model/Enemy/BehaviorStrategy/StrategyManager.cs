using Game2.BehaviorStrategy;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game2
{
    public class StrategyManager
    {
        public void UpdateStrategies(List<Enemy> enemies, Maze maze)
        {
            foreach (Enemy enemy in enemies)
            {
                Player? player = null;

                if ((player = maze.FindClosestPlayer(enemy.Position)) != null)
                {
                    List<Point> path = maze.FindShortestPath(enemy.Position, player.position);
                    if (path.Count < 20)
                    {
                        enemy.SetBehavior(new AggressiveBehavior());
                    }
                    if (enemy.HP < enemy.AttackValue * 2)
                    {
                        if (path.Count > 10) enemy.SetBehavior(new CalmBehavior());
                        else enemy.SetBehavior(new SkittishBehavior());
                    }

                }
            }
        }
    }
}
