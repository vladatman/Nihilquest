using System.Collections.Generic;

namespace Nihilquest
{
    interface IPlayer
    {
        List<Item> Inventory { get; set; }
        int ManaRegen { get; set; }

        bool isInInventory(Item item);
        void pickUpItem(Item item);
    }
}