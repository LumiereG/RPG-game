# RPG Game Documentation

## **Objective**

This project is a simple, console-based RPG game where the player navigates through a dungeon, interacts with items, and faces enemies. The game incorporates various design patterns to manage game objects, item effects, and the dungeon generation system. The project utilizes the **Decorator**, **Singleton**, and **Builder** design patterns to provide flexibility and modularity.

## **Game Features**

1. **Dungeon Layout**:  
   The game world consists of a 20x40 grid representing the dungeon. The grid can have:
   - **Walls (`█`)** blocking movement,
   - **Empty spaces (` `)** where the player can move,
   - **Items** like weapons, potions, and currency (coins/gold),
   - **Enemies** positioned throughout the dungeon.

2. **Player Movement**:  
   The player can move around using **WASD** keys, with restrictions:
   - The player cannot move beyond the dungeon boundaries or through walls.
   - The player starts at position **(0, 0)**.

3. **Player Attributes and Inventory**:  
   The player has several stats, including Strength, Dexterity, Health, Luck, Aggression, and Wisdom.  
   The player has:
   - **Two hands** to equip items (e.g., weapons),
   - An **inventory** where they can store and manage items,
   - **Currencies** like coins and gold.

4. **Items**:  
   The game includes predefined items, such as:
   - **Weapons** with damage values (e.g., sword, bow),
   - **Potions** with various effects,
   - **Currency** like coins and gold.  
   Items are placed within the dungeon, and when the player steps on an item, they can **pick it up** by pressing **E**.  
   Items can have **effects**, such as modifying player stats (e.g., "Unlucky" decreases Luck, "Powerful" increases weapon damage). These effects are implemented using the **Decorator** pattern.

5. **Dungeon Generation**:  
   The dungeon is generated using different strategies, including:
   - **Empty dungeon**: A grid with no walls.
   - **Filled dungeon**: A grid filled entirely with walls.
   - **Random paths and chambers**: Creating paths and random empty spaces.
   - **Central room**: Adding a large central room in the dungeon.
   - **Item placement**: Randomly placing items like weapons, potions, and enemies on non-wall cells.
   
   The **Builder Pattern** is used to allow for flexible and dynamic dungeon construction. Various strategies can be combined to create different types of dungeon layouts.

6. **Game State Presentation**:  
   The game state is rendered on the console and includes:
   - A visual representation of the **dungeon**.
   - The **player’s stats**, including equipped items.
   - **Inventory** showing the items in the player's possession.
   - Information about **nearby items** and **enemies** surrounding the player.
   - **Last action** taken by the player.
   
   The **Singleton Pattern** ensures a single instance for rendering the game state, making it easy to manage and update the display consistently.

7. **Item Effects with Decorators**:  
   Items can be **decorated** with effects that modify their behavior. For example:
   - Weapons can have an effect that increases damage.
   - Potions can modify the player’s attributes (e.g., decrease luck).
   
   These effects are applied dynamically to items using the **Decorator Pattern**. This allows for easy expansion of item functionality in the future.

8. **Inventory Management**:  
   The player can:
   - **Equip and unequip** items in both hands.
   - **Drop** items from the inventory.
   - Manage **two-handed weapons**, ensuring they are handled correctly when equipped or unequipped.

9. **Rendering**:  
   The game screen is updated continuously:
   - The dungeon and player’s actions are redrawn.
   - The player’s current stats and inventory are displayed beside the dungeon.
   - Nearby items and enemies are shown.

## **Design Patterns Used**

1. **Decorator Pattern**:  
   This pattern is used to add effects to items, such as modifying their attributes or adding bonuses. Each item can have multiple effects applied at runtime, and this functionality is crucial for enhancing gameplay with dynamic item behaviors.

2. **Singleton Pattern**:  
   The game rendering system uses the Singleton pattern to ensure there is only one instance responsible for displaying the game state. This centralizes control over the display and ensures consistency in the game’s visual output.

3. **Builder Pattern**:  
   The Builder pattern is utilized to create the dungeon and define its structure. It allows for combining various strategies (e.g., adding walls, paths, items) to generate different types of dungeons, making the system flexible and extensible.

## **How to Play**

1. **Movement**: Use **WASD** keys to move the player.
2. **Pick up Items**: Step on an item and press **E** to pick it up.
3. **Equip and Manage Inventory**: Equip items by selecting them from the inventory. Manage your two hands to equip one-handed and two-handed weapons.
4. **Explore the Dungeon**: Navigate through the dungeon, interact with items, and face enemies.

---

## **Future Features**

- **Combat System**: Add a system where the player fights enemies based on their equipped items and stats.
- **Advanced Dungeon Layouts**: Implement more complex dungeon designs with traps, puzzles, and multi-level layouts.
- **Enhanced Item Effects**: Introduce additional item modifiers that impact combat or player attributes.

