using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game2.Observer
{
    public class ImmortalityPotion : Eliksir
    {
        public override string Name => "Eliksir Nieśmiertelności";
        private int bonus = 5;
        Player player;

        public override string Description => $"{Name}: na zawsze zwiększa atrybuty o {bonus}.";

        public override void Apply(Player player)
        {
            this.player = player;
            player.attributes += bonus;
        }

        public override void Update(ISubject subject)
        {
            // Nic nie robimy w Update, bo eliksir jest wieczny
        }

        public override void Expire(ISubject subject)
        {
            player.attributes -= bonus;
            player.eliksirs.Remove(this);
            subject.Detach(this);
        }
    }
}
