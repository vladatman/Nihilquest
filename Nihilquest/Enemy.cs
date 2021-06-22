using System;
using System.Collections.Generic;
using System.Text;

namespace Nihilquest
{
    class Enemy : Character
    {
        public Enemy(string name, int posX, int posY)
        {
            Name = name;
            PosX = posX;
            PosY = posY;
            Dmg = 2 * Game1.currentLevel;
            Hp = 10 * Game1.currentLevel;
            Range = 1;
        }
        
    }
}
