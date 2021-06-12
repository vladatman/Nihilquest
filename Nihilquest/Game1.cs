using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace Nihilquest
{
    public class Game1 : Game
    {

        //System.Diagnostics.Debug.WriteLine(); write to console

        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        Texture2D[] tileTexture = new Texture2D[8];
        Texture2D playerTexture;
        Texture2D enemyTexture;
        Texture2D obstTexture;
        Texture2D swordTexture;
        Texture2D manaTexture;
        Texture2D heartTexture;

        private Player P = new Player("Wairen", 5, 5);
        private Room[,] roomMap;
        private RoomGeneration rg;

        private int playerRoomX;
        private int playerRoomY;

        public static int windowWidth = 960;
        public static int windowHeight = 640;

        private Item mana = new Item("Mana flask",0,5);
        private Item sword = new Item("Sword", 5,0);

        MainMenu main = new MainMenu();

        private bool playerTurn = true;
        private SpriteFont font;
        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            playerRoomX=1;
            playerRoomY=1;
            rg = new RoomGeneration();
            rg.generateRoom();
            roomMap = rg.Level;
            foreach (Room r in roomMap)
            {
                //System.Diagnostics.Debug.WriteLine(r == null);
                if (r != null)
                {
                    r.generateTileMap();
                    r.createWalls();
                    r.createDoors();
                }
            }
            roomMap[playerRoomX, playerRoomY].Player = P;
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            _graphics.IsFullScreen = false;
            _graphics.PreferredBackBufferWidth = windowWidth;  // set this value to the desired width of your window
            _graphics.PreferredBackBufferHeight = windowHeight;   // set this value to the desired height of your window
            _graphics.ApplyChanges();
            base.Initialize();
        }

        protected override void LoadContent()
        {

            _spriteBatch = new SpriteBatch(GraphicsDevice);
            tileTexture[0] = this.Content.Load<Texture2D>("floor_1");
            tileTexture[1] = this.Content.Load<Texture2D>("floor_2");
            tileTexture[2] = this.Content.Load<Texture2D>("floor_3");
            tileTexture[3] = this.Content.Load<Texture2D>("floor_4");
            tileTexture[4] = this.Content.Load<Texture2D>("floor_5");
            tileTexture[5] = this.Content.Load<Texture2D>("floor_6");
            tileTexture[6] = this.Content.Load<Texture2D>("floor_7");
            tileTexture[7] = this.Content.Load<Texture2D>("floor_8");
            playerTexture = this.Content.Load<Texture2D>("knight_f_run_anim_f0");
            enemyTexture = this.Content.Load<Texture2D>("imp_idle_anim_f0");
            obstTexture = this.Content.Load<Texture2D>("wall_mid");
            swordTexture = this.Content.Load<Texture2D>("weapon_knife");
            manaTexture = this.Content.Load<Texture2D>("flask_big_blue");
            heartTexture = this.Content.Load<Texture2D>("ui_heart_full");
            font = Content.Load<SpriteFont>("UIfont");
            main.LoadContent(Content);


        }

        protected override void Update(GameTime gameTime)
        {

            if (Keyboard.GetState().IsKeyDown(Keys.T))
            {
                playerTurn = true;
            }

            main.Update();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            _spriteBatch.Begin(SpriteSortMode.Deferred,
              BlendState.AlphaBlend,
              SamplerState.PointClamp,
              null, null, null, null);
            MouseState mouseState = Mouse.GetState();
            int mouseX = mouseState.X;
            int mouseY = mouseState.Y;

            if (roomMap[playerRoomX, playerRoomY].Player != null)
            {
                for (int i = 0; i < roomMap[playerRoomX, playerRoomY].GridSize; ++i)
                {
                    for (int j = 0; j < roomMap[playerRoomX, playerRoomY].GridSize; ++j)
                    {
                        //tilemap rendering
                        if (roomMap[playerRoomX, playerRoomY].TileMap[i, j].IsWall)
                        {
                            _spriteBatch.Draw(obstTexture, roomMap[playerRoomX, playerRoomY].TileMap[i, j].Rectangle, Color.White);
                        }
                        else if (roomMap[playerRoomX, playerRoomY].TileMap[i, j].IsDoor)
                        {
                            _spriteBatch.Draw(tileTexture[1], roomMap[playerRoomX, playerRoomY].TileMap[i, j].Rectangle, Color.White);
                        }
                        else
                        {
                            _spriteBatch.Draw(tileTexture[0], roomMap[playerRoomX, playerRoomY].TileMap[i, j].Rectangle, Color.White);
                        }
                        if (playerTurn)
                        {
                            //hover highlight
                            if (roomMap[playerRoomX, playerRoomY].TileMap[i, j].Rectangle.Contains(mouseX, mouseY))
                            {
                                if (roomMap[playerRoomX, playerRoomY].TileMap[i, j].IsLegal)
                                {
                                    _spriteBatch.Draw(tileTexture[0], roomMap[playerRoomX, playerRoomY].TileMap[i, j].Rectangle, null, Color.Blue * 0.5f);
                                }
                                //mouseclick movement
                                if (mouseState.LeftButton == ButtonState.Pressed && roomMap[playerRoomX, playerRoomY].TileMap[i, j].IsLegal)
                                {
                                    roomMap[playerRoomX, playerRoomY].TileMap[P.PosX, P.PosY].Character = null;
                                    roomMap[playerRoomX, playerRoomY].Player.PosX = i;
                                    roomMap[playerRoomX, playerRoomY].Player.PosY = j;
                                    roomMap[playerRoomX, playerRoomY].TileMap[roomMap[playerRoomX, playerRoomY].Player.PosX, roomMap[playerRoomX, playerRoomY].Player.PosY].Character = roomMap[playerRoomX, playerRoomY].Player;
                                    playerTurn = false;
                                    //pickup item
                                    if (roomMap[playerRoomX, playerRoomY].TileMap[i, j].hasItem())
                                    {
                                        P.pickUpItem(roomMap[playerRoomX, playerRoomY].TileMap[i, j].Item);
                                    }
                                    if (mouseState.LeftButton == ButtonState.Pressed && roomMap[playerRoomX, playerRoomY].TileMap[i, j].IsDoor)
                                    {
                                        //TOP
                                        if ((i == 4 || i == 5) && j == 0)
                                        {
                                            if (playerRoomY + 1 < rg.MapSize - 1)
                                            {
                                                if (roomMap[playerRoomX, playerRoomY + 1] != null)
                                                {
                                                    roomMap[playerRoomX, playerRoomY + 1].Player = roomMap[playerRoomX, playerRoomY].Player;
                                                    roomMap[playerRoomX, playerRoomY].Player = null;
                                                    playerRoomY++;
                                                    System.Diagnostics.Debug.WriteLine(playerRoomX + ":" + playerRoomY);
                                                }
                                            }
                                        }
                                        //LEFT
                                        if ((j == 4 || j == 5) && i == 0)
                                        {
                                            if (playerRoomX - 1 > 0)
                                            {
                                                if (roomMap[playerRoomX - 1, playerRoomY] != null)
                                                {
                                                    roomMap[playerRoomX - 1, playerRoomY].Player = roomMap[playerRoomX, playerRoomY].Player;
                                                    roomMap[playerRoomX, playerRoomY].Player = null;
                                                    playerRoomX--;
                                                    System.Diagnostics.Debug.WriteLine(playerRoomX + ":" + playerRoomY);
                                                }
                                            }
                                        }
                                        //BOTTOM
                                        if ((i == 4 || i == 5) && j == 9)
                                        {
                                            if (playerRoomY - 1 > 0)
                                            {
                                                if (roomMap[playerRoomX, playerRoomY - 1] != null)
                                                {
                                                    roomMap[playerRoomX, playerRoomY - 1].Player = roomMap[playerRoomX, playerRoomY].Player;
                                                    roomMap[playerRoomX, playerRoomY].Player = null;
                                                    playerRoomY--;
                                                    System.Diagnostics.Debug.WriteLine(playerRoomX + ":" + playerRoomY);
                                                }
                                            }
                                        }
                                        //RIGHT
                                        if ((j == 4 || j == 5) && i == 9)
                                        {
                                            if (playerRoomX + 1 < rg.MapSize - 1)
                                            {
                                                if (roomMap[playerRoomX + 1, playerRoomY] != null)
                                                {
                                                    roomMap[playerRoomX + 1, playerRoomY].Player = roomMap[playerRoomX, playerRoomY].Player;
                                                    playerRoomX++;
                                                    roomMap[playerRoomX, playerRoomY].Player = null;
                                                    System.Diagnostics.Debug.WriteLine(playerRoomX + ":" + playerRoomY);
                                                }
                                            }
                                        }
                                    }
                                }
                                //attack enemy
                                else if (mouseState.LeftButton == ButtonState.Pressed && roomMap[playerRoomX, playerRoomY].TileMap[i, j].hasCharacter())
                                {
                                    P.Attack(roomMap[playerRoomX, playerRoomY].TileMap[i, j].Character);
                                    playerTurn = false;
                                }
                            }
                        }
                    }
                }
                _spriteBatch.Draw(playerTexture, roomMap[playerRoomX, playerRoomY].TileMap[P.PosX, P.PosY].Rectangle, Color.White);
                createEnemy("mob1", 5, 6);
                createEnemy("mob1", 5, 7);
                drawEnemy();
                //item drawing
                if (!roomMap[playerRoomX, playerRoomY].Player.isInInventory(sword))
                {
                    _spriteBatch.Draw(swordTexture, roomMap[playerRoomX, playerRoomY].TileMap[2, 2].Rectangle, Color.White);
                    roomMap[playerRoomX, playerRoomY].TileMap[2, 2].Item = sword;
                }
                else
                {
                    roomMap[playerRoomX, playerRoomY].TileMap[2, 2].Item = null;
                }
                if (!roomMap[playerRoomX, playerRoomY].Player.isInInventory(mana))
                {
                    _spriteBatch.Draw(manaTexture, roomMap[playerRoomX, playerRoomY].TileMap[4, 4].Rectangle, Color.White);
                    roomMap[playerRoomX, playerRoomY].TileMap[4, 4].Item = mana;
                }
                else
                {
                    roomMap[playerRoomX, playerRoomY].TileMap[4, 4].Item = null;
                }
            }

            _spriteBatch.DrawString(font, "Actions:", new Vector2(670, 10), Color.White);
            _spriteBatch.Draw(heartTexture, new Vector2(670, 30), Color.White);
            _spriteBatch.DrawString(font, "" + roomMap[playerRoomX, playerRoomY].Player.Hp, new Vector2(690, 30), Color.White);
            _spriteBatch.DrawString(font, "Mana: " + roomMap[playerRoomX, playerRoomY].Player.Mana, new Vector2(670, 50), Color.White);
            _spriteBatch.DrawString(font, "DMG: " + roomMap[playerRoomX, playerRoomY].Player.Dmg, new Vector2(670, 70), Color.White);
            _spriteBatch.DrawString(font, "range: " + roomMap[playerRoomX, playerRoomY].Player.Range, new Vector2(670, 90), Color.White);
            _spriteBatch.DrawString(font, "Inventory: ", new Vector2(770, 10), Color.White);
            // Inventory
            int invY = 30;
            int itemDmg = 5;
            int itemMana = 5;
            foreach (Item item in roomMap[playerRoomX, playerRoomY].Player.Inventory)
            {
               if(item.ItemName == "butterknife")
                {
                    _spriteBatch.DrawString(font, "" + item.ItemName + " +" + itemDmg + " DMG", new Vector2(770, invY), Color.White);
                    invY += 20;
                    itemDmg += 5;
                }
               if(item.ItemName == "manaflask")
                {
                    _spriteBatch.DrawString(font, "" + item.ItemName + " +" + itemMana + " Mana", new Vector2(770, invY), Color.White);
                    invY += 20;
                    itemMana += 5;
                }
            }
            main.Draw(_spriteBatch);
            _spriteBatch.End();
            base.Draw(gameTime);
        }
        public void createObstacle(int posX,int posY)
        {
            _spriteBatch.Draw(obstTexture, roomMap[playerRoomX, playerRoomY].TileMap[posX, posY].Rectangle, Color.White);
            roomMap[playerRoomX, playerRoomY].TileMap[posX, posY].IsLegal = false;
        }
        private void drawEnemy()
        {
            foreach (Enemy e in roomMap[playerRoomX, playerRoomY].Enemies)
            {
                if (e.isDead())
                {
                    roomMap[playerRoomX, playerRoomY].TileMap[e.PosX, e.PosY].Character = null;
                    roomMap[playerRoomX, playerRoomY].TileMap[e.PosX, e.PosY].IsLegal = true;
                }
                else
                {
                    _spriteBatch.Draw(enemyTexture, roomMap[playerRoomX, playerRoomY].TileMap[e.PosX, e.PosY].Rectangle, Color.White);
                    _spriteBatch.DrawString(font, "HP:" + e.Hp, new Vector2(roomMap[playerRoomX, playerRoomY].TileMap[e.PosX, e.PosY].Rectangle.X, roomMap[playerRoomX, playerRoomY].TileMap[e.PosX, e.PosY].Rectangle.Y), Color.White);
                    roomMap[playerRoomX, playerRoomY].TileMap[e.PosX, e.PosY].Character = e;
                    roomMap[playerRoomX, playerRoomY].TileMap[e.PosX, e.PosY].IsLegal = false;
                }
            }
        }
        private void createEnemy(string name,int posX,int posY)
        {
            roomMap[playerRoomX, playerRoomY].Enemies.Add(new Enemy(name, posX, posY));
        }

        }
    }
