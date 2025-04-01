using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game2
{
    public interface IEquipable : IItem
    {
        //void EquipMe(Player player, int IsRightHand);
        //void DisEquipMe(Player player, int IsRightHand);

        bool isTwoHanded { get; }

        public void GiveEffects(Player player, bool add);

    }

}
