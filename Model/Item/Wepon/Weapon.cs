using Game2.Observer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Game2
{
    public abstract class Weapon : IWeapon
    {
        public int Damage { get; }

        protected string _name;

        public virtual string Name => _name;

        public void PickMeUp(Player player)
        {
            player.items.Add(this);
        }

        public char GetSymbol() => 'W';

        public string EffectToString => Effects.FormatAttributes();

        public string DamageToString => $" Damage: {Damage}";

        public override string ToString() => Name + EffectToString + DamageToString;

        public Attributes Effects { get; } = new Attributes{Power = 5};

        public bool isTwoHanded => false;

        public Weapon(int damage, string name, Attributes effects)
        {
            Damage = damage;
            _name = name;
            Effects = effects;
        }

        public void GiveEffects(Player player, bool add)
        {
            if (add) player.attributes += Effects;
            else player.attributes -= Effects;
        }
        public bool TryToUse(Player player, ISubject subject) => false;
        public abstract void Accept(IAttackType attack, Player player, Enemy enemy, int Damage = 0);
        public abstract int GetDefense(Player player, IAttackType attack);
    }


    public class HeavyWeapon : Weapon
    {
        public override string Name => "Heavy " + _name;
        public override string ToString() => Name + EffectToString + DamageToString;

        public HeavyWeapon(int damage, string name, Attributes effects)
            : base(damage, name, effects) { }

        public override void Accept(IAttackType attack, Player player, Enemy enemy, int Damage = 0)
        {
            attack.Visit(this, player, enemy, Damage);
        }

        public override int GetDefense(Player player, IAttackType attack)
        {
            return attack.Defense(this, player);
        }
    }

    public class LightWeapon : Weapon
    {
        public override string Name => "Light " + _name;
        public override string ToString() => Name + EffectToString + DamageToString;
        public LightWeapon(int damage, string name, Attributes effects)
        : base(damage, name, effects) { }

        public override void Accept(IAttackType attack, Player player, Enemy enemy, int Damage = 0)
        {
            attack.Visit(this, player, enemy, Damage);
        }

        public override int GetDefense(Player player, IAttackType attack)
        {
            return attack.Defense(this, player);
        }
    }

    public class MagicWeapon : Weapon
    {
        public override string Name => "Magic " + _name;
        public override string ToString() => Name + EffectToString + DamageToString;
        public MagicWeapon(int damage, string name, Attributes effects)
            : base(damage, name, effects) { }

        public override void Accept(IAttackType attack, Player player, Enemy enemy, int Damage = 0)
        {
            attack.Visit(this, player, enemy, Damage);
        }

        public override int GetDefense(Player player, IAttackType attack)
        {
            return attack.Defense(this, player);
        }
    }

}
