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

        private int AddLucky { get; set; } = 5;
        public override int Damage { get => 5 * item.Damage; }

        public override string Name => "Lucky " + item.Name ;

        public override string EffectToString => item.EffectToString + Effects.FormatAttributes();

        public string DamageToString => $" Damage: {Damage}";

        public LuckyDecorator(IWeapon weapon) : base(weapon)
        {
        }

        public override char GetSymbol() => 'L';

        public override void GiveEffects(Player player, bool add)
        {
            if (add) player.attributes += Effects;
            else player.attributes -= Effects;
            item.GiveEffects(player, add);
        }

        public override string ToString() => Name + EffectToString + DamageToString;

        Attributes Effects { get; } = new Attributes { Luck = 5 };
    }
}
