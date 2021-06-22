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
        public Player(string name, int posX, int posY)
        { 
            Name = name;
            PosX = posX;
            PosY = posY;
            Dmg = 5;
            Hp = 100;
            Mana = 40;
            Range = 2;
            ManaRegen = 5;
        }
        public Player()
        {
            PosX = 4;
            PosY = 4;
            Dmg = 5;
            Hp = 100;
            Mana = 40;
            Range = 2;
            ManaRegen = 5;
        }
        public int ManaRegen { get => manaRegen; set => manaRegen = value; }
        public ActiveItem ActiveItem { get => activeItem; set => activeItem = value; }
        public List<Item> Inventory { get => inventory; set => inventory = value; }


        public void pickUpItem(Item item)
        {
            Dmg += item.AddDmg;
            Mana += item.AddMana;
            Hp += item.AddHealth;
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
