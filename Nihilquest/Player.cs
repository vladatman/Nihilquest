using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Nihilquest
{
    class Player : Character
    {
        protected int manaRegen;
        protected List<Item> inventory = new List<Item>();
        protected ActiveItem activeItem;
        private int maxMana;
        private int maxHealth;
        public Player(string name, int posX, int posY)
        { 
            Name = name;
            PosX = posX;
            PosY = posY;
            Dmg = 10;
            Hp = 200;
            Mana = 40;
            MaxHealth = 200;
            MaxMana = 100;
            Range = 2;
            ManaRegen = 5;
        }
        public Player()
        {
            PosX = 4;
            PosY = 4;
            Dmg = 10;
            Hp = 200;
            Mana = 40;
            MaxHealth = 200;
            MaxMana = 100;
            Range = 2;
            ManaRegen = 5;
        }
        public int ManaRegen { get => manaRegen; set => manaRegen = value; }
        public ActiveItem ActiveItem { get => activeItem; set => activeItem = value; }
        public List<Item> Inventory { get => inventory; set => inventory = value; }
        public int MaxMana { get => maxMana; set => maxMana = value; }
        public int MaxHealth { get => maxHealth; set => maxHealth = value; }

        public void pickUpItem(Item item)
        {
            Dmg += item.AddDmg;
            maxMana += item.AddMana;
            MaxHealth += item.AddHealth;
            ManaRegen += item.AddManaRegen;

            Inventory.Add(item);

            if (item is ActiveItem)
            {
                inventory.Remove(activeItem);
                activeItem = item as ActiveItem;
            }
        }
        public bool isInInventory(Item item)
        {
            bool checkItem = false;
            foreach (Item i in Inventory)
            {
                if(i == item)
                {
                    checkItem = true;
                }
            }
            return checkItem;
        }
        // Shows the inventory in string format on UI
    }
}
