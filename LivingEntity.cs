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
    public class LivingEntity : Base, ITickable
    {
        public string Name { get; set; }
        public int Health
        {
            get => _health;
            set => _health = Math.Clamp(value, 0, MaxHealth);
        }
        public int Mana
        {
            get => _mana;
            set => _mana = Math.Clamp(value, 0, MaxMana);
        }
        public int Stamina
        {
            get => _stamina;
            set => _stamina = Math.Clamp(value, 0, MaxStamina);
        }
        public int MaxHealth { get; set; } = 100;
        public int MaxMana { get; set; } = 100;
        public int MaxStamina { get; set; } = 100;
        public int HealthRegeneration { get; set; } = 1;
        public int ManaRegeneration { get; set; } = 1;
        public int StaminaRegeneration { get; set; } = 1;

        public int BaseAttack { get; set; } = 10;
        public int BaseDefence { get; set; } = 10;
        public int Melee { get; set; } = 0;
        public int Magic { get; set; } = 0;

        public int Xp { get; set; }
        public int Level { get; set; }
        public double Speed { get; set; } = 1.0;
        public double X { get; set; } = 0.0;
        public double Y { get; set; } = 0.0;
        public double Z { get; set; } = 0.0;

        public bool IsDead => Health <= 0;
        public bool IsAlive => !IsDead;
        public bool IsAtFullHealth => Health == MaxHealth;
        public bool IsWounded => Health < MaxHealth && Health > 0;
        public SkillTree Skills { get; set; }

        private int _health;
        private int _mana;
        private int _stamina;


        public LivingEntity(string name, int health, int mana, int stamina, int xp, 
            int level, double speed, int baseAttack, int baseDefence,
            int maxHealth = 100, int maxMana = 100, int maxStamina = 100)
        {
            Name = name;
            Health = health;
            Mana = mana;
            Stamina = stamina;
            Xp = xp;
            Level = level;
            Speed = speed;
            BaseAttack = baseAttack;
            BaseDefence = baseDefence;

            MaxHealth = maxHealth;
            MaxMana = maxMana;
            MaxStamina = maxStamina;

            Skills = new(this);

            LevelUpBasedOnXp();
            GameLoop.Instance.RegisterTickable(this);
        }

        public virtual Result Drain(FightType damageType, int vitalAmount)
        {
            switch (damageType)
            {
                case FightType.Natural:
                    return Result.Cancelled;

                case FightType.Melee:

                    if (HasEnoughStamina(vitalAmount))
                    {
                        Stamina -= vitalAmount;
                        return Result.Success;
                    }
                    return Result.InsufficientMana;

                case FightType.Magic:
                    
                    if (HasEnoughMana(vitalAmount))
                    {
                        Mana -= vitalAmount;
                        return Result.Success;
                    }
                    return Result.InsufficientMana;

                default:
                    return Result.Cancelled;
            }
        }

        public bool HasEnoughHealth(int amount)
        {
            return (Health >= amount);
        }
        public bool HasEnoughMana(int amount)
        {
            return (Mana >= amount);
        }
        public bool HasEnoughStamina(int amount)
        {
            return (Stamina >= amount);
        }

        public virtual int CalculateMeleeDamage()
        {
            return BaseAttack + Melee;
        }
        public virtual int CalculateMagicDamage()
        {
            return BaseAttack + Magic;
        }

        public virtual int CalculateMeleeDefence()
        {
            return BaseDefence + Melee;
        }
        public virtual int CalculateMagicDefence()
        {
            return BaseDefence + Magic;
        }

        public virtual int CalculateManaNeedToAttack()
        {
            return (int)(15.0f * ((100.0f - Level) / 100.0f));
        }

        public virtual int CalculateStaminaNeedToAttack()
        {
            return (int)(15.0f * ((100.0f - Level) / 100.0f));
        }
        public virtual int CalculateManaNeedToDefend()
        {
            return (int)(7.5f * ((100.0f - Level) / 100.0f));
        }

        public virtual int CalculateStaminaNeedToDefend()
        {
            return (int)(7.5f * ((100.0f - Level) / 100.0f));
        }

        public virtual FightType CalculateOptimalAttackType()
        {
            int meleeAttack = CalculateMeleeDamage();
            int magicAttack = CalculateMagicDamage();

            int staminaNeed = CalculateStaminaNeedToAttack();
            int manaNeed = CalculateManaNeedToAttack();

            bool hasStamina = HasEnoughStamina(staminaNeed);
            bool hasMana = HasEnoughMana(manaNeed);

            if (hasStamina && hasMana)
            {
                if (meleeAttack >= magicAttack)
                    return FightType.Melee;
                return FightType.Magic;
            }
            else if (hasStamina)
                return FightType.Melee;
            else if (hasMana)
                return FightType.Magic;
            else
                return FightType.CannotAttack;
        }

        public virtual FightType CalculateOptimalDefenceType()
        {
            int meleeDefence = CalculateMeleeDefence();
            int magicDefence = CalculateMagicDefence();

            int staminaNeed = CalculateStaminaNeedToDefend();
            int manaNeed = CalculateManaNeedToDefend();

            bool hasStamina = HasEnoughStamina(staminaNeed);
            bool hasMana = HasEnoughMana(manaNeed);

            if (hasStamina && hasMana)
            {
                if (meleeDefence >= magicDefence)
                    return FightType.Melee;
                return FightType.Magic;
            }
            else if (hasStamina)
                return FightType.Melee;
            else if (hasMana)
                return FightType.Magic;
            else
                return FightType.CannotDefend;
        }

        public virtual int CalculateTotalAttack(FightType optimalFightType)
        {
            switch(optimalFightType)
            {
                default:
                    return CalculateMeleeDamage();
                case FightType.Magic:
                    return CalculateMagicDamage();
                case FightType.Melee:
                    return CalculateMeleeDamage();
            }
        }

        public virtual int CalculateTotalDefence(FightType optimalFightType)
        {
            switch (optimalFightType)
            {
                default:
                    return CalculateMeleeDefence();
                case FightType.Magic:
                    return CalculateMagicDefence();
                case FightType.Melee:
                    return CalculateMeleeDefence();
            }
        }

        public virtual (FightType, int) CalculateAttack()
        {
            FightType optimalFightType = CalculateOptimalAttackType();
            int attackAmount = CalculateTotalAttack(optimalFightType);
            return (optimalFightType, attackAmount);
        }

        public virtual (FightType, int) CalculateDefence()
        {
            FightType optimalFightType = CalculateOptimalDefenceType();
            int defenceAmount = CalculateTotalDefence(optimalFightType);
            return (optimalFightType, defenceAmount);
        }

        public virtual void RegenerateHealth()
        {
            Health += HealthRegeneration;
        }
        public virtual void RegenerateMana()
        {
            Mana += ManaRegeneration;
        }
        public virtual void RegenerateStamina()
        {
            Stamina += StaminaRegeneration;
        }
        public virtual void RegenerateVitals()
        {
            RegenerateHealth();
            RegenerateMana();
            RegenerateStamina();
        }

        public void AddXp(int amount)
        {
            Xp += amount;
            LevelUpBasedOnXp();
        }
        public void LevelUpBasedOnXp()
        {
            while (Xp >= XpRequiredForLevel(Level + 1))
            {
                LevelUp();
            }
        }

        public void LevelUp()
        {
            Level++;
            Skills.GainSkillPoint();
        }

        private static int XpRequiredForLevel(int level)
        {
            return 100 * level * level;
        }


        // Time related stuff here
        public void Tick(float deltaTime)
        {
            _regenTimer += deltaTime;
            if (_regenTimer >= 1.0f)
            {
                RegenerateVitals();
                _regenTimer = 0f;
            }

            // tick the skill tree if needed in future
        }

        private float _regenTimer = 0f;
    }
}
