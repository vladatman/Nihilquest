using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Runtime;
using System.Collections.Generic;
using System.Xml.Linq;
using Microsoft.Xna.Framework.Audio;

namespace Nihilquest
{
    internal class Game1 : Game
    {

        //System.Diagnostics.Debug.WriteLine(); write to console

        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        public static string appDataFilePath;
        

        Texture2D[] tileTexture = new Texture2D[8];
        Texture2D playerTexture;
        Texture2D enemyTexture;
        Texture2D bossTexture;
        Texture2D obstTexture;
        Texture2D manaUITexture;
        Texture2D damageUITexture;
        Texture2D rangeUITexture;
        Texture2D itemRoom;
        Texture2D bossRoom;
        Texture2D ladder;
        Texture2D mainUI;
        Song BGMstart;
        List<Song> BGMlist;
        List<SoundEffect> SFXlist;

        public Room[,] roomMap;
        private RoomGeneration rg;
        public Room[,] exploredRooms;

        private int playerRoomX;
        private int playerRoomY;
        public static Player P;

        private int eIndex;

        MouseState mouseState;

        public static int windowWidth = 960;
        public static int windowHeight = 640;

        private Item mana = new Item("Mana flask", 1, 2);
        private Item sword = new Item("Sword", 1, 1);
        private Item health = new Item("Health", 8, 8);
        private Item halfMana = new Item("Half Mana", 8, 7);
        private Item halfHealth = new Item("Half Health", 8, 1);

        private bool mouseClick = false;


        MainMenu main = new MainMenu();

