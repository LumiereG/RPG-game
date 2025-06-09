using Game2.BehaviorStrategy;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game2
{
    public class Enemy : IEnemy
    {
        public Point Position { get; set; }
        public int HP { get; private set; }
        public int AttackValue { get; }
        public int Armor { get; }
        public string Name { get; }
        public bool IsDead => HP <= 0;

        public bool isAvaible { get; set; } = true;

        public IBehaviorStrategy CurrentBehavior { get; private set; }

        public Enemy(string name, int hp, int attackValue, int armor)
        {
            Name = name;
            HP = hp;
            AttackValue = attackValue;
            Armor = armor;
            SetBehavior(new AggressiveBehavior());
        }

        public void ReceiveDamage(int damage)
        {
            int actualDamage = Math.Max(0, damage - Armor);
            HP -= actualDamage;
        }

        public void SetBehavior(IBehaviorStrategy newBehavior)
        {
            CurrentBehavior = newBehavior;
        }

        public void UpdateBehavior(Maze maze)
        {
            if (IsDead || !isAvaible) return;
            CurrentBehavior?.ExecuteBehavior(this, maze);
        }

        public void AttackPlayer(Player player, IAttackType? lastAttackUsed = null)
        {
            if (lastAttackUsed == null)
            {
                player.HP -= AttackValue;
            }
            int totalDefense = 0;
            if (player.Hands[0] != null && player.Hands[0].isTwoHanded) totalDefense += player.Hands[0].GetDefense(player, lastAttackUsed);
            else
            {   
                if (player.Hands[0] != null) totalDefense += player.Hands[0].GetDefense(player, lastAttackUsed);
                if (player.Hands[1] != null) totalDefense += player.Hands[1].GetDefense(player, lastAttackUsed);
            }

            int damage = Math.Max(0, AttackValue - totalDefense);
            player.HP -= damage;
        }

    }
}
