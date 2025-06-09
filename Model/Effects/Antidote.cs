using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game2.Observer
{
    internal class Antidote : Eliksir
    {
        public override string Description => "Antidotum";

        public override string Name => "Antidotum";

        public override bool TryToUse(Player player, ISubject subject)
        {
            foreach (var eliksir in player.eliksirs.ToList())
            {
                eliksir.Expire(subject);
            }
            player.items.RemoveAt(player.CurrentChoosenItem);
            return true;
        }
        public override void Apply(Player player)
        {
        }

        public override void Expire(ISubject subject)
        {
        }

        public override void Update(ISubject subject)
        {
        }
    }
}
