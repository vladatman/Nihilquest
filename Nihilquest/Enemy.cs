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
            Dmg = 5;
            Hp = 20;
            Range = 2;
        }
        
    }
}
