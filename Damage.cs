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
    public class Damage : Base, ITickable
    {
        public FightType Type { get; set; }
        public int DamageAmount { get; set; }
        public LivingEntity DamageTaker { get; set; }
        public bool Bleeding { get; set; } = false;
        public float BleedingTime { get; set; } = 0.0f;
        private float _elapsedTime = 0f;
        private float _damageTime = 0f;


        public Damage(FightType damageType, int damageAmount, LivingEntity damageTaker)
        {
            Type = damageType;
            DamageAmount = damageAmount;
            DamageTaker = damageTaker;
            Bleeding = RandomProvider.NextBool();

            if (Bleeding)
            {
                BleedingTime = RandomProvider.NextFloat(1.0f, 5.0f);
                GameLoop.Instance.RegisterTickable(this);
            }

            DamageTaker.Health -= damageAmount;

        }

        public void Tick(float deltaTime)
        {
            if (!Bleeding)
                return;

            _elapsedTime += deltaTime;
            _damageTime += deltaTime;

            if (_elapsedTime >= BleedingTime)
            {
                _elapsedTime = 0f;
                Bleeding = false;
                GameLoop.Instance.UnregisterTickable(this);
            }

            else
            {
                if (_damageTime >= 1.0f)
                {
                    DamageTaker.Health -= 1;
                    _damageTime -= 1.0f;
                }
            }

        }


    }
}
