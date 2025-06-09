using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game2
{
    public class KeyBuilder : IMazeBuilder
    {
        private IKeyActions _handlerChain;
        private List<IKeyActions> _list = new List<IKeyActions>();

        private bool AddItem = false;

        public IMazeBuilder AddCentralRoom()
        {
            return this;
        }

        public IMazeBuilder AddElixirs()
        {
            _list.Add(new DrinkPotionKeyHandler());
            return this;
        }

        public IMazeBuilder AddEnemies()
        {
            _list.Add(new AttackKeyHandler());
            return this;
        }

        public IMazeBuilder AddItems(int count)
        {
            if (!AddItem)
            {
                AddItemsInstruction();
                AddItem = true;
            }
            return this;
        }

        public void AddItemsInstruction()
        {
            _list.Add(new PickupKeyHandler());
            _list.Add(new DropActionKeyHandler());
            _list.Add(new DropAllItemsKeyHandler());
            _list.Add(new EquipKeyHandler());
            _list.Add(new NavigateInventoryKeyHandler());
        }

        public IMazeBuilder AddModifiedWeapons()
        {
            if (!AddItem)
            {
                AddItemsInstruction();
                AddItem = true;
            }
            return this;
        }

        public IMazeBuilder AddRooms()
        {
            return this;
        }

        public IMazeBuilder AddWeapons()
        {
            if (!AddItem)
            {
                AddItemsInstruction();
                AddItem = true;
            }
            return this;
        }

        public IMazeBuilder CreateEmptyMaze(int width, int height)
        {
            _list.Add(new InvalidKeyHandler());
            _list.Add(new MoveKeyHandler());
            _list.Add(new ExitGameKeyActionHandler());
            return this;
        }

        public IMazeBuilder CreateFilledMaze(int width, int height)
        {
            _list.Add(new InvalidKeyHandler());
            _list.Add(new MoveKeyHandler());
            _list.Add(new ExitGameKeyActionHandler());
            return this;
        }

        public IMazeBuilder GeneratePaths()
        {
            return this;
        }

        public IKeyActions GetResult()
        {
            for (int i = 0; i < _list.Count; i++)
            {
                var handler = _list[i];
                handler.SetNext(_handlerChain);
                _handlerChain = handler;
            }
            return _handlerChain;
        }
    }
}
