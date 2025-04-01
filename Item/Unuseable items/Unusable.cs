using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game2
{
    public class Unusable : IUnuseable
    {
        public string Name { get; } = "Unusable Item";

        public char GetSymbol() => 'N';

        public void PickMeUp(Player player)
        {
            player.items.Add(this);
        }

        public Unusable(string name, Attributes effects)
        {
            Name = name;
            Effects = effects;
        }   

        public override string ToString() => Name + Effects.FormatAttributes();

        public void GiveEffects(Player player, bool add)
        {
            if (add) player.attributes += Effects;
            else player.attributes -= Effects;
        }

        public Attributes Effects { get; } = new Attributes { Health = 5 };

        public bool isTwoHanded => false;
    }
}
