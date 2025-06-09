using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game2.BehaviorStrategy
{
    public interface IBehaviorStrategy
    {
        void ExecuteBehavior(Enemy enemy,  Maze maze);
    }

    public class CalmBehavior : IBehaviorStrategy
    {
        public void ExecuteBehavior(Enemy enemy, Maze maze)
        {
            // enemy.LastAction = "is calm."; // Optional: for logging or display
        }
    }

    public class AggressiveBehavior : IBehaviorStrategy
    {
        private const int DETECTION_RANGE = 100; 
        private const int ATTACK_RANGE = 1;  

        public void ExecuteBehavior(Enemy enemy, Maze maze)
        {
            if (enemy.IsDead) return;

            Point enemyPos = maze.GetEnemyPosition(enemy);
            if (enemyPos == Point.Empty) return; 

            Player? targetPlayer = maze.FindClosestPlayer(enemyPos);
            if (targetPlayer == null) return;

            Point playerPos = targetPlayer.position;

            double distance = Math.Sqrt(Math.Pow(playerPos.X - enemyPos.X, 2) + Math.Pow(playerPos.Y - enemyPos.Y, 2));

            if (distance <= DETECTION_RANGE)
            {
                if (distance > 1)
                {
                    MoveTowards(enemy, enemyPos, playerPos, maze);
                }
                distance = Math.Sqrt(Math.Pow(playerPos.X - enemy.Position.X, 2) + Math.Pow(playerPos.Y - enemy.Position.Y, 2));
                if (distance <= ATTACK_RANGE)
                {
                    IAttackType attackType = new EnemyAttack();
                    enemy.AttackPlayer(targetPlayer, attackType);
                    if (targetPlayer.HP <= 0) targetPlayer.isDead = true;
                    if(targetPlayer.isDead) return;
                    targetPlayer.Attack(enemy, attackType);
                    if (enemy.HP <= 0)
                    {
                        maze.Enemies.Remove(enemy);
                    }
                }
            }
        }

        private void MoveTowards(Enemy enemy, Point currentPos, Point targetPos, Maze maze)
        {
            var path = maze.FindShortestPath(currentPos, targetPos);
            if (path.Count > 0)
            {
                var nextPos = path[0];
                if (!maze.IsCellOccupiedByPlayer(nextPos.X, nextPos.Y))
                {
                    maze.MoveEnemy(enemy, nextPos);
                }
            }
        }
    }

    public class SkittishBehavior : IBehaviorStrategy
    {
        private const int DETECTION_RANGE = 10; 

        public void ExecuteBehavior(Enemy enemy,  Maze maze)
        {
            if (enemy.IsDead) return;

            Point enemyPos = maze.GetEnemyPosition(enemy);
            if (enemyPos == Point.Empty) return;

            Player closestPlayer = maze.FindClosestPlayer(enemyPos);

            Point playerPos = closestPlayer.position;

            double distance = Math.Sqrt(Math.Pow(playerPos.X - enemyPos.X, 2) + Math.Pow(playerPos.Y - enemyPos.Y, 2));

            if (distance <= DETECTION_RANGE)
            {
                MoveAwayFrom(enemy, enemyPos, playerPos, maze);
            }
        }

        private void MoveAwayFrom(Enemy enemy, Point currentPos, Point playerPos, Maze maze)
        {
            var possibleMoves = new List<Point>
            {
                new Point(currentPos.X + 1, currentPos.Y),
                new Point(currentPos.X - 1, currentPos.Y),
                new Point(currentPos.X, currentPos.Y + 1),
                new Point(currentPos.X, currentPos.Y - 1)
            };

            Point bestMove = currentPos;
            double bestDistance = Distance(currentPos, playerPos);

            foreach (var move in possibleMoves)
            {
                if (maze.MazeBuffer[move.X, move.Y] == Constants.Wall ) continue;
                if (maze.IsCellOccupiedByEnemy(move.X, move.Y)) continue;
                if (maze.IsCellOccupiedByPlayer(move.X, move.Y)) continue;

                double dist = Distance(move, playerPos);
                if (dist > bestDistance)
                {
                    bestDistance = dist;
                    bestMove = move;
                }
            }

            if (bestMove != currentPos)
            {
                maze.MoveEnemy(enemy, bestMove);
            }
        }

        private double Distance(Point a, Point b)
        {
            int dx = a.X - b.X;
            int dy = a.Y - b.Y;
            return Math.Sqrt(dx * dx + dy * dy);
        }
    }


}
