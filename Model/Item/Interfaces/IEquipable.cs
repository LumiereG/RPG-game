using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game2
{
    public interface IEquipable : IItem
    {
        
        bool isTwoHanded { get; }

        public void GiveEffects(Player player, bool add);

        // public string AttackInfo => Constants.OtherInfo;

        bool TryToUse(Player player, ISubject subject);

        void Accept(IAttackType attack, Player player, Enemy enemy, int Damage = 0)
        {
            attack.Visit(this, player, enemy);
        }
        int GetDefense(Player player, IAttackType attack)
        {
            return attack.Defense(this, player);
        }

    }

}
