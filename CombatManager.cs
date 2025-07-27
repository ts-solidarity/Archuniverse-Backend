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
    /// <summary>
    /// Manages all real-time combat encounters. Handles creation, tracking, and cleanup.
    /// </summary>
    public class CombatManager : Base, ITickable
    {
        // Singleton instance
        private static CombatManager? _instance;
        public static CombatManager Instance => _instance ??= new CombatManager();

        private CombatManager()
        {
            GameLoop.Instance.RegisterTickable(this);
        }

        public List<RealtimeCombat> ActiveCombats { get; } = new();
        public List<RealtimeCombat> CompletedCombats { get; } = new();

        private float _elapsedTime = 0f;

        /// <summary>
        /// Starts a new combat encounter between two characters.
        /// </summary>
        public RealtimeCombat NewCombat(Character attacker, Character defender)
        {
            var combat = new RealtimeCombat(attacker, defender);
            ActiveCombats.Add(combat);
            return combat;
        }

        /// <summary>
        /// Periodically checks active combats and finalizes those that are over.
        /// </summary>
        private void Update()
        {
            foreach (var combat in ActiveCombats.ToList())
            {
                if (combat.IsFightOver)
                {
                    // XP reward logic
                    if (combat.Winner is Character winner && combat.Loser is Character loser)
                    {
                        winner.AddXp(200 * loser.Level); // Simple XP formula
                    }

                    ActiveCombats.Remove(combat);
                    CompletedCombats.Add(combat);
                }
            }
        }

        /// <summary>
        /// Called every frame with deltaTime from GameLoop.
        /// Handles periodic update calls.
        /// </summary>
        public void Tick(float deltaTime)
        {
            _elapsedTime += deltaTime;

            if (_elapsedTime >= 1.0f)
            {
                _elapsedTime -= 1.0f;
                Update();
            }
        }
    }
}
