using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nihilquest
{
    class Room
    {
        private List<Enemy> Enemies;
        private List<Item> Items;
        private Cell[,] tileMap;

        private Player player;

        private int gridSize = 10;
        private int tileSize = 64;

        internal Cell[,] TileMap { get => tileMap; set => tileMap = value; }
        public int GridSize { get => gridSize; set => gridSize = value; }
        internal Player Player { get => player; set => player = value; }

        public Room()
        {
            this.TileMap = new Cell[GridSize, GridSize];
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
                tileMap[o, gridSize-1].IsLegal = false;
                tileMap[o, gridSize - 1].IsWall = true;
                tileMap[gridSize - 1, o].IsLegal = false;
                tileMap[gridSize - 1, o].IsWall = true;
            }

        }
    }

}
