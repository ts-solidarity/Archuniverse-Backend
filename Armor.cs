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
    public class Armor : Item
    {

        public int AdditionalDefence { get; set; } = 0;

        public Armor(string name, Grade grade, int worth, int defenceValue) 
            : base(name, Type.Armor, grade, worth, 0, defenceValue)
        {

        }

        public override void Equip()
        {
            if (Owner != null)
            {
                if (!Owner.Inventory.Contains(this))
                    return;


                if (Owner.EquippedArmor != null)
                {
                    Owner.Inventory.Add(Owner.EquippedArmor);
                }

                Owner.EquippedArmor = this;
                Owner.Inventory.Remove(this);
                Equipped = true;
                ApplySpecialEffects();
            }
        }

        public override void Unequip()
        {
            if (Owner != null && Equipped)
            {
                if (Owner.EquippedArmor != this)
                    return;

                Owner.EquippedArmor = null;
                Owner.Inventory.Add(this);
                Equipped = false;
                RevertSpecialEffects();
            }
        }

        public override int CalculateTotalDefence() 
        {
            return base.CalculateTotalDefence() + AdditionalDefence; 
        }


    }
}
