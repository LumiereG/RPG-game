using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Game2.Command
{
    public class AttackCommand : ICommand
    {
        public int PlayerId { get; set; }

        public IAttackType attackType { get; set; }

        public bool Execute(GameModelServer model)
        {
            Maze maze = model.Maze;
            var player = maze.GetPlayer(PlayerId);
            Enemy? enemy = null;
            if (maze.IsCellOccupiedByEnemy(player.position.X - 1, player.position.Y))
                enemy = maze.GetEnemyfromPosition(player.position.X - 1, player.position.Y);

            if (maze.IsCellOccupiedByEnemy(player.position.X + 1, player.position.Y))
               enemy = maze.GetEnemyfromPosition(player.position.X + 1, player.position.Y);

            if (maze.IsCellOccupiedByEnemy(player.position.X, player.position.Y + 1))
               enemy = maze.GetEnemyfromPosition(player.position.X, player.position.Y + 1);

            if (maze.IsCellOccupiedByEnemy(player.position.X, player.position.Y - 1))
                enemy = maze.GetEnemyfromPosition(player.position.X, player.position.Y - 1);

            if (enemy == null) return false;

            bool result;
            result = Attack(player, enemy, maze);

            if (result)
                player.LastAction = "Attack with a normal attack. ";
            return result;

        }

        public bool Attack(Player player, Enemy enemy, Maze maze)
        {
            player.Attack(enemy, attackType);
            if (enemy.IsDead)
            {
                maze.Enemies.Remove(enemy);
                return true;
            }
            enemy.AttackPlayer(player, attackType);
            if (player.HP <= 0)
            {
                player.isDead = true;
            }
            return true;
        }
    }
}
