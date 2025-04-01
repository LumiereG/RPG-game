using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Game2
{
    public class Potion : IPotion
    {
        public string Name { get; } = "Potion";

        public Attributes Effects { get; }

        public Potion(string name, Attributes effects)
        {
            Name = name;
            Effects = effects;
        }
        public char GetSymbol() => 'P';

        public void GiveEffect(Player player)
        {
            player.attributes += Effects;
        }

        public void PickMeUp(Player player)
        {
            player.potions.Add(this);
        }

        public override string ToString() => Name;
    }
}
