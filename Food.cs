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

namespace Archuniverse.Items
{
    public class Food : Item
    {
        public Food(string name, Grade grade, int worth, int healthBoost) 
            : base(name, Type.Food, grade, worth)
        {
            HealthBoost = healthBoost;
        }

        public int HealthBoost { get; set; }

        public override void Use()
        {
            if (Owner?.Inventory.Contains(this) == true)
            {
                Owner.Health += HealthBoost;
                Owner.Inventory.Remove(this);
            }
        }

    }
}
