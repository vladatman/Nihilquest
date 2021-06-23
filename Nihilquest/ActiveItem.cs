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
        private int OldDamage;
        private int OldRange;

        public ActiveItem(string itemName, int posX, int posY, int itemType)
        {
            this.ItemName = itemName;
            this.PosX = posX;
            this.PosY = posY;
            this.itemType = itemType;
            this.damageToDeal = 10;
            this.hpToHeal = 20;
        }

        public ActiveItem()
        {
        }

        public void Ability(Room[,] roomMap , int playerRoomX, int playerRoomY){
            var Player = roomMap[playerRoomX, playerRoomY].Player;
            //OldRange = Player.Range;
            //OldDamage = Player.Dmg;
            switch (itemType)
	        {
                // Deal damage to everything
                case 1:
                    if (Player.Mana >= 40)
                    {
                        foreach (Enemy enemy in roomMap[playerRoomX, playerRoomY].Enemies)
                        {
                            if (!enemy.isDead() )
                            {
                                enemy.Hp -= damageToDeal + Player.Dmg;
                                
                            }
                        }
                        if(!roomMap[playerRoomX, playerRoomY].Boss.isDead() && roomMap[playerRoomX, playerRoomY].Boss != null)
                        {
                            roomMap[playerRoomX, playerRoomY].Boss.Hp -= damageToDeal + Player.Dmg;
                        }
                        Player.Mana -= 40;
                    }
                    break;

                case 2:
                    if (Player.Mana >= 20)
                    {
                        if (!Player.isDead() && Player.Hp < Player.MaxHealth)
                        {
                            Player.Hp += hpToHeal;
                            Player.Mana -= 20;
                        }

                    }
                    break;

                case 3:
                    if (Player.Mana >= 40)
                    {
                        if (!Player.isDead())
                        {
                            Player.Range = 10;
                        }
                        Player.Mana -= 40;
                    }
                    break;

                case 4:
                    if (Player.Mana >= 50)
                    {
                        if (!Player.isDead())
                        {
                            Player.Dmg += Player.Dmg + 50;
                        }
                        Player.Mana -= 50;
                    }
                    break;
	        }
        }

        public void EndItem(Room[,] roomMap, int playerRoomX, int playerRoomY)
        {
            var Player = roomMap[playerRoomX, playerRoomY].Player;
            Player.Range = OldRange;
            Player.Dmg = OldDamage;
        }
    }
}
