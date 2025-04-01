using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Numerics;

namespace Game2
{
    public class Player
    {
        public Point position;

        public Attributes attributes;

        public List<IEquipable> items;

        public IEquipable[] Hands;

        public List<IPotion> potions;

        public int PlecakCounter => items.Count;

        public int Coins = 0;

        public int Gold = 0;

        public int CurrentChoosenItem = -1;

        public Player(Point position)
        {
            this.position = position;
            attributes = new Attributes { Power = 10, Agression = 10, Dexterity = 10, Health = 10, Luck = 0, Wisdom = 10 };
            items = new List<IEquipable>();
            Hands = new IEquipable[2];
            potions = new List<IPotion>();
        }

        public bool Move (Constants.Direction? direction, Maze maze)
        {
            switch (direction)
            {
                case Constants.Direction.Left:
                    if (position.X == 0 || maze.MazeBuffer[position.X - 1, position.Y] == Constants.Wall || maze.Enemies.ContainsKey((position.X - 1, position.Y))) return false;
                    else
                    {
                        position.X -= 1;
                        return true;
                    }
                case Constants.Direction.Right:
                    if (position.X == Constants.MapWidth - 1 || maze.MazeBuffer[position.X + 1, position.Y] == Constants.Wall || maze.Enemies.ContainsKey((position.X + 1, position.Y))) return false;
                    else
                    {
                        position.X += 1;
                        return true;
                    }
                case Constants.Direction.Up:
                    if (position.Y == 0 || maze.MazeBuffer[position.X, position.Y - 1] == Constants.Wall || maze.Enemies.ContainsKey((position.X, position.Y - 1))) return false;
                    else{
                        position.Y -= 1;
                        return true;
                    }
                case Constants.Direction.Down:
                    if (position.Y == Constants.MapHeight - 1 || maze.MazeBuffer[position.X, position.Y + 1] == Constants.Wall || maze.Enemies.ContainsKey((position.X, position.Y + 1))) return false;
                    else
                    {
                        position.Y += 1;
                        return true;
                    }
            }
            return false;
        }

        public void PickUp(IItem item)
        {
            item.PickMeUp(this);
            ChangeChoosenItem();
        }

        public IItem Drop()
        {
            if (items.Count > 0)
            {
                IEquipable firstItem = items[CurrentChoosenItem];
                items.RemoveAt(CurrentChoosenItem);
                ChangeChoosenItem();
                return firstItem;
            }
            return null;

        }


        public (bool, IEquipable?) Equip(int hand)
        {
            bool isEquip = true;
            IEquipable? equip = null;
            if (CurrentChoosenItem > items.Count) return (isEquip, null);
            if (Hands[hand] != null)
            {
                equip = Hands[hand];
                equip.GiveEffects(this, false);
                items.Add(equip);
                if (Hands[hand].isTwoHanded) Hands[0] = Hands[1] = null;
                else Hands[hand] = null;
                isEquip = false;
            }
            else
            {
                if (CurrentChoosenItem == -1) return (isEquip, null);
                equip = items[CurrentChoosenItem];
                if (equip.isTwoHanded && (Hands[0] != null || Hands[1] != null)) return (isEquip, null);
                equip.GiveEffects(this, true);
                items.RemoveAt(CurrentChoosenItem);
                if (equip.isTwoHanded) Hands[0] = Hands[1] = equip;
                else Hands[hand] = equip;
                isEquip = true;
            }
            ChangeChoosenItem();
            return (isEquip, equip);
        }

        public void ChangeChoosenItem()
        {
           if (CurrentChoosenItem == -1 && PlecakCounter > 0) CurrentChoosenItem = 0;
           if (CurrentChoosenItem + 1 > PlecakCounter) CurrentChoosenItem--;
        }

        public void DrinkPotion()
        {
            if (potions.Count > 0)
            {
                IPotion potion = potions.First();
                potions.RemoveAt(0);
                potion.GiveEffect(this);
            }
        }
    }
}
