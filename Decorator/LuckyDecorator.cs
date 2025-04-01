using Game2.Decorator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game2
{
    public class LuckyDecorator : BaseDecorator, IWeapon
    {
        private IWeapon weapon;

        private int AddLucky { get; set; } = 5;
        public int Damage { get => 5 * weapon.Damage; }

        public override string Name => "Lucky " + item.Name ;

        public LuckyDecorator(IWeapon weapon) : base(weapon)
        {
           this.weapon = weapon;
        }

        public override char GetSymbol() => 'L';

        public override void GiveEffects(Player player, bool add)
        {
            if (add) player.attributes += Effects;
            else player.attributes -= Effects;
            item.GiveEffects(player, add);
        }

        public override string ToString() => "Lucky " + item.ToString() + Effects.FormatAttributes();

        Attributes Effects { get; } = new Attributes { Luck = 5 };
    }
}
