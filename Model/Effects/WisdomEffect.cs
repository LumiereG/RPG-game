using Game2.Observer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game2
{
    public class WisdomEffect : Eliksir
    {
        public override string Name => "Eliksir Mądrości";
        private int duration = 7;
        private int bonus = 2;
        Player player;
        public override string Description => Name + $" bonus {bonus} to Wisdom for {duration} turns";

        public override void Apply(Player player)
        {
            this.player = player;
            player.attributes.Wisdom += bonus;
        }

        public override void Update(ISubject subject)
        {
            duration--;
            if (duration <= 0)
            {
                Expire(subject);
            }
        }

        public override void Expire(ISubject subject)
        {
            player.attributes.Wisdom -= bonus;
            player.eliksirs.Remove(this);
            subject.Detach(this);
        }
    }
}
