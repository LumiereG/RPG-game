using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Game2.Observer
{
    public class PowerEffect : Eliksir
    {
        public override string Name => "Eliksir Mocy";
        private int duration = 5;
        private int bonus = 2;
        Player player;
        public override string Description  => Name + $"bonus {bonus} for {duration} tours";
        public override void Apply(Player player)
        {
            this.player = player;
            player.attributes += bonus;
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
            player.attributes -= bonus;
            player.eliksirs.Remove(this);
            subject.Detach(this);
        }
    }
}
