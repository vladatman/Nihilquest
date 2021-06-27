using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Nihilquest
{
    class Room
    {
        private List<Enemy> enemies;
        private Boss boss;
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
        internal Boss Boss { get => boss; set => boss = value; }
        public bool IsStart { get => isStart; set => isStart = value; }
        public bool IsBoss { get => isBoss; set => isBoss = value; }
        public bool IsItem { get => isItem; set => isItem = value; }
        internal List<Enemy> Enemies { get => enemies; set => enemies = value; }
        internal List<Item> Items { get => items; set => items = value; }

        public Room(List<Enemy> enemies, Boss boss, List<Item> items, Cell[,] tileMap, bool isStart, bool isBoss, bool isItem, Player player, int gridSize, int tileSize)
        {
            this.enemies = enemies;
            this.boss = boss;
            this.items = items;
            this.tileMap = tileMap;
            this.isStart = isStart;
            this.isBoss = isBoss;
            this.isItem = isItem;
            this.player = player;
            this.gridSize = gridSize;
            this.tileSize = tileSize;
        }

        public Room()
        {
          TileMap = new Cell[GridSize, GridSize];
          Enemies = new List<Enemy>();
          items = new List<Item>();
            isStart = false;
            isBoss = false;
            IsItem = false;

        }
        //determines the layout of tiles
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
            if (isBoss == false && isItem == false && isStart == false)
            {
                generateRoomInside();
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
        //generates everything in the room according to the image it is reading
        public void generateRoomInside()
        {
            Random rand = new Random();
            //use this for debugging
            //String path = Path.Combine(VisualStudioProvider.TryGetSolutionDirectoryInfo().Parent.FullName, @"Nihilquest\Nihilquest\Content\rooms\room" + rand.Next(1, 10)+".png");

            //use this for release
            String path = Path.Combine(Environment.CurrentDirectory, @"rooms\room" + rand.Next(1, 10) + ".png");
            imgReader imgReader = new imgReader(path);
            Cell[,] cellMap = imgReader.readImg();
            for (int i = 0; i < GridSize; ++i)
            {
                for (int j = 0; j < GridSize; ++j)
                {
                   if(cellMap[i,j]!= null)
                    {
                        if(cellMap[i, j].IsWall)
                        {
                            tileMap[i, j].IsWall = true;
                            tileMap[i, j].IsLegal = false;
                        }
                        else if (cellMap[i, j].isEnemy())
                        {
                            Enemy e = new Enemy("mob", i, j);
                            switch (new Random().Next(6))
                            {
                                //swamp
                                case 1:
                                    e.Dmg = 3 * Game1.currentLevel;
                                    e.Hp = 15 * Game1.currentLevel;
                                    e.Texture = Game1.swampTexture;
                                    e.Range = 2;
                                    break;
                                //slime
                                case 2:
                                    e.Range = 4;
                                    e.Texture = Game1.skeletonTexture;
                                    break;
                                //goblin
                                case 3:
                                    e.Dmg = 1 * Game1.currentLevel;
                                    e.Hp = 5 * Game1.currentLevel;
                                    e.Texture = Game1.goblinTexture;
                                    break;
                                case 4:
                                    e.Dmg = 1 * Game1.currentLevel;
                                    e.Hp = 5 * Game1.currentLevel;
                                    e.Texture = Game1.goblinTexture;
                                    break;
                                //imp
                                default:
                                    e.Texture = Game1.enemyTexture;
                                    break;
                            }
                            Enemies.Add(e);
                            tileMap[i, j].Character = e;
                            tileMap[i, j].IsLegal = false;
                        }
                    }
                }
            }
        }
    }

}
