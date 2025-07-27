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
    public class Weapon : Item
    {

        public int AdditionalAttack { get; set; } = 0;
        public int AdditionalDefence { get; set; } = 0;


        public Weapon(string name, Grade grade, int worth, int attackValue, int defenceValue) 
            : base(name, Type.Weapon, grade, worth, attackValue, defenceValue)
        {
            
        }

        public override void Equip()
        {
            if (Owner != null)
            {
                if (!Owner.Inventory.Contains(this))
                    return;


                if (Owner.EquippedWeapon != null)
                {
                    Owner.Inventory.Add(Owner.EquippedWeapon);
                }

                Owner.EquippedWeapon = this;
                Owner.Inventory.Remove(this);
                Equipped = true;
                ApplySpecialEffects();
            }
        }

        public override void Unequip()
        {
            if (Owner != null && Equipped)
            {
                if (Owner.EquippedWeapon != this)
                    return;

                Owner.EquippedWeapon = null;
                Owner.Inventory.Add(this);
                Equipped = false;
                RevertSpecialEffects();
            }
        }

        public override int CalculateTotalAttack()
        {
            return base.CalculateTotalAttack() + AdditionalAttack;
        }
        public override int CalculateTotalDefence()
        {
            return base.CalculateTotalDefence() + AdditionalDefence;
        }
    }
}
