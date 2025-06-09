using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Numerics;
using Game2.Observer;
using System.Xml.Linq;

namespace Game2
{
    public class Player
    {
        public Point position;

        public Attributes attributes;

        public List<IEquipable> items;

        public IEquipable[] Hands;

        public List<Eliksir> eliksirs = new List<Eliksir>();
        public int PlecakCounter => items.Count;

        public int Coins = 0;

        public int Gold = 0;

        public int CurrentChoosenItem = -1;

        public int HP = 100;

        public bool isDead = false;

        public string LastAction = "";

        public int ID;

        public Player(Point position, int iD)
        {
            this.position = position;
            attributes = new Attributes { Power = 10, Agression = 10, Dexterity = 10, Health = 10, Luck = 20, Wisdom = 10 };
            items = new List<IEquipable>();
            Hands = new IEquipable[2];
            ID = iD;
        }

        public bool Move (Constants.Direction? direction, Maze maze)
        {
            bool moved = false;
            switch (direction)
            {
                case Constants.Direction.Left:
                    if (position.X == 0 || maze.MazeBuffer[position.X - 1, position.Y] == Constants.Wall || maze.GetEnemyfromPosition(position.X - 1, position.Y) != null) return false;
                    else
                    {
                        position.X -= 1;
                        moved = true;
                    }
                    break;
                case Constants.Direction.Right:
                    if (position.X == Constants.MapWidth - 1 || maze.MazeBuffer[position.X + 1, position.Y] == Constants.Wall || maze.GetEnemyfromPosition(position.X + 1, position.Y) != null) return false;
                    else
                    {
                        position.X += 1;
                        moved = true;
                    }
                    break;
                case Constants.Direction.Up:
                    if (position.Y == 0 || maze.MazeBuffer[position.X, position.Y - 1] == Constants.Wall || maze.GetEnemyfromPosition(position.X, position.Y - 1) != null) return false;
                    else{
                        position.Y -= 1;
                        moved = true;
                    }
                    break;
                case Constants.Direction.Down:
                    if (position.Y == Constants.MapHeight - 1 || maze.MazeBuffer[position.X, position.Y + 1] == Constants.Wall || maze.GetEnemyfromPosition(position.X, position.Y + 1) != null) return false;
                    else
                    {
                        position.Y += 1;
                        moved = true;
                    }
                    break;
            }
            return moved;
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

        public void DrinkPotion(ISubject subject)
        {
            if (items.Count > 0)
            {
                if (items[CurrentChoosenItem].TryToUse(this, subject)) ChangeChoosenItem();
            }
        }

        public void Attack(Enemy enemy, IAttackType attack)
        {
            if (Hands[0] != null && Hands[0].isTwoHanded) Hands[0].Accept(attack, this, enemy);
            else
            {
                if (Hands[0] != null) Hands[0].Accept(attack, this, enemy);
                if (Hands[1] != null) Hands[1].Accept(attack, this, enemy);
            }
        }

        public int CalculateDefense(IAttackType attack)
        {
            int defense = 0;
            if (Hands[0] != null && Hands[0].isTwoHanded) defense += Hands[0].GetDefense(this, attack);
            else
            {
                if (Hands[0] != null) defense += Hands[0].GetDefense(this, attack);
                if (Hands[1] != null) defense += Hands[1].GetDefense(this, attack);
            }
            return defense;
        }

        //public void ReceiveDamage(int damage)
        //{
        //    int actualDamage = Math.Max(0, damage);
        //    HP -= actualDamage;
        //}

    }
}
