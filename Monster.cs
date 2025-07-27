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

namespace Archuniverse.Characters
{
    public class Monster : LivingEntity
    {
        public Monster(string name, int health, int mana, int stamina, int xp, 
            int level, double speed, int baseAttack, int baseDefence,
            int maxHealth = 100, int maxMana = 100, int maxStamina = 100) 
            : base(name, health, mana, stamina, xp, level, speed, baseAttack, baseDefence,
                  maxHealth, maxMana, maxStamina)
        {

        }



    }
}
