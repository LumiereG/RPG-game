using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game2
{
    public interface IItem : IPickable
    {
        string Name { get; }
        char GetSymbol();
    }
}
