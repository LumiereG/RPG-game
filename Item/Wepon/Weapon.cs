using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game2
{
    public class Weapon : IWeapon
    {
        public int Damage { get; }

        public string Name { get; } = "Weapon";

        public void PickMeUp(Player player)
        {
            player.items.Add(this);
        }

        public char GetSymbol() => 'W';

        public override string ToString() => Name + Effects.FormatAttributes();

        public Attributes Effects { get; } = new Attributes{Power = 5};

        public bool isTwoHanded => false;

        public Weapon(int damage, string name, Attributes effects)
        {
            Damage = damage;
            Name = name;
            Effects = effects;
        }

        public void GiveEffects(Player player, bool add)
        {
            if (add) player.attributes += Effects;
            else player.attributes -= Effects;
        }
    }
}
