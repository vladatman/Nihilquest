using System;
using System.Collections.Generic;
using System.Text;

namespace Nihilquest
{
    class ActiveItem : Item
    {
        private int itemType;
        private int damageToDeal;
        private int hpToHeal;

        public ActiveItem(string itemName, int posX, int posY, int itemType)
        {
            this.ItemName = itemName;
            this.PosX = posX;
            this.PosY = posY;
            this.itemType = itemType;
            this.damageToDeal = 20;
            this.hpToHeal = 10;
        }

        public void ability(Room[,] roomMap , int playerRoomX, int playerRoomY){
            switch (itemType)
	        {
                // Deal damage to everything
                case 1:
                    foreach (Enemy enemy in roomMap[playerRoomX, playerRoomY].Enemies)
	                {
                        if (!enemy.isDead())
	                    {
                            enemy.Hp -= damageToDeal;
	                    }
	                }
                    break;

                case 2:
                    if (!Game1.P.isDead())
	                {
                        Game1.P.Hp += hpToHeal;
	                }
                    break;

                case 3:
                    break;

                case 4:
                    break;
	        }
        }
    }
}
