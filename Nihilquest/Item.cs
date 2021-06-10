using System;
using System.Collections.Generic;
using System.Text;

namespace Nihilquest
{
    class Item
    {
        private String itemName;
        private int addDmg;

        public Item(string itemName, int addDmg)
        {
            this.ItemName = itemName;
            this.AddDmg = addDmg;
        }

        public int AddDmg { get => addDmg; set => addDmg = value; }
        public string ItemName { get => itemName; set => itemName = value; }
    }
}
