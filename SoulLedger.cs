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

namespace Archuniverse
{
    /// <summary>
    /// SoulLedger is a globally accessible, singleton registry of all living beings (characters and monsters).
    /// Acts as the authoritative source for entity tracking, spawning, and removal across the game.
    /// </summary>
    public class SoulLedger : Base
    {
        // Singleton instance
        private static SoulLedger? _instance;
        public static SoulLedger Instance => _instance ??= new SoulLedger();

        // Private constructor prevents external instantiation
        private SoulLedger() { }

        private readonly List<Character> _characters = new();
        private readonly List<Monster> _monsters = new();

        public IReadOnlyList<Character> Characters => _characters;
        public IReadOnlyList<Monster> Monsters => _monsters;

        /// <summary>
        /// Returns all living entities (characters + monsters) as LivingEntity references.
        /// </summary>
        public IEnumerable<LivingEntity> AllLivingBeings =>
            _characters.Cast<LivingEntity>().Concat(_monsters);

        /// <summary>
        /// Creates and registers a new playable character.
        /// </summary>
        public Character CreateCharacter(
            string name, Character.Sex gender,
            int health, int mana, int stamina,
            int gold, int xp, int level,
            float speed, int baseAttack, int baseDefence,
            int maxHealth = 100, int maxMana = 100, int maxStamina = 100)
        {
            var character = new Character(
                name, gender, health, mana, stamina, gold, xp,
                level, speed, baseAttack, baseDefence,
                maxHealth, maxMana, maxStamina);

            _characters.Add(character);
            return character;
        }

        /// <summary>
        /// Creates and registers a new monster.
        /// </summary>
        public Monster CreateMonster(
            string name, int health, int mana, int stamina,
            int xp, int level, float speed,
            int baseAttack, int baseDefence,
            int maxHealth = 100, int maxMana = 100, int maxStamina = 100)
        {
            var monster = new Monster(
                name, health, mana, stamina, xp,
                level, speed, baseAttack, baseDefence,
                maxHealth, maxMana, maxStamina);

            _monsters.Add(monster);
            return monster;
        }

        /// <summary>
        /// Removes a character or monster from the ledger.
        /// </summary>
        public void Remove(LivingEntity entity)
        {
            if (entity is Character character)
                _characters.Remove(character);
            else if (entity is Monster monster)
                _monsters.Remove(monster);
        }

        /// <summary>
        /// Finds a living entity by its UniqueId. Returns null if not found.
        /// </summary>
        public LivingEntity? GetById(Guid id)
        {
            return AllLivingBeings.FirstOrDefault(e => e.UniqueId == id);
        }

        /// <summary>
        /// Clears all tracked characters and monsters.
        /// </summary>
        public void ResetLedger()
        {
            _characters.Clear();
            _monsters.Clear();
        }

        /// <summary>
        /// Called when an entity dies — removes it from the ledger.
        /// </summary>
        public void OnEntityDeath(LivingEntity entity)
        {
            Remove(entity);
            // Future: trigger events, drop loot, respawn, etc.
        }
    }
}
