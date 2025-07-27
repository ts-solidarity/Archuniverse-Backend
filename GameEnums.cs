using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Archuniverse.Characters;
using Archuniverse.Combat;
using Archuniverse.Items;
using Archuniverse.Utilities;
using Archuniverse;

namespace Archuniverse
{
    public enum Result
    {
        Success,                // Process completed successfuly
        Cancelled,              // Unsuitable action
        InventoryFull,          // Inventory is at max capacity
        ItemAlreadyOwned,       // Item is alredy owned (Same instance)
        ItemNotInInventory,     // Item is not in inventory
        ItemNotOwned,           // Item owner is different
        InsufficientGold,       // Not enough gold
        InsufficientLevel,      // Not enough level to take some action
        InsufficientMana,       // Not enough mana to perform
        InsufficientStamina,    // Not enough stamina to perform
    }

    public enum FightType
    {
        Natural, Magic, Melee, CannotAttack, CannotDefend
    }
}
