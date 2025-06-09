using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game2.Observer
{
    public abstract class Eliksir : IEquipable, IPotionObserver
    {
        public int Duration { get; private set; }
        public abstract string Description { get; }

        public abstract void Apply(Player player);

        public virtual bool isTwoHanded => false;

        public abstract string Name { get; }

        public char GetSymbol() => 'P';
        public abstract void Expire(ISubject subject);

        public virtual void PickMeUp(Player player)
        {
            player.items.Add(this);
        }

        public abstract void Update(ISubject subject);

        public void GiveEffects(Player player, bool add)
        {
            if (add) player.attributes += Effects;
            else player.attributes -= Effects;
        }
        public override string ToString() => Name + Effects.FormatAttributes();

        public virtual bool TryToUse(Player player, ISubject subject)
        {
            subject.Attach(this);
            player.eliksirs.Add(this);
            player.items.RemoveAt(player.CurrentChoosenItem);
            Apply(player);
            return true;
        }



        Attributes Effects { get; } = new Attributes { Luck = 5 };
    }
}
