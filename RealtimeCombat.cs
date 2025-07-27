#nullable enable
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

namespace Archuniverse.Combat
{
    public class RealtimeCombat : Base, ITickable
    {
        public LivingEntity Fighter1 { get; set; }
        public LivingEntity Fighter2 { get; set; }
        public LivingEntity? Winner {  get; set; }
        public LivingEntity? Loser {  get; set; }
        public bool IsFightOver => Fighter1.IsDead || Fighter2.IsDead;
        public List<Damage> Damages { get; set; } = new();

        // Real-time combat properties
        private float _fighter1Cooldown = 0f;
        private float _fighter2Cooldown = 0f;
        private readonly float _baseAttackSpeed = 1.0f; // Base attack every 1 second

        public RealtimeCombat(LivingEntity fighter1, LivingEntity fighter2)
        {
            Fighter1 = fighter1;
            Fighter2 = fighter2;
            GameLoop.Instance.RegisterTickable(this);
        }

        public void Fight(LivingEntity attacker, LivingEntity defender)
        {
            (FightType attackType, int attackAmount) = attacker.CalculateAttack();
            (FightType defenceType, int defenceAmount) = defender.CalculateDefence();

            int attackerVitalNeed = (attackType == FightType.Melee) ? attacker.CalculateStaminaNeedToAttack() : attacker.CalculateManaNeedToAttack();
            int defenderVitalNeed = (defenceType == FightType.Melee) ? defender.CalculateStaminaNeedToDefend() : attacker.CalculateManaNeedToDefend();

            if (attacker.Drain(attackType, attackerVitalNeed) == Result.Success)
            {
                if (defender.Drain(defenceType, defenderVitalNeed) == Result.Success)
                {
                    int damageAmount = Math.Max(0, attackAmount - defenceAmount);
                    Damage damage = new(attackType, damageAmount, defender);
                    Console.WriteLine($"{attacker.Name} hits {defender.Name} for {damageAmount} ({attackType})");
                    Damages.Add(damage);
                }
                else
                {
                    Console.WriteLine($"{defender.Name} couldn't defend!");

                    if (defender is Character character)
                    {
                        defenceAmount = character.EquippedArmor?.CalculateTotalDefence() ?? 0;
                    }

                    int damageAmount = Math.Max(0, attackAmount - defenceAmount);
                    Damage damage = new(attackType, damageAmount, defender);
                    Console.WriteLine($"{attacker.Name} hits {defender.Name} for {damageAmount} ({attackType})");
                    Damages.Add(damage);
                }
            }
        }

        public void Tick(float deltaTime)
        {
            if (IsFightOver)
            {
                GameLoop.Instance.UnregisterTickable(this);

                if (Fighter1.IsDead && Fighter2.IsDead)
                {
                    GameLoop.Instance.UnregisterTickable(Fighter1);
                    GameLoop.Instance.UnregisterTickable(Fighter2);
                }
                else if (Fighter1.IsDead)
                {
                    GameLoop.Instance.UnregisterTickable(Fighter1);
                    Winner = Fighter2;
                    Loser = Fighter1;
                }
                else
                {
                    GameLoop.Instance.UnregisterTickable(Fighter2);
                    Winner = Fighter1;
                    Loser = Fighter2;
                }

                return;
            }

            // Update cooldowns
            _fighter1Cooldown -= deltaTime;
            _fighter2Cooldown -= deltaTime;

            // Fighter1 attacks when ready
            if (_fighter1Cooldown <= 0f)
            {
                Fight(Fighter1, Fighter2);
                _fighter1Cooldown = CalculateAttackCooldown(Fighter1);
            }

            // Fighter2 attacks when ready
            if (_fighter2Cooldown <= 0f)
            {
                Fight(Fighter2, Fighter1);
                _fighter2Cooldown = CalculateAttackCooldown(Fighter2);
            }
        }

        private float CalculateAttackCooldown(LivingEntity character)
        {
            // Speed affects attack frequency
            return _baseAttackSpeed / (float)character.Speed;
        }
    }
}