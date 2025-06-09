using Game2.Command;
using Game2.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using static Game2.Constants;

namespace Game2
{
    public interface IKeyActions
    {
        PlayerAction? Handle(ConsoleKey keyo, int PlayerId);
        IKeyActions SetNext(IKeyActions next);
    }

    public class DrinkPotionKeyHandler : IKeyActions
    {
        public IKeyActions _next;
        public PlayerAction? Handle(ConsoleKey key, int PlayerId)
        {
            if (key == ConsoleKey.P)
            {
                return new PlayerAction() { Type = "Drink Potion", PlayerId = PlayerId };
            }
            return _next.Handle(key, PlayerId);
        }
        public IKeyActions SetNext(IKeyActions next)
        {
            _next = next;
            return next;
        }
    }
    public class AttackKeyHandler : IKeyActions
    {
        public IKeyActions _next;
        public PlayerAction? Handle(ConsoleKey key, int PlayerId)
        {
            switch (key)
            {
                case ConsoleKey.N:
                    return new PlayerAction() { Type = "Attack", Payload = "Normal Attack", PlayerId = PlayerId };
                case ConsoleKey.B:
                    return new PlayerAction() { Type = "Attack", Payload = "Sneaky Attack", PlayerId = PlayerId };
                case ConsoleKey.M:
                    return new PlayerAction() { Type = "Attack", Payload = "Magic Attack", PlayerId = PlayerId };
                default:
                    return _next.Handle(key, PlayerId);
            }
        }
        public IKeyActions SetNext(IKeyActions next)
        {
            _next = next;
            return next;
        }
    }

    public class DropActionKeyHandler : IKeyActions
    {
        private IKeyActions _next;

        public PlayerAction? Handle(ConsoleKey key, int PlayerId)
        {
            if (key == ConsoleKey.C)
            {
                return new PlayerAction() { Type = "Drop an item", PlayerId = PlayerId };
            }
            return _next.Handle(key, PlayerId);
        }
        public IKeyActions SetNext(IKeyActions next)
        {
            _next = next;
            return next;
        }
    }

    public class DropAllItemsKeyHandler : IKeyActions
    {
        private IKeyActions _next;

        public PlayerAction? Handle(ConsoleKey key, int PlayerId)
        {
            if (key == ConsoleKey.F)
            {
                return new PlayerAction() { Type = "Drop all Items", PlayerId = PlayerId };
            }
            return _next.Handle(key, PlayerId);
        }
        public IKeyActions SetNext(IKeyActions next)
        {
            _next = next;
            return next;
        }
    }

    public class EquipKeyHandler : IKeyActions
    {
        private IKeyActions? _next;

        public PlayerAction? Handle(ConsoleKey key, int PlayerId)
        {
            if (key == ConsoleKey.L)
            {
                return new PlayerAction() { Type = "Equip an item", Payload = "Left", PlayerId = PlayerId };
            }
            if (key == ConsoleKey.R)
            {
                return new PlayerAction() { Type = "Equip an item", Payload = "Right", PlayerId = PlayerId };
            }
            return _next.Handle(key, PlayerId);
        }
        public IKeyActions SetNext(IKeyActions next)
        {
            _next = next;
            return next;
        }

    }

    public class ExitGameKeyActionHandler : IKeyActions
    {
        private IKeyActions _next;

        public PlayerAction? Handle(ConsoleKey key, int PlayerId)
        {
            if (key == ConsoleKey.Q)
            {
                new PlayerAction() { Type = "Exit Game", PlayerId = PlayerId };
            }
            return _next.Handle(key, PlayerId);
        }

        public IKeyActions SetNext(IKeyActions next)
        {
            _next = next;
            return next;
        }
    }
    public class InvalidKeyHandler : IKeyActions
    {
        private IKeyActions _next;

        public PlayerAction? Handle(ConsoleKey key, int PlayerId)
        {
            return null;
        }

        public IKeyActions SetNext(IKeyActions next)
        {
            _next = next;
            return next;
        }
    }

    public class NavigateInventoryKeyHandler : IKeyActions
    {
        private IKeyActions _next;

        public PlayerAction? Handle(ConsoleKey key, int PlayerId)
        {
            if (key == ConsoleKey.UpArrow) return new PlayerAction() { Type = "Navigate Inventory", Payload = "Up", PlayerId = PlayerId };
            if (key == ConsoleKey.DownArrow) return new PlayerAction() { Type = "Navigate Inventory", Payload = "Down", PlayerId = PlayerId };
            return _next.Handle(key, PlayerId);
        }
        public IKeyActions SetNext(IKeyActions next)
        {
            _next = next;
            return next;
        }
    }

    public class MoveKeyHandler : IKeyActions
    {
        private IKeyActions _next;

        public PlayerAction? Handle(ConsoleKey key, int PlayerId)
        {
            if (key == ConsoleKey.W) return new PlayerAction() { Type = "Move", Payload = "Up", PlayerId = PlayerId };
            if (key == ConsoleKey.S) return new PlayerAction() { Type = "Move", Payload = "Down", PlayerId = PlayerId };
            if (key == ConsoleKey.A) return new PlayerAction() { Type = "Move", Payload = "Left", PlayerId = PlayerId };
            if (key == ConsoleKey.D) return new PlayerAction() { Type = "Move", Payload = "Right", PlayerId = PlayerId };
            return _next.Handle(key, PlayerId);
        }
        public IKeyActions SetNext(IKeyActions next)
        {
            _next = next;
            return next;
        }
    }

    public class PickupKeyHandler : IKeyActions
    {
        private IKeyActions _next;
        public PlayerAction? Handle(ConsoleKey key, int PlayerId)
        {
            if (key == ConsoleKey.E) return new PlayerAction() { Type = "Pick Up an item", PlayerId = PlayerId };
            return _next.Handle(key, PlayerId);
        }

        public IKeyActions SetNext(IKeyActions next)
        {
            _next = next;
            return next;
        }

    }


}
