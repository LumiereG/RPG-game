using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game2.Decorator
{
    public abstract class BaseDecorator : IEquipable
    {

        protected IEquipable item;
        public BaseDecorator(IEquipable item)
        {
            this.item = item;
        }

        public virtual bool isTwoHanded => item.isTwoHanded;

        public abstract string Name { get; }

        public abstract char GetSymbol();

        public abstract void GiveEffects(Player player, bool add);

        public virtual void PickMeUp(Player player)
        {
            player.items.Add(this);
        }
    }
}
