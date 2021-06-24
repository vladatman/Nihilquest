using System;
using System.Collections.Generic;
using System.Text;

namespace Nihilquest
{
    class Character
    {
        protected String name;
        protected int hp;
        protected int mana;
        protected int posX;
        protected int posY;
        protected int dmg;
        protected int range;

        public string Name { get => name; set => name = value; }
        public int Hp { get => hp; set => hp = value; }
        public int Mana { get => mana; set => mana = value; }
        public int PosX { get => posX; set => posX = value; }
        public int PosY { get => posY; set => posY = value; }
        public int Dmg { get => dmg; set => dmg = value; }
        public int Range { get => range; set => range = value; }

        //attacks character in range
        public void Attack(Character charA)
        {
            
            if (charA.hp > 0 && Math.Abs(charA.PosX - PosX) <= Range && Math.Abs(charA.PosY - PosY) <= Range)
            {
               charA.Hp -= dmg;
            }
        }
        public bool isDead()
        {
            return Hp <= 0 ? true : false;
        }
    }
}
