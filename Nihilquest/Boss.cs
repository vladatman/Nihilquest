using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nihilquest
{

    // This class is exteneded from character
    class Boss : Character {

        // This is the constructor of the boss class
        public Boss(string name, int posX, int posY)
        {
            Name = name;
            PosX = posX;
            PosY = posY;
            // Damage is increased per level
            Dmg = 5 * Game1.currentLevel;
            // Hp is increased per level
            Hp = 25 * Game1.currentLevel;
            Range = 2;
        }
    }
}
