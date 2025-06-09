using Game2.Decorator;
using Game2.Observer;
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

        private static (string name, int HP, int AttackPower, int Armor)[] enemies = new (string, int, int, int)[]
       {
            ("Ghoul", 100, 20, 5),
            ("Shadow Wolf", 70, 30, 20),
            ("Vampire Noble", 200, 50, 10)
       };


        public static IItem GenerateRandomItem()
        {
            int choice = rand.Next(4);
            if (choice < 3)
            {
                var unusableData = unusableItems[rand.Next(unusableItems.Length)];
                IEquipable item = new Unusable(unusableData.name, unusableData.attributes);
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
            IItem item;
            int i = rand.Next(4);
            switch (i)
            {
                case 0:
                    item = new PowerEffect();
                    break;
                case 1:
                    item = new WisdomEffect();
                    break;
                case 2:
                    item = new ImmortalityPotion();
                    break;
                default: 
                    item = new Antidote();
                    break;
            }
            return item;
        }

        public static Enemy GenerateEnemies()
        {
            var enemiesData = enemies[rand.Next(enemies.Length)];
            return new Enemy(enemiesData.name, enemiesData.HP, enemiesData.AttackPower, enemiesData.Armor);
        }


        public static IItem GenerateRandomWeapons()
        {
            IWeapon weapon = GenerateWeapons();
            if (rand.Next(2) == 0) weapon = new LuckyDecorator(weapon);

            int decoratorChoice = rand.Next(3);
            switch (decoratorChoice)
            {
                case 0:
                    weapon = new BurningDecorator(weapon);
                    break;
                case 1:
                    weapon = new TwoHanded(weapon);
                    break;
                case 2:
                    weapon = new TwoHanded(new BurningDecorator(weapon));
                    break;
            }
            return weapon;
        }

        public static IWeapon GenerateWeapons()
        {
            IWeapon weapon;
            int weaponType = rand.Next(3);
            var weaponData = weaponItems[rand.Next(weaponItems.Length)];
            switch (weaponType)
            {
                case 0:
                    weapon = new HeavyWeapon(weaponData.damage, weaponData.name, weaponData.attributes);
                    break;
                case 1:
                    weapon = new LightWeapon(weaponData.damage, weaponData.name, weaponData.attributes);
                    break;
                case 2:
                    weapon = new MagicWeapon(weaponData.damage, weaponData.name, weaponData.attributes);
                    break;
                default:
                    throw new Exception("Invalid weapon type");
            }
            return weapon;
        }
    }
}
