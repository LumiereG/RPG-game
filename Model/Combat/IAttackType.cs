using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game2
{
    public interface IAttackType
    {
        void Visit(IEquipable weapon, Player player, Enemy enemy, int Damage = 0);
        void Visit(LightWeapon weapon, Player player, Enemy enemy, int Damage = 0);
        void Visit(HeavyWeapon weapon, Player player, Enemy enemy, int Damage = 0);
        void Visit(MagicWeapon weapon, Player player, Enemy enemy, int Damage = 0);

        int Defense(IEquipable weapon, Player player);
        int Defense(LightWeapon weapon, Player player);
        int Defense(HeavyWeapon weapon, Player player);
        int Defense(MagicWeapon weapon, Player player);

    }

    public class NormalAttack : IAttackType
    {
        public void Visit(IEquipable weapon, Player player, Enemy enemy, int Damage)
        {
            enemy.ReceiveDamage(0);
        }
        public void Visit(HeavyWeapon weapon, Player player, Enemy enemy, int Damage)
        {
            int weapon_damage = Damage == 0 ? weapon.Damage : Damage;
            int damage = player.attributes.Power + player.attributes.Agression + weapon_damage;
            enemy.ReceiveDamage(damage);
        }

        public void Visit(LightWeapon weapon, Player player, Enemy enemy, int Damage)
        {
            int weapon_damage = Damage == 0 ? weapon.Damage : Damage;
            int damage = player.attributes.Dexterity + player.attributes.Luck + weapon_damage;
            enemy.ReceiveDamage(damage);
        }

        public void Visit(MagicWeapon weapon, Player player, Enemy enemy, int Damage)
        {
            int damage = 1;
            enemy.ReceiveDamage(damage);
        }

        public int Defense(IEquipable weapon, Player player)
        {
            return player.attributes.Dexterity;
        }
        public int Defense(HeavyWeapon weapon, Player player)
        {
            return player.attributes.Power + player.attributes.Luck;
        }

        public int Defense(LightWeapon weapon, Player player)
        {
            return player.attributes.Dexterity + player.attributes.Luck;
        }

        public int Defense(MagicWeapon weapon, Player player)
        {
            return player.attributes.Dexterity + player.attributes.Luck;
        }

    }

    public class SneakyAttack : IAttackType
    {
        public void Visit(HeavyWeapon weapon, Player player, Enemy enemy, int Damage)
        {
            int weapon_damage = Damage == 0 ? weapon.Damage : Damage;
            int damage = (player.attributes.Power + player.attributes.Agression + weapon_damage) / 2;
            enemy.ReceiveDamage(damage);
        }

        public void Visit(LightWeapon weapon, Player player, Enemy enemy, int Damage)
        {
            int weapon_damage = Damage == 0 ? weapon.Damage : Damage;
            int damage = 2 * (player.attributes.Dexterity + player.attributes.Luck + weapon_damage);
            enemy.ReceiveDamage(damage);
        }

        public void Visit(MagicWeapon weapon, Player player, Enemy enemy, int Damage)
        {
            enemy.ReceiveDamage(1);
        }

        public void Visit(IEquipable weapon, Player player, Enemy enemy, int Damage)
        {
            enemy.ReceiveDamage(0);
        }

        public int Defense(HeavyWeapon weapon, Player player) => player.attributes.Power;
        public int Defense(LightWeapon weapon, Player player) => player.attributes.Dexterity;
        public int Defense(MagicWeapon weapon, Player player) => 0;
        public int Defense(IEquipable weapon, Player player) => 0;
    }

    public class MagicAttack : IAttackType
    {
        public void Visit(HeavyWeapon weapon, Player player, Enemy enemy, int Damage)
        {
            enemy.ReceiveDamage(1);
        }

        public void Visit(LightWeapon weapon, Player player, Enemy enemy, int Damage)
        {
            enemy.ReceiveDamage(1);
        }

        public void Visit(MagicWeapon weapon, Player player, Enemy enemy, int Damage)
        {
            int weapon_damage = Damage == 0 ? weapon.Damage : Damage;
            int damage = player.attributes.Wisdom + weapon_damage;
            enemy.ReceiveDamage(damage);
        }

        public void Visit(IEquipable item, Player player, Enemy enemy, int Damage)
        {
            enemy.ReceiveDamage(0);
        }

        public int Defense(HeavyWeapon weapon, Player player) => player.attributes.Luck;
        public int Defense(LightWeapon weapon, Player player) => player.attributes.Luck;
        public int Defense(MagicWeapon weapon, Player player) => player.attributes.Wisdom * 2;
        public int Defense(IEquipable item, Player player) => player.attributes.Luck;
    }

    public class EnemyAttack : IAttackType
    {
        public void Visit(HeavyWeapon weapon, Player player, Enemy enemy, int Damage)
        {
            int weapon_damage = Damage == 0 ? weapon.Damage : Damage;
            int damage = player.attributes.Wisdom + weapon_damage / 2;
            enemy.ReceiveDamage(damage);
        }

        public void Visit(LightWeapon weapon, Player player, Enemy enemy, int Damage)
        {
            int weapon_damage = Damage == 0 ? weapon.Damage : Damage;
            int damage = player.attributes.Wisdom + weapon_damage / 2;
            enemy.ReceiveDamage(damage);
        }

        public void Visit(MagicWeapon weapon, Player player, Enemy enemy, int Damage)
        {
            int weapon_damage = Damage == 0 ? weapon.Damage : Damage;
            int damage = player.attributes.Wisdom + weapon_damage / 2;
            enemy.ReceiveDamage(damage);
        }

        public void Visit(IEquipable item, Player player, Enemy enemy, int Damage)
        {
            int damage = player.attributes.Wisdom;
            enemy.ReceiveDamage(damage);
        }

        public int Defense(HeavyWeapon weapon, Player player) => (player.attributes.Luck + weapon.Damage / 4);
        public int Defense(LightWeapon weapon, Player player) => player.attributes.Luck  + weapon.Damage / 4;
        public int Defense(MagicWeapon weapon, Player player) => player.attributes.Wisdom + weapon.Damage / 4;
        public int Defense(IEquipable item, Player player) => player.attributes.Luck;
    }

}
