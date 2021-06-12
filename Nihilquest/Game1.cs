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

        private Player P = new Player("Wairen",5,5);
        private Room[,] roomMap;
        private RoomGeneration rg;

        public static int windowWidth = 960;
        public static int windowHeight = 640;

        MainMenu main = new MainMenu();

        private bool playerTurn = true;
        private SpriteFont font;
        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            rg = new RoomGeneration();
            rg.generateRoom();
            roomMap = rg.Level;
            foreach(Room r in roomMap)
            {
                //System.Diagnostics.Debug.WriteLine(r == null);
                if (r != null)
                {
                    r.generateTileMap();
                    r.createWalls();
                    r.createDoors();
                    r.Player = P;
                }
            }
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
            for (int x = 0; x < rg.MapSize; ++x)
            {
                for (int y = 0; y < rg.MapSize; ++y)
                {
                    for (int i = 0; i < roomMap[x, y].GridSize; ++i)
                    {
                        for (int j = 0; j < roomMap[x, y].GridSize; ++j)
                        {
                            //tilemap rendering
                            if (roomMap[x, y].TileMap[i, j].IsWall)
                            {
                                _spriteBatch.Draw(obstTexture, roomMap[x, y].TileMap[i, j].Rectangle, Color.White);
                            }
                            else if (roomMap[x, y].TileMap[i, j].IsDoor)
                            {
                                _spriteBatch.Draw(tileTexture[1], roomMap[x, y].TileMap[i, j].Rectangle, Color.White);
                            }
                            else
                            {
                                _spriteBatch.Draw(tileTexture[0], roomMap[x, y].TileMap[i, j].Rectangle, Color.White);
                            }
                            if (playerTurn)
                            {
                                //hover highlight
                                if (roomMap[x, y].TileMap[i, j].Rectangle.Contains(mouseX, mouseY))
                                {
                                    if (roomMap[x, y].TileMap[i, j].IsLegal)
                                    {
                                        _spriteBatch.Draw(tileTexture[0], roomMap[x, y].TileMap[i, j].Rectangle, null, Color.Blue * 0.5f);
                                    }
                                    //mouseclick movement
                                    if (mouseState.LeftButton == ButtonState.Pressed && roomMap[x, y].TileMap[i, j].IsLegal)
                                    {
                                        roomMap[x, y].TileMap[P.PosX, P.PosY].Character = null;
                                        P.PosX = i;
                                        P.PosY = j;
                                        roomMap[x, y].TileMap[P.PosX, P.PosY].Character = P;
                                        playerTurn = false;
                                        //pickup item
                                        if (roomMap[x, y].TileMap[i, j].hasItem())
                                        {
                                            P.pickUpItem(roomMap[x, y].TileMap[i, j].Item);
                                        }

                                    }
                                    //attack enemy
                                    else if (mouseState.LeftButton == ButtonState.Pressed && roomMap[x, y].TileMap[i, j].hasCharacter())
                                    {
                                        P.Attack(roomMap[x, y].TileMap[i, j].Character);
                                        playerTurn = false;
                                    }
                                }
                            }
                        }
                    }
                    _spriteBatch.Draw(playerTexture, roomMap[x, y].TileMap[P.PosX, P.PosY].Rectangle, Color.White);
                }
            }
            main.Draw(_spriteBatch);
            _spriteBatch.End();
            base.Draw(gameTime);
            /*
            //player model rendering
            _spriteBatch.Draw(playerTexture, tileMap[P1.PosX, P1.PosY].Rectangle, Color.White);
            for (int o = 0; o < gridSize; ++o)
            {
                createObstacle(o,0);
                createObstacle(o, gridSize-1);
                createObstacle(0,o);
                createObstacle(gridSize-1, o);

            }
            createEnemy("mob1",5,6);
            createEnemy("mob1", 5, 7);
            drawEnemy();
            //item drawing
            if (!P1.isInInventory(sword))
            {
                _spriteBatch.Draw(swordTexture, tileMap[2, 2].Rectangle, Color.White);
                tileMap[2, 2].Item = sword;
            }
            else
            {
                tileMap[2, 2].Item = null;
            }
            if (!P1.isInInventory(mana))
            {
                _spriteBatch.Draw(manaTexture, tileMap[4, 4].Rectangle, Color.White);
                tileMap[4, 4].Item = mana;
            }
            else
            {
                tileMap[4, 4].Item = null;
            }

            for (int i = 0; i < gridSize; ++i)
            {
                for (int j = 0; j < gridSize; ++j)
                {
                    if (playerTurn)
                    {
                        //hover highlight
                        if (tileMap[i, j].Rectangle.Contains(mouseX, mouseY))
                        {
                            if (tileMap[i, j].IsLegal)
                            {
                                _spriteBatch.Draw(tileTexture[0], tileMap[i, j].Rectangle, null, Color.Blue * 0.5f);
                            }
                            //mouseclick movement
                            if (mouseState.LeftButton == ButtonState.Pressed && tileMap[i, j].IsLegal)
                            {
                                tileMap[P1.PosX, P1.PosY].Character = null;
                                P1.PosX = i;
                                P1.PosY = j;
                                tileMap[P1.PosX, P1.PosY].Character = P1;
                                playerTurn = false;
                                //pickup item
                                if (tileMap[i, j].hasItem())
                                {
                                    P1.pickUpItem(tileMap[i, j].Item);
                                }

                            }
                            //attack enemy
                            else if(mouseState.LeftButton == ButtonState.Pressed && tileMap[i, j].hasCharacter())
                            {
                                P1.Attack(tileMap[i,j].Character);
                                playerTurn = false;
                            }
                        }
                    }
                }
            }
            _spriteBatch.DrawString(font, "Actions:", new Vector2(670, 10), Color.White);
            _spriteBatch.Draw(heartTexture, new Vector2(670, 30), Color.White);
            _spriteBatch.DrawString(font, "" + P1.Hp, new Vector2(690, 30), Color.White);
            _spriteBatch.DrawString(font, "Mana: " + P1.Mana, new Vector2(670, 50), Color.White);
            _spriteBatch.DrawString(font, "DMG: " + P1.Dmg, new Vector2(670, 70), Color.White);
            _spriteBatch.DrawString(font, "range: " + P1.Range, new Vector2(670, 90), Color.White);
            _spriteBatch.DrawString(font, "Inventory: ", new Vector2(770, 10), Color.White);
            // Inventory
            int invY = 30;
            int itemDmg = 5;
            int itemMana = 5;
            foreach (Item item in P1.Inventory)
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


        }
        public void createObstacle(int posX,int posY)
        {
            _spriteBatch.Draw(obstTexture, tileMap[posX, posY].Rectangle, Color.White);
            tileMap[posX, posY].IsLegal = false;
        }
        private void drawEnemy()
        {
            foreach (Enemy e in Enemies)
            {
                if (e.isDead())
                {
                    tileMap[e.PosX, e.PosY].Character = null;
                    tileMap[e.PosX, e.PosY].IsLegal = true;
                }
                else
                {
                    _spriteBatch.Draw(enemyTexture, tileMap[e.PosX, e.PosY].Rectangle, Color.White);
                    _spriteBatch.DrawString(font, "HP:" + e.Hp, new Vector2(tileMap[e.PosX, e.PosY].Rectangle.X, tileMap[e.PosX, e.PosY].Rectangle.Y), Color.White);
                    tileMap[e.PosX, e.PosY].Character = e;
                    tileMap[e.PosX, e.PosY].IsLegal = false;
                }
            }
        }
        private void createEnemy(string name,int posX,int posY)
        {
            Enemies.Add(new Enemy(name, posX, posY));
        }
              */
        }
    }
}
