using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game2.Decorator
{
    public abstract class BaseDecorator : IWeapon
    {

        protected IWeapon item;
        public BaseDecorator(IWeapon item)
        {
            this.item = item;
        }

        public virtual bool isTwoHanded => item.isTwoHanded;

        public abstract string Name { get; }

        public abstract string EffectToString { get; }

        public virtual int Damage => item.Damage;

        public abstract char GetSymbol();

        public abstract void GiveEffects(Player player, bool add);

        public virtual void PickMeUp(Player player)
        {
            player.items.Add(this);
        }

        public virtual void Accept(IAttackType attack, Player player, Enemy enemy, int Damage = 0)
        {
            item.Accept(attack, player, enemy, this.Damage);
        }

        public virtual int GetDefense(Player player, IAttackType attack)
        {
            return item.GetDefense(player, attack);
        }

        public bool TryToUse(Player player, ISubject subject) => item.TryToUse(player, subject);
    }
}
