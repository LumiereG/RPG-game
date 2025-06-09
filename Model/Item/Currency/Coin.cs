using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game2
{
    public class Coin : ICurrency
    {
        public string Name => "Coin";
        public char GetSymbol() => '$';

        public void PickMeUp(Player player)
        {
            player.Coins++;
        }

        public override string ToString() => Name;
    }
}
