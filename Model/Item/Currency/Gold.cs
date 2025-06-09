using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game2
{
    public class Gold : ICurrency
    {
        public string Name => "Gold";
        public char GetSymbol() => '$';

        public void PickMeUp(Player player)
        {
            player.Gold++;
        }
        public override string ToString() => Name;
    }
}
