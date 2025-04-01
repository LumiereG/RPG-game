using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game2;
public class InstructionsBuilder : IMazeBuilder
{
    private string _instructions;

    private bool AddItem = false;

    public IMazeBuilder AddCentralRoom()
    {
        return this;
    }

    public IMazeBuilder AddElixirs()
    {
        _instructions +=
         "- Drink the first potion: F\n";
        return this;
    }

    public IMazeBuilder AddEnemies()
    {
        return this;
    }

    public IMazeBuilder AddItems(int count)
    {
        if (!AddItem)
        {
            _instructions +=
            "- Pick up an item: E\n" +
            "- Drop an item from the inventory: C\n" +
            "- Equip or unequip an item: L (Left hand) | R (Right hand)\n" +
            "- Navigate inventory: ↑ (Up) | ↓ (Down)\n";
            AddItem = true;
        }
        return this;
    }

    public IMazeBuilder AddModifiedWeapons()
    {
        if (!AddItem)
        {
            _instructions +=
            "- Pick up an item: E\n" +
            "- Drop an item from the inventory: C\n" +
            "- Equip or unequip an item: L (Left hand) | R (Right hand)\n" +
            "- Navigate inventory: ↑ (Up) | ↓ (Down)\n";
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
            _instructions +=
            "- Pick up an item: E\n" +
            "- Drop an item from the inventory: C\n" +
            "- Equip or unequip an item: L (Left hand) | R (Right hand)\n" +
            "- Navigate inventory: ↑ (Up) | ↓ (Down)\n";
            AddItem = true;
        }
        return this;
    }

    public IMazeBuilder CreateEmptyMaze(int width, int height)
    {
        _instructions =
             "Controls:\n" +
             "- Move the player: W (Up), A (Left), S (Down), D (Right)\n";
        return this;
    }

    public IMazeBuilder CreateFilledMaze(int width, int height)
    {
        _instructions =
             "Controls:\n" +
             "- Move the player: W (Up), A (Left), S (Down), D (Right)\n";
        return this;
    }

    public IMazeBuilder GeneratePaths()
    {
        return this;
    }

    public string GetResult() => _instructions;
}
