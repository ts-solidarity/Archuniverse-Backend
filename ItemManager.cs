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
    /// ItemManager handles item creation, lookup, and registration across the world.
    /// Items can be manually created or pulled from a predefined catalog in the future.
    /// </summary>
    public class ItemManager : Base
    {
        // Singleton instance
        private static ItemManager? _instance;
        public static ItemManager Instance => _instance ??= new ItemManager();

        private ItemManager() { }

        // All generated item instances
        private readonly List<Item> _items = new();
        public IReadOnlyList<Item> AllItems => _items;

        // Optionally preload item templates or drop tables here
        // private Dictionary<string, ItemTemplate> _templates = new();

        // ========== CREATE METHODS ==========

        public Armor CreateArmor(string name, Item.Grade grade, int worth, int defenceValue)
        {
            var armor = new Armor(name, grade, worth, defenceValue);
            _items.Add(armor);
            return armor;
        }

        public Weapon CreateWeapon(string name, Item.Grade grade, int worth, int attackValue, int defenceValue)
        {
            var weapon = new Weapon(name, grade, worth, attackValue, defenceValue);
            _items.Add(weapon);
            return weapon;
        }

        public Potion CreatePotion(string name, Item.Grade grade, int worth, 
            int healthBoost, int manaBoost, int staminaBoost, int effectTime)
        {
            var potion = new Potion(name, grade, worth, healthBoost, manaBoost, staminaBoost, effectTime);
            _items.Add(potion);
            return potion;
        }

        public Food CreateFood(string name, Item.Grade grade, int worth, int healthBoost)
        {
            var food = new Food(name, grade, worth, healthBoost);
            _items.Add(food);
            return food;
        }

        public Ware CreateWare(string name, Item.Grade grade, int worth)
        {
            var ware = new Ware(name, grade, worth);
            _items.Add(ware);
            return ware;
        }

        // ========== MANAGEMENT METHODS ==========

        public void RemoveItem(Item item)
        {
            _items.Remove(item);
        }

        public Item? GetItemById(Guid id)
        {
            return _items.Find(item => item.UniqueId == id);
        }

        public void ClearAllItems()
        {
            _items.Clear();
        }

        /// <summary>
        /// Creates a brand new item with the same stats as the original. Returns the duplicated item.
        /// </summary>
        public Item DuplicateItem(Item original)
        {
            Item copy;

            switch (original)
            {
                case Armor armor:
                    copy = CreateArmor(armor.Name, armor.ItemGrade, armor.Worth, armor.DefenceValue);
                    break;

                case Weapon weapon:
                    copy = CreateWeapon(weapon.Name, weapon.ItemGrade, weapon.Worth, weapon.AttackValue, weapon.DefenceValue);
                    break;

                case Potion potion:
                    copy = CreatePotion(potion.Name, potion.ItemGrade, potion.Worth,
                        potion.HealthBoost, potion.ManaBoost, potion.StaminaBoost, potion.EffectTime);
                    break;

                case Food food:
                    copy = CreateFood(food.Name, food.ItemGrade, food.Worth, food.HealthBoost);
                    break;

                case Ware ware:
                    copy = CreateWare(ware.Name, ware.ItemGrade, ware.Worth);
                    break;

                default:
                    throw new NotSupportedException($"Cannot duplicate unknown item type: {original.GetType().Name}");
            }

            return copy;
        }


        // ========== FUTURE EXTENSIONS ==========

        // public Item GenerateFromTemplate(string templateId) { ... }
        // public Item DropLoot(string monsterType) { ... }
        // public void LoadItemCatalogFromJson() { ... }
    }
}
