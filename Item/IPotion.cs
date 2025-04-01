using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game2
{
    public interface IPotion : IItem
    {
        void GiveEffect(Player player);
    }
}
