using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nihilquest
{
    class Room
    {
        private List<Enemy> enemies;
        private List<Item> items;
        private Cell[,] tileMap;
        private bool isStart;
        private bool isBoss;
        private bool isItem;

        private Player player;

        private int gridSize = 10;
        private int tileSize = 64;

        internal Cell[,] TileMap { get => tileMap; set => tileMap = value; }
        public int GridSize { get => gridSize; set => gridSize = value; }
        internal Player Player { get => player; set => player = value; }
        public bool IsStart { get => isStart; set => isStart = value; }
        public bool IsBoss { get => isBoss; set => isBoss = value; }
        public bool IsItem { get => isItem; set => isItem = value; }
        internal List<Enemy> Enemies { get => enemies; set => enemies = value; }
        internal List<Item> Items { get => items; set => items = value; }


        public Room()
        {
          TileMap = new Cell[GridSize, GridSize];
          Enemies = new List<Enemy>();
          items = new List<Item>();

        }

        public void generateTileMap()
        {
            for (int i = 0; i < GridSize; ++i)
            {
                for (int j = 0; j < GridSize; ++j)
                {
                    Rectangle rectangle = new Rectangle(i * tileSize, j * tileSize, tileSize, tileSize);
                    TileMap[i, j] = new Cell(rectangle, true);

                }
            }
        }
        public void createWalls()
        {
            for (int o = 0; o < gridSize; ++o)
            {

                tileMap[o, 0].IsLegal = false;
                tileMap[o, 0].IsWall = true;

                tileMap[0, o].IsLegal = false;
                tileMap[0, o].IsWall = true;

                tileMap[o, gridSize - 1].IsLegal = false;
                tileMap[o, gridSize - 1].IsWall = true;

                tileMap[gridSize - 1, o].IsLegal = false;
                tileMap[gridSize - 1, o].IsWall = true;
            }

        }
        public void createDoors(Room[,] rooms, int x, int y)
        {
            for (int o = 4; o <= 5; ++o)
            {
                if (y != 8 && rooms[x, y + 1] != null)
                {
                    //TOP
                    tileMap[o, 0].IsLegal = true;
                    tileMap[o, 0].IsDoor = true;
                    tileMap[o, 0].IsWall = false;
                }
                if (x != 8 && rooms[x + 1, y] != null)
                {
                    //RIGHT
                    tileMap[9, o].IsLegal = true;
                    tileMap[9, o].IsDoor = true;
                    tileMap[9, o].IsWall = false;
                }
                if (y != 0 && rooms[x, y - 1] != null)
                {
                    //BOTTOM
                    tileMap[o, 9].IsLegal = true;
                    tileMap[o, 9].IsDoor = true;
                    tileMap[o, 9].IsWall = false;
                }
                if (x != 0 && rooms[x - 1, y] != null)
                {
                    //LEFT
                    tileMap[0, o].IsLegal = true;
                    tileMap[0, o].IsDoor = true;
                    tileMap[0, o].IsWall = false;
                }
                

            }

        }
    }

}
