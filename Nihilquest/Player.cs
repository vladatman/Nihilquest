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

    }
}
