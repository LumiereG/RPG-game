﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game2.Decorator
{
    public class BurningDecorator : BaseDecorator
    {
        public override string Name => "Burning " + item.Name;

        public BurningDecorator(IEquipable item) : base(item)
        {
        }

        public override char GetSymbol() => 'B';

        public override void GiveEffects(Player player, bool add)
        {
            if (add) player.attributes += Effects;
            else player.attributes -= Effects;
            item.GiveEffects(player, add);
        }

        public Attributes Effects { get; } = new Attributes { Agression = 15 };

        public override string ToString() => "Burning " + item.ToString() + Effects.FormatAttributes();
    }
}
