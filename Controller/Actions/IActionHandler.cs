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
    public interface IActionHandler
    {
        Command.ICommand? Handle(PlayerActionContext context);
        IActionHandler SetNext(IActionHandler next);
    }

    public class DrinkPotionHandler : IActionHandler
    {
        public IActionHandler _next;
        public Command.ICommand? Handle(PlayerActionContext context)
        {
            if (context.ActionType == "Drink Potion")
            {
                return new Command.DrinkPotionCommand { PlayerId = context.PlayerId };
            }
            return _next?.Handle(context);
        }
        public IActionHandler SetNext(IActionHandler next)
        {
            _next = next;
            return next;
        }
    }
    public class AttackHandler : IActionHandler
    {
        public IActionHandler _next;
        public Command.ICommand? Handle(PlayerActionContext context)
        {

            switch (context.Direction)
            {
                case "Normal Attack":
                    return new Command.AttackCommand { PlayerId = context.PlayerId, attackType = new NormalAttack() };
                case "Sneaky Attack":
                    return new Command.AttackCommand { PlayerId = context.PlayerId, attackType = new SneakyAttack() };
                case "Magic Attack":
                    return new Command.AttackCommand { PlayerId = context.PlayerId, attackType = new MagicAttack() };
                default:
                    return _next?.Handle(context);
            }
        }
        public IActionHandler SetNext(IActionHandler next)
        {
            _next = next;
            return next;
        }
    }

    public class DropActionHandler : IActionHandler
    {
        private IActionHandler _next;

        public Command.ICommand? Handle(PlayerActionContext context)
        {
            if (context.ActionType == "Drop an item")
            {
                return new Command.DropCommand { PlayerId = context.PlayerId };
            }
            return _next?.Handle(context);
        }
        public IActionHandler SetNext(IActionHandler next)
        {
            _next = next;
            return next;
        }
    }

    public class DropAllItemsHandler : IActionHandler
    {
        private IActionHandler _next;

        public Command.ICommand? Handle(PlayerActionContext context)
        {
            if (context.ActionType == "Drop all Items")
            {
                return new Command.DropAllCommand { PlayerId = context.PlayerId };
            }
            return _next?.Handle(context);
        }
        public IActionHandler SetNext(IActionHandler next)
        {
            _next = next;
            return next;
        }
    }

    public class EquipActionHandler : IActionHandler
    {
        private IActionHandler? _next;

        public Command.ICommand? Handle(PlayerActionContext context)
        {
            if (context.ActionType == "Equip an item" && context.Direction == "Left")
            {
                return new Command.EquipCommand { PlayerId = context.PlayerId, Hand = "left" };
            }
            if (context.ActionType == "Equip an item" && context.Direction == "Right")
            {
                return new Command.EquipCommand { PlayerId = context.PlayerId, Hand = "right" };
            }
            return _next?.Handle(context);
        }
        public IActionHandler SetNext(IActionHandler next)
        {
            _next = next;
            return next;
        }

    }

    public class ExitGameActionHandler : IActionHandler
    {
        private IActionHandler _next;

        public Command.ICommand? Handle(PlayerActionContext context)
        {
            if (context.ActionType == "Exit Game")
            {
                return new Command.ExitGamenCommand
                {
                    PlayerId = context.PlayerId
                };
            }

            return _next?.Handle(context);
        }

        public IActionHandler SetNext(IActionHandler next)
        {
            _next = next;
            return next;
        }
    }
    public class InvalidKeyActionHandler : IActionHandler
    {
        private IActionHandler _next;

        public Command.ICommand? Handle(PlayerActionContext context)
        {
            return new Command.InvalidKeyActionCommand
            {
                PlayerId = context.PlayerId
            };
        }

        public IActionHandler SetNext(IActionHandler next)
        {
            _next = next;
            return next;
        }
    }

    public class NavigateInventoryHandler : IActionHandler
    {
        private IActionHandler _next;

        public Command.ICommand? Handle(PlayerActionContext context)
        {
            if (context.ActionType == "Navigate Inventory")
            {
                return new Command.NavigateInventoryCommand { PlayerId = context.PlayerId, InventoryAction = context.Direction };
            }
            return _next?.Handle(context);
        }
        public IActionHandler SetNext(IActionHandler next)
        {
            _next = next;
            return next;
        }
    }

    public class MoveActionHandler : IActionHandler
    {
        private IActionHandler _next;

        public Command.ICommand? Handle(PlayerActionContext context)
        {
            if (context.ActionType == "Move")
            {
                return new Command.MoveCommand{ PlayerId = context.PlayerId, direction = context.Direction };
            }
            return _next?.Handle(context);
        }
        public IActionHandler SetNext(IActionHandler next)
        {
            _next = next;
            return next;
        }
    }

    public class PickupActionHandler : IActionHandler
    {
        private IActionHandler _next;
        public Command.ICommand? Handle(PlayerActionContext context)
        {
            if (context.ActionType == "Pick Up an item")
            {
                return new Command.PickUpCommand { PlayerId = context.PlayerId };
            }

            return _next?.Handle(context);
        }

        public IActionHandler SetNext(IActionHandler next)
        {
            _next = next;
            return next;
        }

    }


}
