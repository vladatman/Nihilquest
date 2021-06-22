using System;
using System.Collections.Generic;
using System.Text;

namespace Nihilquest
{
    class Boss : Character
    {
        public Boss(string name, int posX, int posY)
        {
            Name = name;
            PosX = posX;
            PosY = posY;
            Dmg = 5 * Game1.currentLevel;
            Hp = 25 * Game1.currentLevel;
            Range = 2;
        }
    }
}
