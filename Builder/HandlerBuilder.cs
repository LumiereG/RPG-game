using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game2
{
    public class HandlerBuilder : IMazeBuilder
    {
        private IActionHandler _handlerChain;
        private List<IActionHandler> _list = new List<IActionHandler>();

        private bool AddItem = false;

        public IMazeBuilder AddCentralRoom()
        {
            return this;
        }

        public IMazeBuilder AddElixirs()
        {
            _list.Add(new DrinkPotionHandler());
            return this;
        }

        public IMazeBuilder AddEnemies()
        {
            _list.Add(new AttackHandler());
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
            _list.Add(new PickupActionHandler());
            _list.Add(new DropActionHandler());
            _list.Add(new DropActionHandler());
            _list.Add(new DropAllItemsHandler());
            _list.Add(new EquipActionHandler());
            _list.Add(new NavigateInventoryHandler());
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
            _list.Add(new InvalidKeyActionHandler());
            _list.Add(new MoveActionHandler());
            _list.Add(new ExitGameActionHandler());
            return this;
        }

        public IMazeBuilder CreateFilledMaze(int width, int height)
        {
            _list.Add(new InvalidKeyActionHandler());
            _list.Add(new MoveActionHandler());
            _list.Add(new ExitGameActionHandler());
            return this;
        }

        public IMazeBuilder GeneratePaths()
        {
            return this;
        }

        public IActionHandler GetResult()
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
