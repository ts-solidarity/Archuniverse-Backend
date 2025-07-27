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
    public class Skill : Base
    {
        public string Name { get; private set; }
        public string Description { get; private set; }
        public int RequiredLevel { get; private set; } = 1;
        public List<Skill> Prerequisites { get; private set; }
        public bool IsUnlocked { get; private set; } = false;

        public Action<LivingEntity> ApplyEffect { get; }

        public Skill(string name, string description, int requiredLevel, Action<LivingEntity> applyEffect)
        {
            Name = name;
            Description = description;
            RequiredLevel = requiredLevel;
            ApplyEffect = applyEffect;
        }

        public void AddPrerequisite(Skill skill)
        {
            Prerequisites.Add(skill);
        }

        public bool CanUnlock(LivingEntity entity)
        {
            return !IsUnlocked &&
                   entity.Level >= RequiredLevel &&
                   Prerequisites.All(p => p.IsUnlocked);
        }

        public bool Unlock(LivingEntity entity)
        {
            if (!CanUnlock(entity)) 
                return false;

            IsUnlocked = true;
            ApplyEffect(entity); // Immediately apply effects
            return true;
        }
    }

}
