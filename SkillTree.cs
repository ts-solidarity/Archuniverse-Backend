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

public class SkillTree : Base
{
    public enum Ability
    {
        Melee, Magic
    }

    public Dictionary<Ability, int> Abilities { get; set; }

    public LivingEntity Owner { get; }
    public List<Skill> AllSkills { get; set; } = new List<Skill>();
    public int UnusedSkillPoints { get; private set; } = 0;

    public SkillTree(LivingEntity owner)
    {
        Owner = owner;


        Abilities = new Dictionary<Ability, int>
        {
            {Ability.Melee, 1 },
            {Ability.Magic, 1 },
        };


        var strongBody = new Skill("Strong Body", "Increases Max Health by 50", 2,
            entity => entity.MaxHealth += 50);

        var trainedMind = new Skill("Trained Mind", "Increases Max Mana by 40", 2,
            entity => entity.MaxMana += 40);

        var fasterRegen = new Skill("Faster Regen", "Regenerate +2 HP/turn", 3,
            entity => entity.HealthRegeneration += 2);
        var carrier = new Skill("Carrier", "Doubling the max inventory", 5, 
            entity =>
            {
                if (entity is Character character)
                {
                    character.InventoryCapacity *= 2;
                }
            });
        var ironBody = new Skill("Iron Body", "Increases base defence by 30", 2,
    entity => entity.BaseDefence += 30);

        AddSkill(strongBody);
        AddSkill(trainedMind);
        AddSkill(fasterRegen);
        AddSkill(carrier);
        AddSkill(ironBody);
    }

    public void UpdateStats()
    {
        Owner.Melee = Abilities[Ability.Melee] * 5;
        Owner.Magic = Abilities[Ability.Magic] * 5;
    }

    public void AddSkill(Skill skill)
    {
        if (!AllSkills.Contains(skill))
            AllSkills.Add(skill);
    }

    public void GainSkillPoint()
    {
        UnusedSkillPoints++;
    }

    public bool UnlockSkill(string skillName)
    {
        var skill = AllSkills.FirstOrDefault(s => s.Name == skillName);
        if (skill == null || UnusedSkillPoints <= 0 || !skill.CanUnlock(Owner))
            return false;

        if (skill.Unlock(Owner))
        {
            UnusedSkillPoints--;
            return true;
        }

        return false;
    }

    public bool IncreaseAbility(Ability ability, int amount = 1)
    {
        if (UnusedSkillPoints < amount)
            return false;

        for (int i = 0; i < amount; i++)
        {
            Abilities[ability] += 1;
            UnusedSkillPoints -= 1;
        }

        UpdateStats();
        return true;
    }
}
