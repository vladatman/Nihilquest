using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nihilquest
{
    class Enemy : Character
    {
        private Texture2D texture;
        public Enemy(string name, int posX, int posY)
        {
            Name = name;
            PosX = posX;
            PosY = posY;
            Dmg = 2;
            Hp = 10 * Game1.currentLevel;
            Range = 1;
        }

        public Texture2D Texture { get => texture; set => texture = value; }
    }
}
