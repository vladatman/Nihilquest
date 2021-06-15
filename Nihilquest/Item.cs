using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nihilquest
{
    class Item
    {
        private String itemName;
        private int addDmg;
        private int addMana;
        private int addHealth;
        private int addManaRegen;
        private int posX;
        private int posY;
        private Texture2D texture;

        public Item(string itemName, int posX, int posY)
        {
            this.ItemName = itemName;
            this.addDmg = 0;
            this.addMana = 0;
            this.addHealth = 0;
            this.AddManaRegen = 0;
            this.PosX = posX;
            this.PosY = posY;
        }

        public int AddDmg { get => addDmg; set => addDmg = value; }
        public int AddMana { get => addMana; set => addMana = value; }
        public int PosX { get => posX; set => posX = value; }
        public int PosY { get => posY; set => posY = value; }
        public string ItemName { get => itemName; set => itemName = value; }
        public Texture2D Texture { get => texture; set => texture = value; }
        public int AddHealth { get => addHealth; set => addHealth = value; }
        public int AddManaRegen { get => addManaRegen; set => addManaRegen = value; }
    }
}
