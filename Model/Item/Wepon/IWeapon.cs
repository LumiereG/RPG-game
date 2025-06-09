using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game2
{
    public interface IWeapon : IEquipable
    {
        int Damage { get; }

        public string EffectToString { get; }
    }
}
