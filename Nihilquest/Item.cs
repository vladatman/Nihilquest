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
        public Item(string itemName, int addDmg, int addMana)
        {
            this.ItemName = itemName;
            this.AddDmg = addDmg;
            this.addMana = addMana;
        }

        public int AddDmg { get => addDmg; set => addDmg = value; }
        public string ItemName { get => itemName; set => itemName = value; }
        public int AddMana { get => addMana; set => addMana = value; }
    }
}
