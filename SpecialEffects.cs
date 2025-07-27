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
    public class Solidness : ArmorSpecialEffect
    {
        
        public int DefenceBoostPercentage { get; set; } = 10;
        
        
        public Solidness(int defenceBoostPercentage) 
        {
            DefenceBoostPercentage = defenceBoostPercentage;
        }

        public override void Effect(Armor armor)
        {
            armor.AdditionalDefence += armor.DefenceValue * DefenceBoostPercentage / 100;
        }

        public override void Revert(Armor armor)
        {
            armor.AdditionalDefence -= armor.DefenceValue * DefenceBoostPercentage / 100;
        }

    }

    public class Aggressiveness : WeaponSpecialEffect
    {
        public int AttackBoostPercentage { get; set; } = 10;

        public Aggressiveness(int attackBoostPercentage)
        {
            AttackBoostPercentage = attackBoostPercentage;
        }

        public override void Effect(Weapon weapon)
        {
            weapon.AdditionalAttack += weapon.AttackValue * AttackBoostPercentage / 100;
        }
        public override void Revert(Weapon weapon)
        {
            weapon.AdditionalAttack -= weapon.AttackValue * AttackBoostPercentage / 100;
        }
    }

    public class GenderCurse : WareSpecialEffect
    {
        public override void Effect(Ware ware)
        {
            ToggleGender(ware);
        }

        public override void Revert(Ware ware)
        {
            ToggleGender(ware);
        }

        private void ToggleGender(Ware ware)
        {
            if (ware.Owner.Gender == Character.Sex.Male)
            {
                ware.Owner.Gender = Character.Sex.Female;
                return;
            }

            ware.Owner.Gender = Character.Sex.Male;
        }
    }

    public class HealthGenerationBoost : PotionSpecialEffect
    {
        public override void Effect(Potion potion)
        {
            if (potion.Owner != null)
            {
                potion.Owner.HealthRegeneration *= 2;
            }
        }

        public override void Revert(Potion potion)
        {
            if (potion.Owner != null)
            {
                potion.Owner.HealthRegeneration /= 2;
            }
        }
    }
}