        private bool playerTurn = true;
        private SpriteFont font;
        private bool canLeave = true;
        public Game1()
        {

            appDataFilePath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            BGMlist = new List<Song>();
            SFXlist = new List<SoundEffect>();

            playerRoomX = 1;
            playerRoomY = 1;


            P = new Player("Wairen", 5, 5);
            eIndex = 0;
            rg = new RoomGeneration();
            rg.generateRoom();
            roomMap = rg.Level;
            for (int i = 0; i < rg.MapSize; i++)
            {
                for (int j = 0; j < rg.MapSize; j++)
                {
                    if (roomMap[i, j] != null)
                    {
                        roomMap[i, j].generateTileMap();
                        roomMap[i, j].createWalls();
                        roomMap[i, j].createDoors(roomMap,i,j);
                        if (roomMap[i, j].IsStart)
                        {
                            playerRoomX = i;
                            playerRoomY = j;
                        }
                        if (roomMap[i, j].IsBoss == true)
                        {
                            createBoss("boss1", i, j);
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

            roomMap[playerRoomX, playerRoomY].Player = P;
            exploredRooms = new Room[rg.MapSize, rg.MapSize];
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            _graphics.IsFullScreen = false;
            _graphics.PreferredBackBufferWidth = windowWidth;  // set this value to the desired width of your window
            _graphics.PreferredBackBufferHeight = windowHeight;   // set this value to the desired height of your window
            _graphics.ApplyChanges();
            mouseState = Mouse.GetState();
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
            bossTexture = this.Content.Load<Texture2D>("boss");
            obstTexture = this.Content.Load<Texture2D>("wall_mid");
            sword.Texture = this.Content.Load<Texture2D>("weapon_knife");
            mana.Texture = this.Content.Load<Texture2D>("flask_big_blue");
            health.Texture = this.Content.Load<Texture2D>("heart_full");
            halfHealth.Texture = this.Content.Load<Texture2D>("heart_half");
            halfMana.Texture = this.Content.Load<Texture2D>("small_flask_blue");
            damageUITexture = this.Content.Load<Texture2D>("ui_damage");
            rangeUITexture = this.Content.Load<Texture2D>("ui_range");
            bossRoom = this.Content.Load<Texture2D>("skull");
            itemRoom = this.Content.Load<Texture2D>("chest_full_open_anim_f2");
            ladder = this.Content.Load<Texture2D>("floor_ladder");
            font = this.Content.Load<SpriteFont>("Text");

            mainUI = this.Content.Load<Texture2D>("UI");

            BGMstart = this.Content.Load<Song>("songs/Invitation");
            BGMlist.Add(this.Content.Load<Song>("songs/Against All Odds"));
            BGMlist.Add(this.Content.Load<Song>("songs/Before the Dawn"));
            BGMlist.Add(this.Content.Load<Song>("songs/Fire in the Hole"));
            BGMlist.Add(this.Content.Load<Song>("songs/Gone Fishing"));
            BGMlist.Add(this.Content.Load<Song>("songs/Hopeful Feeling"));
            BGMlist.Add(this.Content.Load<Song>("songs/Point Zero"));
            BGMlist.Add(this.Content.Load<Song>("songs/Shelf Space"));
            BGMlist.Add(this.Content.Load<Song>("songs/Singularity"));
            BGMlist.Add(this.Content.Load<Song>("songs/Tonal Dissonance"));

            SFXlist.Add(this.Content.Load<SoundEffect>("sfx/Hit damage"));
            SFXlist.Add(this.Content.Load<SoundEffect>("sfx/FootstepPlayer"));
            SFXlist.Add(this.Content.Load<SoundEffect>("sfx/Ouch"));
            SFXlist.Add(this.Content.Load<SoundEffect>("sfx/PickupStat"));

            MediaPlayer.Volume = 0.03f;
            MediaPlayer.Play(BGMstart);

            main.LoadContent(Content);
            

        }

        protected override void Update(GameTime gameTime)
        {

            Random rand = new Random();
            if (MediaPlayer.State != MediaState.Playing)
            {
                MediaPlayer.Play(BGMlist[rand.Next(BGMlist.Count)]);
            }

            if (Keyboard.GetState().IsKeyDown(Keys.T))
            {
                playerTurn = true;
            }

            
            if (mouseState.LeftButton == ButtonState.Released && Mouse.GetState().LeftButton == ButtonState.Pressed && main.gameState != GameState.mainMenu)
            {
                mouseClick = true;
            }
            else
            {
                mouseClick = false;
            }
            mouseState = Mouse.GetState();
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
            int mouseX = Mouse.GetState().X;
            int mouseY = Mouse.GetState().Y;
            
            if (!Contains2D(exploredRooms, roomMap[playerRoomX, playerRoomY]))
            {
                exploredRooms[playerRoomX, playerRoomY] = roomMap[playerRoomX, playerRoomY];
            }
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
                        else if (roomMap[playerRoomX, playerRoomY].TileMap[i, j].IsExit)
                        {
                            _spriteBatch.Draw(ladder, roomMap[playerRoomX, playerRoomY].TileMap[i, j].Rectangle, Color.White);
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
                                if (roomMap[playerRoomX, playerRoomY].TileMap[i, j].IsLegal || roomMap[playerRoomX, playerRoomY].TileMap[i, j].hasCharacter())
                                {
                                    _spriteBatch.Draw(tileTexture[0], roomMap[playerRoomX, playerRoomY].TileMap[i, j].Rectangle, null, Color.Aqua * 0.5f);
                                }
                                //mouseclick movement
                                if (mouseClick)
                                {
                                    if (roomMap[playerRoomX, playerRoomY].TileMap[i, j].IsLegal && Math.Abs(roomMap[playerRoomX, playerRoomY].Player.PosX - i) <= roomMap[playerRoomX, playerRoomY].Player.Range && Math.Abs(roomMap[playerRoomX, playerRoomY].Player.PosY - j) <= roomMap[playerRoomX, playerRoomY].Player.Range)
                                    {
                                        SoundEffectInstance soundEffectInstance = SFXlist[1].CreateInstance();
                                        soundEffectInstance.Play();
                                        roomMap[playerRoomX, playerRoomY].TileMap[roomMap[playerRoomX, playerRoomY].Player.PosX, roomMap[playerRoomX, playerRoomY].Player.PosY].Character = null;
                                        roomMap[playerRoomX, playerRoomY].Player.PosX = i;
                                        roomMap[playerRoomX, playerRoomY].Player.PosY = j;
                                        roomMap[playerRoomX, playerRoomY].TileMap[roomMap[playerRoomX, playerRoomY].Player.PosX, roomMap[playerRoomX, playerRoomY].Player.PosY].Character = roomMap[playerRoomX, playerRoomY].Player;
                                        playerTurn = false;
                                        //pickup item
                                        if (roomMap[playerRoomX, playerRoomY].TileMap[i, j].hasItem())
                                        {
                                            soundEffectInstance = SFXlist[3].CreateInstance();
                                            soundEffectInstance.Volume = 0.1f;
                                            soundEffectInstance.Play();
                                            roomMap[playerRoomX, playerRoomY].Player.pickUpItem(roomMap[playerRoomX, playerRoomY].TileMap[i, j].Item);
                                        }
                                        if (roomMap[playerRoomX, playerRoomY].TileMap[i, j].IsDoor && roomMap[playerRoomX, playerRoomY].Enemies.Count == 0 && canLeave)
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
                                                    }
                                                }
                                            }
                                            //LEFT
                                            if ((j == 4 || j == 5) && i == 0)
                                            {
                                                if (playerRoomX - 1 >= 0)
                                                {
                                                    if (roomMap[playerRoomX - 1, playerRoomY] != null)
                                                    {
                                                        roomMap[playerRoomX - 1, playerRoomY].Player = roomMap[playerRoomX, playerRoomY].Player;
                                                        playerRoomX--;
                                                        roomMap[playerRoomX, playerRoomY].Player.PosX = 8;
                                                        roomMap[playerRoomX, playerRoomY].Player.PosY = 4;
                                                    }
                                                }
                                            }
                                            //BOTTOM
                                            if ((i == 4 || i == 5) && j == 9)
                                            {
                                                if (playerRoomY - 1 >= 0)
                                                {
                                                    if (roomMap[playerRoomX, playerRoomY - 1] != null)
                                                    {
                                                        roomMap[playerRoomX, playerRoomY - 1].Player = roomMap[playerRoomX, playerRoomY].Player;
                                                        playerRoomY--;
                                                        roomMap[playerRoomX, playerRoomY].Player.PosX = 4;
                                                        roomMap[playerRoomX, playerRoomY].Player.PosY = 1;
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
                                                    }
                                                }
                                            }
                                        }
                                        if (roomMap[playerRoomX, playerRoomY].TileMap[i, j].IsExit)
                                        {
                                            newLevel();
                                        }
                                    }
                                    //attack enemy
                                    else if (roomMap[playerRoomX, playerRoomY].TileMap[i, j].hasCharacter())
                                    {
                                        SoundEffectInstance soundEffectInstance = SFXlist[2].CreateInstance();
                                        soundEffectInstance.Volume = 0.1f;
                                        soundEffectInstance.Play();
                                        roomMap[playerRoomX, playerRoomY].Player.Attack(roomMap[playerRoomX, playerRoomY].TileMap[i, j].Character);
                                        playerTurn = false;
                                    }
                                }
                            }
                        }
                        //enemies attack
                        else
                        {
                            while (eIndex < roomMap[playerRoomX, playerRoomY].Enemies.Count)
                            {
                                
                                if (!playerTurn && Math.Abs(roomMap[playerRoomX, playerRoomY].Enemies[eIndex].PosX - roomMap[playerRoomX, playerRoomY].Player.PosX) <= roomMap[playerRoomX, playerRoomY].Enemies[eIndex].Range && Math.Abs(roomMap[playerRoomX, playerRoomY].Enemies[eIndex].PosY - roomMap[playerRoomX, playerRoomY].Player.PosY) <= roomMap[playerRoomX, playerRoomY].Enemies[eIndex].Range)
                                {
                                    roomMap[playerRoomX, playerRoomY].Enemies[eIndex].Attack(roomMap[playerRoomX, playerRoomY].Player);
                                    eIndex++;
                                }
                                else if(Math.Abs(roomMap[playerRoomX, playerRoomY].Enemies[eIndex].PosX - roomMap[playerRoomX, playerRoomY].Player.PosX) >= roomMap[playerRoomX, playerRoomY].Enemies[eIndex].Range || Math.Abs(roomMap[playerRoomX, playerRoomY].Enemies[eIndex].PosY - roomMap[playerRoomX, playerRoomY].Player.PosY) >= roomMap[playerRoomX, playerRoomY].Enemies[eIndex].Range)
                                {
                                    int[] newPoint = Pathfinding(roomMap[playerRoomX, playerRoomY].Enemies[eIndex], roomMap[playerRoomX, playerRoomY].TileMap);
                                    roomMap[playerRoomX, playerRoomY].TileMap[roomMap[playerRoomX, playerRoomY].Enemies[eIndex].PosX, roomMap[playerRoomX, playerRoomY].Enemies[eIndex].PosY].Character = null;
                                    roomMap[playerRoomX, playerRoomY].TileMap[roomMap[playerRoomX, playerRoomY].Enemies[eIndex].PosX, roomMap[playerRoomX, playerRoomY].Enemies[eIndex].PosY].IsLegal = true;
                                    roomMap[playerRoomX, playerRoomY].Enemies[eIndex].PosX = newPoint[0];
                                    roomMap[playerRoomX, playerRoomY].Enemies[eIndex].PosY = newPoint[1];
                                    roomMap[playerRoomX, playerRoomY].TileMap[roomMap[playerRoomX, playerRoomY].Enemies[eIndex].PosX, roomMap[playerRoomX, playerRoomY].Enemies[eIndex].PosY].Character = roomMap[playerRoomX, playerRoomY].Enemies[eIndex];
                                    roomMap[playerRoomX, playerRoomY].TileMap[roomMap[playerRoomX, playerRoomY].Enemies[eIndex].PosX, roomMap[playerRoomX, playerRoomY].Enemies[eIndex].PosY].IsLegal = false;
                                    eIndex++;
                                } 
                                else
                                {
                                    eIndex = roomMap[playerRoomX, playerRoomY].Enemies.Count;
                                }

                            }
                            if(roomMap[playerRoomX, playerRoomY].IsBoss == true)
                            {
                                if (Math.Abs(roomMap[playerRoomX, playerRoomY].Boss.PosX - roomMap[playerRoomX, playerRoomY].Player.PosX) >= roomMap[playerRoomX, playerRoomY].Boss.Range || Math.Abs(roomMap[playerRoomX, playerRoomY].Boss.PosY - roomMap[playerRoomX, playerRoomY].Player.PosY) >= roomMap[playerRoomX, playerRoomY].Boss.Range)
                                {
                                    int[] newPoint = Pathfinding(roomMap[playerRoomX, playerRoomY].Boss, roomMap[playerRoomX, playerRoomY].TileMap);
                                    roomMap[playerRoomX, playerRoomY].TileMap[roomMap[playerRoomX, playerRoomY].Boss.PosX, roomMap[playerRoomX, playerRoomY].Boss.PosY].Character = null;
                                    roomMap[playerRoomX, playerRoomY].TileMap[roomMap[playerRoomX, playerRoomY].Boss.PosX, roomMap[playerRoomX, playerRoomY].Boss.PosY].IsLegal = true;
                                    roomMap[playerRoomX, playerRoomY].Boss.PosX = newPoint[0];
                                    roomMap[playerRoomX, playerRoomY].Boss.PosY = newPoint[1];
                                    roomMap[playerRoomX, playerRoomY].TileMap[roomMap[playerRoomX, playerRoomY].Boss.PosX, roomMap[playerRoomX, playerRoomY].Boss.PosY].Character = roomMap[playerRoomX, playerRoomY].Boss;
                                    roomMap[playerRoomX, playerRoomY].TileMap[roomMap[playerRoomX, playerRoomY].Boss.PosX, roomMap[playerRoomX, playerRoomY].Boss.PosY].IsLegal = false;
                                }
                                else
                                {
                                    roomMap[playerRoomX, playerRoomY].Boss.Attack(roomMap[playerRoomX, playerRoomY].Player);
                                    SoundEffectInstance soundEffectInstance = SFXlist[2].CreateInstance();
                                    soundEffectInstance.Volume = 0.1f;
                                    soundEffectInstance.Play();
                                }
                            }
                            playerTurn = true;
                        }
                    }
                }
                _spriteBatch.Draw(mainUI, new Rectangle(640, 0, 320, 640), Color.White);
                //enemy drawing
                for (int e = 0; e < roomMap[playerRoomX, playerRoomY].Enemies.Count; e++)
                {
                    if (roomMap[playerRoomX, playerRoomY].Enemies[e].isDead())
                    {
                        roomMap[playerRoomX, playerRoomY].TileMap[roomMap[playerRoomX, playerRoomY].Enemies[e].PosX, roomMap[playerRoomX, playerRoomY].Enemies[e].PosY].Character = null;
                        roomMap[playerRoomX, playerRoomY].TileMap[roomMap[playerRoomX, playerRoomY].Enemies[e].PosX, roomMap[playerRoomX, playerRoomY].Enemies[e].PosY].IsLegal = true;
                        roomMap[playerRoomX, playerRoomY].Enemies.Remove(roomMap[playerRoomX, playerRoomY].Enemies[e]);
                    }
                    else
                    {
                        _spriteBatch.Draw(enemyTexture, roomMap[playerRoomX, playerRoomY].TileMap[roomMap[playerRoomX, playerRoomY].Enemies[e].PosX, roomMap[playerRoomX, playerRoomY].Enemies[e].PosY].Rectangle, Color.White);
                        _spriteBatch.DrawString(font, "HP:" + roomMap[playerRoomX, playerRoomY].Enemies[e].Hp, new Vector2(roomMap[playerRoomX, playerRoomY].TileMap[roomMap[playerRoomX, playerRoomY].Enemies[e].PosX, roomMap[playerRoomX, playerRoomY].Enemies[e].PosY].Rectangle.X, roomMap[playerRoomX, playerRoomY].TileMap[roomMap[playerRoomX, playerRoomY].Enemies[e].PosX, roomMap[playerRoomX, playerRoomY].Enemies[e].PosY].Rectangle.Y), Color.White);

                    }
                }
                //boss drawing
                if(roomMap[playerRoomX, playerRoomY].IsBoss == true)
                {
                    if(roomMap[playerRoomX, playerRoomY].Boss.isDead() == false)
                    {
                        _spriteBatch.Draw(bossTexture, roomMap[playerRoomX, playerRoomY].TileMap[roomMap[playerRoomX, playerRoomY].Boss.PosX, roomMap[playerRoomX, playerRoomY].Boss.PosY].Rectangle, Color.White);
                        _spriteBatch.DrawString(font, "HP:" + roomMap[playerRoomX, playerRoomY].Boss.Hp, new Vector2(roomMap[playerRoomX, playerRoomY].TileMap[roomMap[playerRoomX, playerRoomY].Boss.PosX, roomMap[playerRoomX, playerRoomY].Boss.PosY].Rectangle.X, roomMap[playerRoomX, playerRoomY].TileMap[roomMap[playerRoomX, playerRoomY].Boss.PosX, roomMap[playerRoomX, playerRoomY].Boss.PosY].Rectangle.Y), Color.White);
                        canLeave = false;
                    }
                    else
                    {
                        roomMap[playerRoomX, playerRoomY].TileMap[roomMap[playerRoomX, playerRoomY].Boss.PosX, roomMap[playerRoomX, playerRoomY].Boss.PosY].Character = null;
                        roomMap[playerRoomX, playerRoomY].TileMap[roomMap[playerRoomX, playerRoomY].Boss.PosX, roomMap[playerRoomX, playerRoomY].Boss.PosY].IsLegal = true;
                        roomMap[playerRoomX, playerRoomY].TileMap[5, 5].IsExit = true;
                        canLeave = true;
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
                //minimap drawing
                for (int x = 0; x < rg.MapSize; x++)
                {
                    for (int y = 0; y < rg.MapSize; y++)
                    {
                        if (exploredRooms[x, y] != null)
                        {
                            Rectangle rectangle = new Rectangle(700 + (x * 32), 550 + (-y * 32), 32, 32);
                            if (exploredRooms[x, y] == roomMap[playerRoomX, playerRoomY])
                            {
                                _spriteBatch.Draw(tileTexture[0], rectangle, Color.Aqua);
                            }
                            else
                            {
                                _spriteBatch.Draw(tileTexture[0], rectangle, Color.White);
                            }
                            if (exploredRooms[x, y].IsItem)
                            {
                                rectangle.Height = 16;
                                rectangle.Width = 16;
                                rectangle.X += 8;
                                rectangle.Y += 8;
                                _spriteBatch.Draw(itemRoom, rectangle, Color.White);
                            }
                            if (exploredRooms[x, y].IsBoss)
                            {
                                _spriteBatch.Draw(bossRoom, rectangle, Color.White);
                            }
                        }

                    }
                }
            }

            _spriteBatch.DrawString(font, "Stats:", new Vector2(670, 10), Color.White);
            _spriteBatch.Draw(health.Texture, new Vector2(670, 30), Color.White);
            _spriteBatch.DrawString(font, "" + roomMap[playerRoomX, playerRoomY].Player.Hp, new Vector2(690, 30), Color.White);
            _spriteBatch.Draw(halfMana.Texture, new Vector2(670, 50), Color.White);
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
        private void createBoss(string name, int roomX, int roomY)
        {
            Boss b = new Boss(name, 4, 4);
            roomMap[roomX, roomY].Boss = b;
            roomMap[roomX, roomY].TileMap[b.PosX, b.PosY].Character = b;
            roomMap[roomX, roomY].TileMap[b.PosX, b.PosY].IsLegal = false;
        }
        private void createItem(Item i)
        {
            roomMap[playerRoomX, playerRoomY].Items.Add(i);
            roomMap[playerRoomX, playerRoomY].TileMap[i.PosX, i.PosY].Item = i;
        }
        private bool Contains2D(Room[,] array, Room room)
        {
            for (int x = 0; x < rg.MapSize; x++)
            {
                for (int y = 0; y < rg.MapSize; y++)
                {
                    if (array != null && array[x, y] == room)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private void newLevel()
        {
            Player temp = roomMap[playerRoomX, playerRoomY].Player;
            exploredRooms = new Room[rg.MapSize, rg.MapSize];
            rg = new RoomGeneration();
            rg.generateRoom();
            roomMap = rg.Level;

            for (int i = 0; i < rg.MapSize; i++)
            {
                for (int j = 0; j < rg.MapSize; j++)
                {
                    if (roomMap[i, j] != null)
                    {
                        roomMap[i, j].generateTileMap();
                        roomMap[i, j].createWalls();
                        roomMap[i, j].createDoors(roomMap, i, j);
                        if (roomMap[i, j].IsStart)
                        {
                            playerRoomX = i;
                            playerRoomY = j;
                        }
                        if (roomMap[i, j].IsBoss == true)
                        {
                            createBoss("boss1", i, j);
                        }

                    }

                }
            }
            roomMap[playerRoomX, playerRoomY].Player = temp;
        }
        private int[] Pathfinding(Character E, Cell[,] tiles)
        {
            int playerX = roomMap[playerRoomX, playerRoomY].Player.PosX;
            int playerY = roomMap[playerRoomX, playerRoomY].Player.PosY;

            int gridSize = roomMap[playerRoomX, playerRoomY].GridSize;

            int enemyX = E.PosX;
            int enemyY = E.PosY;

            int G = 0;
            int H = 0;

            int nextX = 0;
            int nextY = 0;
            int tempF = 100000;
            for (int x = 0; x < gridSize; x++)
            {
                for (int y = 0; y < gridSize; y++)
                {
                    if (tiles[x, y].IsLegal && tiles[x, y].IsExit == false && tiles[x, y].IsDoor == false && tiles[x, y].hasItem() == false && tiles[x,y].hasCharacter() == false)
                    {
                        if (Math.Abs(enemyX - x) <= 1 && Math.Abs(enemyY - y) <= 1)
                        {
                            G = (Math.Abs(enemyX - x) + Math.Abs(enemyY - y)) / 2 * 14 + (Math.Abs(enemyX - x) + Math.Abs(enemyY - y)) % 2 * 10;
                            H = (Math.Abs(playerX - x) + Math.Abs(playerY - y)) / 2 * 14 + (Math.Abs(playerX - x) + Math.Abs(playerY - y)) % 2 * 10;
                            tiles[x, y].TileVal = G + H;
                            //System.Diagnostics.Debug.WriteLine(x+":"+y+" ["+tiles[x, y].F+"]");

                            if (tiles[x, y].TileVal < tempF)
                            {
                                tempF = tiles[x, y].TileVal;
                                nextX = x;
                                nextY = y;
                            }
                        }
                    }
                }
            }
            int[] next = new int[2];
            next[0] = nextX;
            next[1] = nextY;
            return next;
        }
    }
}

