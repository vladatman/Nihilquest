using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Nihilquest
{
    class Player : Character
    {
        protected int manaRegen;
        protected ArrayList actionList;
        protected List<Item> inventory = new List<Item>();
        public Player(string name, int posX, int posY)
        { 
            Name = name;
            PosX = posX;
            PosY = posY;
            Dmg = 5;
            Hp = 50;
            Mana = 40;
            Range = 2;
            ManaRegen = 5;
            actionList = new ArrayList();
            actionList.Add("Attack");
        }

        public int ManaRegen { get => manaRegen; set => manaRegen = value; }
        public List<Item> Inventory { get => inventory; set => inventory = value; }

        public void pickUpItem(Item item)
        {
            if(item.ItemName == "butterknife")
            {
                if (item.AddDmg != null)
                {
                    Dmg += item.AddDmg;
                }
            }

            if (item.ItemName == "manaflask")
            {
                if (item.AddMana != null)
                {
                    Mana += item.AddMana;
                }
            }
            
            Inventory.Add(item);
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
    }
}
