using Game2.Decorator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game2
{
    public static class ItemFactory
    {
        private static Random rand = new Random();

        private static (string name, Attributes attributes, int damage)[] weaponItems = new (string, Attributes, int)[]
        {
            ("Sword", new Attributes { Power = 10 }, 10),
            ("Gun", new Attributes { Luck = -10 }, 100)
        };

        private static (string name, Attributes attributes)[] unusableItems = new (string, Attributes)[]
        {
            ("Laptop", new Attributes { Wisdom = 20 }),
            ("Food", new Attributes { Power = 5, Agression = 5 }),
            ("Old Book", new Attributes { Wisdom = 10, Dexterity = 5 })
        };

        private static (string name, Attributes attributes)[] potions = new (string, Attributes)[]
        {
            ("Coffee", new Attributes { Power = 50 }),
            ("Wisdom potion", new Attributes { Wisdom = 5, Agression = 5 }),
            ("Lucky potion", new Attributes { Wisdom = 10, Luck = 20 })
        };

        public static IItem GenerateRandomItem()
        {
            int choice = rand.Next(4);
            if (choice < 3)
            {
                var unusableData = unusableItems[rand.Next(unusableItems.Length)];
                IEquipable item = new Unusable(unusableData.name, unusableData.attributes);

                int i = rand.Next(5);
                if (i == 0) item = new BurningDecorator(item);
                if (i == 1) item = new TwoHanded(item);
                if (i == 2) item = new TwoHanded(new BurningDecorator(item));
                return item;
            }
            else
            {
                IItem item = rand.Next(2) == 0 ? new Coin() : new Gold();
                return item;
            }
        }

        public static IItem GenerateRandomPotions()
        {
            var potionData = potions[rand.Next(unusableItems.Length)];
            IItem item = new Potion(potionData.name, potionData.attributes);
            return item;
        }

        public static IItem GenerateRandomWeapons()
        {

            IEquipable item;

            var weaponData = weaponItems[rand.Next(weaponItems.Length)];
            IWeapon weapon = new Weapon(weaponData.damage, weaponData.name, weaponData.attributes);
            int decoratorChoice = rand.Next(3);
            if (decoratorChoice == 0) item = new LuckyDecorator(weapon);
            else item = weapon;
            int i = rand.Next(3);
            if (i == 0) item = new BurningDecorator(item);
            if (i == 1) item = new TwoHanded(item);
            if (i == 2) item = new TwoHanded(new BurningDecorator(item));
            return item;
        }

        public static IItem GenerateWeapons()
        { 
            var weaponData = weaponItems[rand.Next(weaponItems.Length)];
            Weapon weapon = new Weapon(weaponData.damage, weaponData.name, weaponData.attributes);
            return weapon;
        }
    }
}
