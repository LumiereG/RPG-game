using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game2
{
    public static class Constants
    {
        static public readonly int MapWidth = 43;
        static public readonly int MapHeight = 23;
        static public readonly char Wall = '█';
        static public readonly char Player = '¶';

        static public readonly int MinimumWidth = MapWidth + 100;
        static public readonly int MinimumHeight = MapHeight + 10;

        static public readonly string NormalAttack =
            "Normal Attack:\n" +
            "Heavy Weapon: Power + Agression + Damage\n" +
            "Light Weapon: Dexterity + Luck + Damage\n" +
            "Magic Weapon: 1\n" +
            "Others: 0\n" +
            "Defense:\n" +
            "Heavy Weapon: Power + Luck\n" +
            "Light Weapon: Dexterity + Luck\n" +
            "Magic Weapon: Dexterity + Luck\n" +
            "Others: Dexterity";
        public static readonly string SneakyAttack =
            "Sneaky Attack:\n" +
            "Heavy Weapon: (Power + Agression + Damage) / 2\n" +
            "Light Weapon: 2 * (Dexterity + Luck + Damage)\n" +
            "Magic Weapon: 1\n" +
            "Others: 0\n" +
            "Defense:\n" +
            "Heavy Weapon: Power\n" +
            "Light Weapon: Dexterity\n" +
            "Magic Weapon: 0\n" +
            "Others: 0";
        public static readonly string MagicAttack =
            "Magic Attack:\n" +
            "Heavy Weapon: 1\n" +
            "Light Weapon: 1\n" +
            "Magic Weapon: Wisdom + Damage\n" +
            "Others: 0\n" +
            "Defense:\n" +
            "Heavy Weapon: Luck\n" +
            "Light Weapon: Luck\n" +
            "Magic Weapon: Wisdom * 2\n" +
            "Others: Luck";
        public enum Direction
        {
            Up,
            Down,
            Left,
            Right
        }

    }
}
