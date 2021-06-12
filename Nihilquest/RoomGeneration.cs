using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace Nihilquest
{
    public class RoomGeneration
    {
        public static int screenWidth;
        public static int screenHeight;
        private Room[,] level;
        private int mapSize = 10;

        public int MapSize { get => mapSize; set => mapSize = value; }
        internal Room[,] Level { get => level; set => level = value; }

        public void generateRoom()
        {
            // Initial coordinates for spawning rooms
            int x = 1;
            int y = 1;

            Random rnd = new Random();
            // Last direction in which a room was generated
            int last = 1;

            // Amount of rooms to generate
            level = new Room[MapSize, MapSize];

            int roomsGenerated = 0;
            while (roomsGenerated < 10)
            {
                int value = rnd.Next(1, 5);
                if (value != last)
                {
                    switch (value)
                    {
                        case 1:
                            if (y >= 1) { y -= 1; };
                            break;
                        case 2:
                            if (x < MapSize - 1) { x += 1; };
                            break;
                        case 3:
                            if (y < MapSize-1) { y += 1; };
                            break;
                        case 4:
                            if (x >= 1) { x -= 1; };
                            break;
                    }
                    if (level[x, y] == null)
                    {
                        level[x, y] = new Room();
                        roomsGenerated++;
                        last = value;
                    }
                }
            }
        }
    }
}