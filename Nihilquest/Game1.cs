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
        Texture2D heartTexture;
        Texture2D manaUITexture;
        Texture2D damageUITexture;
        Texture2D rangeUITexture;

        private Room[,] roomMap;
        private RoomGeneration rg;

        private int playerRoomX;
        private int playerRoomY;

        private int eIndex;


        public static int windowWidth = 960;
        public static int windowHeight = 640;

        private Item mana = new Item("Mana flask", 1,2);
        private Item sword = new Item("Sword", 1,1);
        private Item health = new Item("Health", 8,8);
        private Item halfMana = new Item("Half Mana", 8,7);
        private Item halfHealth = new Item("Half Health", 8,1);

        MainMenu main = new MainMenu();

        private bool playerTurn = true;
        private SpriteFont font;
        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            playerRoomX = 1;
            playerRoomY = 1;
            eIndex = 0;
            rg = new RoomGeneration();
            rg.generateRoom();
            roomMap = rg.Level;
            for (int ඞ = 0; ඞ < roomMap.Length; ඞ++)
			{
                for (int j = 0; j < roomMap.Length; j++)
			    {
                    if (roomMap[ඞ, j] != null)
                    {
                        roomMap[ඞ, j].generateTileMap();
                        roomMap[ඞ, j].createWalls();
                        roomMap[ඞ, j].createDoors();
                        if (roomMap[ඞ, j].IsStart)
                        {
                            playerRoomX = ඞ;
                            playerRoomY = j;
                        }
                    }

			    }
            }
            sword.AddDmg = 5;
            mana.AddMana = 10;
            health.AddHealth = 10;
            halfHealth.AddHealth = 5;
            halfMana.AddMana = 5;
            createItem(sword);
            createItem(mana);
            createItem(health);
            createItem(halfHealth);
            createItem(halfMana);
            createEnemy("mob1", 5, 6);
            createEnemy("mob1", 5, 7);
            roomMap[playerRoomX, playerRoomY].Player = new Player("Wairen", 5, 5);
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
            sword.Texture = this.Content.Load<Texture2D>("weapon_knife");
            mana.Texture = this.Content.Load<Texture2D>("flask_big_blue");
            health.Texture = this.Content.Load<Texture2D>("heart_full");
            halfHealth.Texture = this.Content.Load<Texture2D>("heart_half");
            halfMana.Texture = this.Content.Load<Texture2D>("small_flask_blue");
            heartTexture = this.Content.Load<Texture2D>("ui_heart_full");
            manaUITexture = this.Content.Load<Texture2D>("ui_flask_blue");
            damageUITexture = this.Content.Load<Texture2D>("ui_damage");
            rangeUITexture = this.Content.Load<Texture2D>("ui_range");
            font = Content.Load<SpriteFont>("Text");
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
                        _spriteBatch.Draw(playerTexture, roomMap[playerRoomX, playerRoomY].TileMap[roomMap[playerRoomX, playerRoomY].Player.PosX, roomMap[playerRoomX, playerRoomY].Player.PosY].Rectangle, Color.White);
                        //tilemap rendering
                        if (roomMap[playerRoomX, playerRoomY].TileMap[i, j].IsWall)
                        {
                            _spriteBatch.Draw(obstTexture, roomMap[playerRoomX, playerRoomY].TileMap[i, j].Rectangle, Color.White);
                        }
                        else if (roomMap[playerRoomX, playerRoomY].TileMap[i, j].IsDoor)
                        {
                            _spriteBatch.Draw(tileTexture[4], roomMap[playerRoomX, playerRoomY].TileMap[i, j].Rectangle, Color.White);
                        }
                        else
                        {
                            _spriteBatch.Draw(tileTexture[0], roomMap[playerRoomX, playerRoomY].TileMap[i, j].Rectangle, Color.White);
                        }
                        if (playerTurn)
                        {
                            eIndex = 0;
                            //hover highlight
                            if (roomMap[playerRoomX, playerRoomY].TileMap[i, j].Rectangle.Contains(mouseX, mouseY) && Math.Abs(roomMap[playerRoomX, playerRoomY].Player.PosX - i) <= roomMap[playerRoomX, playerRoomY].Player.Range && Math.Abs(roomMap[playerRoomX, playerRoomY].Player.PosY - j) <= roomMap[playerRoomX, playerRoomY].Player.Range)
                            {
                                if (roomMap[playerRoomX, playerRoomY].TileMap[i, j].IsLegal)
                                {
                                    _spriteBatch.Draw(tileTexture[0], roomMap[playerRoomX, playerRoomY].TileMap[i, j].Rectangle, null, Color.Blue * 0.5f);
                                }
                                //mouseclick movement
                                if (mouseState.LeftButton == ButtonState.Pressed)
                                {
                                    if (roomMap[playerRoomX, playerRoomY].TileMap[i, j].IsLegal && Math.Abs(roomMap[playerRoomX, playerRoomY].Player.PosX - i) <= roomMap[playerRoomX, playerRoomY].Player.Range && Math.Abs(roomMap[playerRoomX, playerRoomY].Player.PosY - j) <= roomMap[playerRoomX, playerRoomY].Player.Range)
                                    {
                                        roomMap[playerRoomX, playerRoomY].TileMap[roomMap[playerRoomX, playerRoomY].Player.PosX, roomMap[playerRoomX, playerRoomY].Player.PosY].Character = null;
                                        roomMap[playerRoomX, playerRoomY].Player.PosX = i;
                                        roomMap[playerRoomX, playerRoomY].Player.PosY = j;
                                        roomMap[playerRoomX, playerRoomY].TileMap[roomMap[playerRoomX, playerRoomY].Player.PosX, roomMap[playerRoomX, playerRoomY].Player.PosY].Character = roomMap[playerRoomX, playerRoomY].Player;
                                        playerTurn = false;
                                        //pickup item
                                        if (roomMap[playerRoomX, playerRoomY].TileMap[i, j].hasItem())
                                        {
                                            roomMap[playerRoomX, playerRoomY].Player.pickUpItem(roomMap[playerRoomX, playerRoomY].TileMap[i, j].Item);
                                        }
                                        if (roomMap[playerRoomX, playerRoomY].TileMap[i, j].IsDoor)
                                        {
                                            //TOP
                                            if ((i == 4 || i == 5) && j == 0)
                                            {
                                                if (playerRoomY + 1 < rg.MapSize - 1)
                                                {
                                                    if (roomMap[playerRoomX, playerRoomY + 1] != null)
                                                    {
                                                        roomMap[playerRoomX, playerRoomY + 1].Player = roomMap[playerRoomX, playerRoomY].Player;
                                                        playerRoomY++;
                                                        roomMap[playerRoomX, playerRoomY].Player.PosX = 4;
                                                        roomMap[playerRoomX, playerRoomY].Player.PosY = 8;
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
                                                        playerRoomX--;
                                                        roomMap[playerRoomX, playerRoomY].Player.PosX = 8;
                                                        roomMap[playerRoomX, playerRoomY].Player.PosY = 4;
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
                                                        playerRoomY--;
                                                        roomMap[playerRoomX, playerRoomY].Player.PosX = 4;
                                                        roomMap[playerRoomX, playerRoomY].Player.PosY = 1;
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
                                                        roomMap[playerRoomX, playerRoomY].Player.PosX = 1;
                                                        roomMap[playerRoomX, playerRoomY].Player.PosY = 4;
                                                        System.Diagnostics.Debug.WriteLine(playerRoomX + ":" + playerRoomY);
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    //attack enemy
                                    else if (roomMap[playerRoomX, playerRoomY].TileMap[i, j].hasCharacter())
                                    {
                                        roomMap[playerRoomX, playerRoomY].Player.Attack(roomMap[playerRoomX, playerRoomY].TileMap[i, j].Character);
                                        playerTurn = false;
                                    }
                                }
                            }
                        }
                        else
                        {

                            while(eIndex < roomMap[playerRoomX, playerRoomY].Enemies.Count)
                            {
                                if (Math.Abs(roomMap[playerRoomX, playerRoomY].Enemies[eIndex].PosX - roomMap[playerRoomX, playerRoomY].Player.PosX) <= roomMap[playerRoomX, playerRoomY].Enemies[eIndex].Range && Math.Abs(roomMap[playerRoomX, playerRoomY].Enemies[eIndex].PosX - roomMap[playerRoomX, playerRoomY].Player.PosY) <= roomMap[playerRoomX, playerRoomY].Enemies[eIndex].Range)
                                {
                                    roomMap[playerRoomX, playerRoomY].Enemies[eIndex].Attack(roomMap[playerRoomX, playerRoomY].Player);
                                    eIndex++;
                                }
                            }
                        }
                    }
                }
                //enemy drawing
                foreach (Enemy e in roomMap[playerRoomX, playerRoomY].Enemies)
                {
                    if (e.isDead())
                    {
                        roomMap[playerRoomX, playerRoomY].TileMap[e.PosX, e.PosY].Character = null;
                        roomMap[playerRoomX, playerRoomY].TileMap[e.PosX, e.PosY].IsLegal = true;
                        roomMap[playerRoomX, playerRoomY].Enemies.Remove(e);
                    }
                    else
                    {
                        _spriteBatch.Draw(enemyTexture, roomMap[playerRoomX, playerRoomY].TileMap[e.PosX, e.PosY].Rectangle, Color.White);
                        _spriteBatch.DrawString(font, "HP:" + e.Hp, new Vector2(roomMap[playerRoomX, playerRoomY].TileMap[e.PosX, e.PosY].Rectangle.X, roomMap[playerRoomX, playerRoomY].TileMap[e.PosX, e.PosY].Rectangle.Y), Color.White);

                    }
                }
                //item drawing
                foreach (Item i in roomMap[playerRoomX, playerRoomY].Items)
                {
                    if (!roomMap[playerRoomX, playerRoomY].Player.isInInventory(i))
                    {
                        _spriteBatch.Draw(i.Texture, roomMap[playerRoomX, playerRoomY].TileMap[i.PosX, i.PosY].Rectangle, Color.White);
                    }
                    else
                    {
                        roomMap[playerRoomX, playerRoomY].TileMap[i.PosX, i.PosY].Item = null;
                    }

                }
            }

            _spriteBatch.DrawString(font, "Stats:", new Vector2(670, 10), Color.White);
            _spriteBatch.Draw(heartTexture, new Vector2(670, 30), Color.White);
            _spriteBatch.DrawString(font, "" + roomMap[playerRoomX, playerRoomY].Player.Hp, new Vector2(690, 30), Color.White);
            _spriteBatch.Draw(manaUITexture, new Vector2(670, 50), Color.White);
            _spriteBatch.DrawString(font, "" + roomMap[playerRoomX, playerRoomY].Player.Mana, new Vector2(690, 50), Color.White);
            _spriteBatch.Draw(damageUITexture, new Vector2(672, 70), Color.White);
            _spriteBatch.DrawString(font, "" + roomMap[playerRoomX, playerRoomY].Player.Dmg, new Vector2(690, 70), Color.White);
            _spriteBatch.Draw(rangeUITexture, new Vector2(670, 90), Color.White);
            _spriteBatch.DrawString(font, "" + roomMap[playerRoomX, playerRoomY].Player.Range, new Vector2(690, 90), Color.White);
            _spriteBatch.DrawString(font, "Inventory: ", new Vector2(770, 10), Color.White);
            // Inventory
            int invY = 30;
            foreach (Item item in roomMap[playerRoomX, playerRoomY].Player.Inventory)
            {
                    _spriteBatch.DrawString(font, "" + item.ItemName, new Vector2(770, invY), Color.White);
                    invY += 20;
            }
            main.Draw(_spriteBatch);
            _spriteBatch.End();
            base.Draw(gameTime);
        }
        public void createObstacle(int posX, int posY)
        {
            _spriteBatch.Draw(obstTexture, roomMap[playerRoomX, playerRoomY].TileMap[posX, posY].Rectangle, Color.White);
            roomMap[playerRoomX, playerRoomY].TileMap[posX, posY].IsLegal = false;
        }
        private void createEnemy(string name, int posX, int posY)
        {
            Enemy e = new Enemy(name, posX, posY);
            roomMap[playerRoomX, playerRoomY].Enemies.Add(e);
            roomMap[playerRoomX, playerRoomY].TileMap[e.PosX, e.PosY].Character = e;
            roomMap[playerRoomX, playerRoomY].TileMap[e.PosX, e.PosY].IsLegal = false;
        }
        private void createItem(Item i)
        {
            roomMap[playerRoomX, playerRoomY].Items.Add(i);
            roomMap[playerRoomX, playerRoomY].TileMap[i.PosX, i.PosY].Item = i;
        }
    }
}

