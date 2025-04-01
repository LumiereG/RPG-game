using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game2.Decorator
{
    public class TwoHanded : BaseDecorator
    {
        public override string Name => "Two-Handed " + item.Name;

        public override bool isTwoHanded => true;

        public TwoHanded(IEquipable item) : base(item)
        {

        }

        public override char GetSymbol() => 'T';

        public override void GiveEffects(Player player, bool add)
        {
            item.GiveEffects(player, add);
        }
        public override string ToString() => "Two-Handed " + item.ToString();
    }
}
