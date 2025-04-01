using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game2
{
    public class Attributes
    {
        public int Power { get; set; } = 0;
        public int Dexterity { get; set; } = 0;
        public int Health { get; set; } = 0;
        public int Luck { get; set; } = 0;
        public int Agression { get; set; } = 0;
        public int Wisdom { get; set; } = 0;

        public override string ToString()
        {
            return $"Power: {Power}\n" +
                   $"Dexterity: {Dexterity}\n" +
                   $"Health: {Health}\n" +
                   $"Luck: {Luck}\n" +
                   $"Agression: {Agression}\n" +
                   $"Wisdom: {Wisdom}\n";
        }

        public string FormatAttributes()
        {
            var properties = GetType().GetProperties()
                .Select(p => new { p.Name, Value = (int)p.GetValue(this) })
                .Where(a => a.Value != 0)
                .Select(a => $"{a.Name}: {a.Value}")
                .ToList();

            return properties.Any() ? $" ({string.Join(", ", properties)})" : "";
        }

        public static Attributes operator +(Attributes attribute1, Attributes attribute2)
        {
            return new Attributes
            {
                Power = attribute1.Power + attribute2.Power,
                Dexterity = attribute1.Dexterity + attribute2.Dexterity,
                Health = attribute1.Health + attribute2.Health,
                Agression = attribute1.Agression + attribute2.Agression,
                Luck = attribute1.Luck + attribute2.Luck,
                Wisdom = attribute1.Wisdom + attribute2.Wisdom
            };
        }

        public static Attributes operator -(Attributes attribute1, Attributes attribute2)
        {
            return new Attributes
            {
                Power = attribute1.Power - attribute2.Power,
                Dexterity = attribute1.Dexterity - attribute2.Dexterity,
                Health = attribute1.Health - attribute2.Health,
                Agression = attribute1.Agression - attribute2.Agression,
                Luck = attribute1.Luck - attribute2.Luck,
                Wisdom = attribute1.Wisdom - attribute2.Wisdom
            };
        }
    }
}
