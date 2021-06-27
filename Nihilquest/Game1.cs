using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Runtime;
using System.Collections.Generic;
using System.Xml.Linq;
using Microsoft.Xna.Framework.Audio;
using System.Diagnostics;

namespace Nihilquest
{
    internal class Game1 : Game
    {

        //System.Diagnostics.Debug.WriteLine(); write to console

        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        public static string appDataFilePath;

        //Textures
        Texture2D[] tileTexture = new Texture2D[8];
        Texture2D playerTexture;
        public static Texture2D enemyTexture;
        Texture2D bossTexture;
        Texture2D obstTexture;
        Texture2D damageUITexture;
        Texture2D rangeUITexture;
        Texture2D swordTexture;
        Texture2D healthTexture;
        Texture2D manaTexture;
        Texture2D halfHealthTexture;
        Texture2D halfManaTexture;
        Texture2D itemRoom;
        Texture2D bossRoom;
        Texture2D ladder;
        Texture2D mainUI;
        Texture2D staffOfDeath;
        Texture2D unendingHealing;
        Texture2D teleportDagger;
        Texture2D heavyAttack;
        
        Texture2D healthTexture2;
        Rectangle rectangle;

        public static Texture2D skeletonTexture;
        public static Texture2D swampTexture;
        public static Texture2D goblinTexture;
        private SpriteFont font;
        //SFX and Songs
        Song BGMstart;
        List<Song> BGMlist;
        List<SoundEffect> SFXlist;

        //Room 2D layout
        public static Room[,] roomMap;
        private RoomGeneration rg;

        //Which room the player is in
        private int playerRoomX;
        private int playerRoomY;

        //Miniap array
        public  static Room[,] exploredRooms;


        public static Player P;
        public static int currentLevel;

        //index for enemy attacking
        private int eIndex;

        //prevents rooms from spawning more than one item after death
        private bool itemDrop = true;


        //decides if the player has died
        public static bool playerRestart = true;
        //prevents key holding
        private bool isKeyPressed;
        MouseState mouseState;
        private bool mouseClick = false;

        //resolution
        public static int windowWidth = 960;
        public static int windowHeight = 640;

        private ActiveItem activeItemTest = new ActiveItem("Active", 8, 2, 1);



        MainMenu main;

        private bool playerTurn = true;

        //decides if players can leave room in the boss room
        public static bool canLeave = true;


        public Game1()
        {
            currentLevel = 1;
            appDataFilePath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            BGMlist = new List<Song>();
            SFXlist = new List<SoundEffect>();

            playerRoomX = 1;
            playerRoomY = 1;
            isKeyPressed=false;
            P = new Player("Wairen", 5, 5);
            eIndex = 0;
            rg = new RoomGeneration();
            rg.generateRoom();
            roomMap = rg.Level;

            IsMouseVisible = true;

            main = new MainMenu();
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
            //loading in textures
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
            swordTexture = this.Content.Load<Texture2D>("weapon_knife");
            manaTexture = this.Content.Load<Texture2D>("flask_big_blue");
            healthTexture = this.Content.Load<Texture2D>("heart_full");
            halfHealthTexture = this.Content.Load<Texture2D>("heart_half");
            halfManaTexture = this.Content.Load<Texture2D>("small_flask_blue");
            activeItemTest.Texture = this.Content.Load<Texture2D>("flask_big_blue");
            damageUITexture = this.Content.Load<Texture2D>("ui_damage");
            rangeUITexture = this.Content.Load<Texture2D>("ui_range");
            bossRoom = this.Content.Load<Texture2D>("skull");
            itemRoom = this.Content.Load<Texture2D>("chest_full_open_anim_f2");
            ladder = this.Content.Load<Texture2D>("floor_ladder");
            font = this.Content.Load<SpriteFont>("Text");
            staffOfDeath = this.Content.Load<Texture2D>("weapon_red_magic_staff");
            unendingHealing = this.Content.Load<Texture2D>("flask_big_red");
            skeletonTexture = this.Content.Load<Texture2D>("skelet_idle_anim_f0");
            swampTexture = this.Content.Load<Texture2D>("swampy_idle_anim_f0");
            goblinTexture = this.Content.Load<Texture2D>("goblin_run_anim_f0");
            teleportDagger = this.Content.Load<Texture2D>("teleportdagger");
            heavyAttack = this.Content.Load<Texture2D>("heavyattack");
            mainUI = this.Content.Load<Texture2D>("UI");
            //healthTexture2 = this.Content.Load<Texture2D>("Health1px");
            healthTexture2 = this.Content.Load<Texture2D>("Health100px");

            //loading in songs and SFX
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
            //starting music
            MediaPlayer.Volume = 0.03f;
            MediaPlayer.Play(BGMstart);
            //generating rooms and special rooms
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
                        if (roomMap[i, j].IsItem == true)
                        {
                            createActiveItem(i, j, 4, 5);
                        }
                    }

                }
            }
            //adding player to the starting room
            roomMap[playerRoomX, playerRoomY].Player = P;
            //setting minimap
            exploredRooms = new Room[rg.MapSize, rg.MapSize];

            main.LoadContent(Content);


        }
        //if (roomMap[playerRoomX, playerRoomY].Player.ActiveItem != null && !canLeave && roomMap[playerRoomX, playerRoomY].Enemies.Count == 0)
        //{
        //    roomMap[playerRoomX, playerRoomY].Player.ActiveItem.EndItem(roomMap, playerRoomX, playerRoomY);
        //}
        protected override void Update(GameTime gameTime)
        {
            //randomized music
            Random rand = new Random();
            if (MediaPlayer.State != MediaState.Playing)
            {
                MediaPlayer.Play(BGMlist[rand.Next(BGMlist.Count)]);
            }

            // Check for keypress to activate special item ability
            if (Keyboard.GetState().IsKeyDown(Keys.Space) && !isKeyPressed)
	        {
                if (playerTurn)
                {
                    if (roomMap[playerRoomX, playerRoomY].Player.ActiveItem != null)
                    {
                        roomMap[playerRoomX, playerRoomY].Player.ActiveItem.Ability(roomMap, playerRoomX, playerRoomY);
                    }
                }
                isKeyPressed = true;
	        }
            if (Keyboard.GetState().IsKeyUp(Keys.Space)) isKeyPressed = false;
            //resets everything if player dies and opens menu
            if (roomMap[playerRoomX, playerRoomY].Player.isDead())
            {
                main.gameState = GameState.mainMenu;
                newLevel(true);
                playerRestart = false;
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

            //doors opening and closing depending on enemies in the room
            if (roomMap[playerRoomX, playerRoomY].Enemies.Count == 0)
            {
                canLeave = true;
                if (itemDrop && roomMap[playerRoomX, playerRoomY].TileMap[4,4].IsLegal && roomMap[playerRoomX, playerRoomY].Boss == null)
                {
                    createItem(playerRoomX, playerRoomY, 4, 4);
                }
            }
            else
            {
                canLeave = false;
            }
            if (roomMap[playerRoomX, playerRoomY].IsBoss == true)
            {
                if (roomMap[playerRoomX, playerRoomY].Boss.isDead())
                {
                    canLeave = true;
                    if (itemDrop)
                    {
                        createItem(playerRoomX, playerRoomY, 4, 4);
                    }
                }
                else
                {
                    canLeave = false;
                }
            }
            if (canLeave)
            {
                if (roomMap[playerRoomX, playerRoomY].Player.ActiveItem != null)
                {
                    if(roomMap[playerRoomX, playerRoomY].Player.ActiveItem.InUse)
                    {
                        roomMap[playerRoomX, playerRoomY].Player.ActiveItem.EndItem(roomMap, playerRoomX, playerRoomY);
                    }
                }
                itemDrop = false;
            }
            else
            {
                itemDrop = true;
            }


            //Update Rectangle
            //rectangle = new Rectangle(100, 100, Boss.Hp, healthTexture2.Height);
                

            main.Update();

            base.Update(gameTime);
        }

        //TODO: Refactor the draw method
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            _spriteBatch.Begin(SpriteSortMode.Deferred,
              BlendState.AlphaBlend,
              SamplerState.PointClamp,
              null, null, null, null);
            int mouseX = Mouse.GetState().X;
            int mouseY = Mouse.GetState().Y;
            //minimap
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
                        //tilemap rendering depending on type of tile
                        if (roomMap[playerRoomX, playerRoomY].TileMap[i, j].IsWall)
                        {
                           _spriteBatch.Draw(obstTexture, roomMap[playerRoomX, playerRoomY].TileMap[i, j].Rectangle, Color.White);
                        }
                        else if (roomMap[playerRoomX, playerRoomY].TileMap[i, j].IsDoor)
                        {
                            if(canLeave)
                            {
                                _spriteBatch.Draw(tileTexture[4], roomMap[playerRoomX, playerRoomY].TileMap[i, j].Rectangle, Color.White);
                                roomMap[playerRoomX, playerRoomY].TileMap[i, j].IsLegal = true;
                            }
                            else
                            {
                                _spriteBatch.Draw(obstTexture, roomMap[playerRoomX, playerRoomY].TileMap[i, j].Rectangle, Color.White);
                                roomMap[playerRoomX, playerRoomY].TileMap[i, j].IsLegal = false;
                            }
                           
                        }
                        else if (roomMap[playerRoomX, playerRoomY].TileMap[i, j].IsExit)
                        {
                            _spriteBatch.Draw(ladder, roomMap[playerRoomX, playerRoomY].TileMap[i, j].Rectangle, Color.White);
                        }
                        else
                        {
                            _spriteBatch.Draw(tileTexture[0], roomMap[playerRoomX, playerRoomY].TileMap[i, j].Rectangle, Color.White);
                        }
                        if (roomMap[playerRoomX, playerRoomY].TileMap[i, j].hasItem())
                        {
                            _spriteBatch.Draw(roomMap[playerRoomX, playerRoomY].TileMap[i, j].Item.Texture, roomMap[playerRoomX, playerRoomY].TileMap[i, j].Rectangle, Color.White);
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
                                        //item pickup
                                        if (roomMap[playerRoomX, playerRoomY].TileMap[i, j].hasItem())
                                        {
                                            soundEffectInstance = SFXlist[3].CreateInstance();
                                            soundEffectInstance.Volume = 0.1f;
                                            soundEffectInstance.Play();
                                            roomMap[playerRoomX, playerRoomY].Player.pickUpItem(roomMap[playerRoomX, playerRoomY].TileMap[i, j].Item);
                                            roomMap[playerRoomX, playerRoomY].TileMap[i, j].Item = null;
                                            roomMap[playerRoomX, playerRoomY].Items.Remove(roomMap[playerRoomX, playerRoomY].TileMap[i, j].Item);
                                            playerTurn = false;
                                        }
                                        //leaving room
                                        if (roomMap[playerRoomX, playerRoomY].TileMap[i, j].IsDoor && canLeave)
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
                                        //entering new level
                                        if (roomMap[playerRoomX, playerRoomY].TileMap[i, j].IsExit)
                                        {
                                            newLevel(false);
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
                        //enemies attack queue
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
                            //boss attack
                            if(roomMap[playerRoomX, playerRoomY].IsBoss == true && roomMap[playerRoomX, playerRoomY].Boss.isDead() == false)
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

                //UI rendering
                _spriteBatch.Draw(mainUI, new Rectangle(640, 0, 320, 640), Color.White);

                //enemy drawing
                for (int e = 0; e < roomMap[playerRoomX, playerRoomY].Enemies.Count; e++)
                {
                    if (roomMap[playerRoomX, playerRoomY].Enemies[e].isDead())
                    {
                        roomMap[playerRoomX, playerRoomY].TileMap[roomMap[playerRoomX, playerRoomY].Enemies[e].PosX, roomMap[playerRoomX, playerRoomY].Enemies[e].PosY].Character = null;
                        roomMap[playerRoomX, playerRoomY].TileMap[roomMap[playerRoomX, playerRoomY].Enemies[e].PosX, roomMap[playerRoomX, playerRoomY].Enemies[e].PosY].IsLegal = true;
                        roomMap[playerRoomX, playerRoomY].Enemies.Remove(roomMap[playerRoomX, playerRoomY].Enemies[e]);
                        if(roomMap[playerRoomX, playerRoomY].Player.Mana < roomMap[playerRoomX, playerRoomY].Player.MaxMana) roomMap[playerRoomX, playerRoomY].Player.Mana += roomMap[playerRoomX, playerRoomY].Player.ManaRegen;
                    }
                    else
                    {
                        _spriteBatch.Draw(roomMap[playerRoomX, playerRoomY].Enemies[e].Texture, roomMap[playerRoomX, playerRoomY].TileMap[roomMap[playerRoomX, playerRoomY].Enemies[e].PosX, roomMap[playerRoomX, playerRoomY].Enemies[e].PosY].Rectangle, Color.White);
                        _spriteBatch.DrawString(font, "HP:" + roomMap[playerRoomX, playerRoomY].Enemies[e].Hp, new Vector2(roomMap[playerRoomX, playerRoomY].TileMap[roomMap[playerRoomX, playerRoomY].Enemies[e].PosX, roomMap[playerRoomX, playerRoomY].Enemies[e].PosY].Rectangle.X, roomMap[playerRoomX, playerRoomY].TileMap[roomMap[playerRoomX, playerRoomY].Enemies[e].PosX, roomMap[playerRoomX, playerRoomY].Enemies[e].PosY].Rectangle.Y), Color.White);

                    }
                }

                //boss drawing
                if(roomMap[playerRoomX, playerRoomY].IsBoss == true)
                {
                    if(roomMap[playerRoomX, playerRoomY].Boss.isDead() == false)
                    {
                        _spriteBatch.Draw(bossTexture, roomMap[playerRoomX, playerRoomY].TileMap[roomMap[playerRoomX, playerRoomY].Boss.PosX, roomMap[playerRoomX, playerRoomY].Boss.PosY].Rectangle, Color.White);

                        //_spriteBatch.DrawString(font, "HP:" + roomMap[playerRoomX, playerRoomY].Boss.Hp, new Vector2(roomMap[playerRoomX, playerRoomY].TileMap[roomMap[playerRoomX, playerRoomY].Boss.PosX, roomMap[playerRoomX, playerRoomY].Boss.PosY].Rectangle.X, roomMap[playerRoomX, playerRoomY].TileMap[roomMap[playerRoomX, playerRoomY].Boss.PosX, roomMap[playerRoomX, playerRoomY].Boss.PosY].Rectangle.Y), Color.White);

                        // bossHealthBarTexture red rectangle

                        _spriteBatch.Draw(healthTexture2, new Vector2(roomMap[playerRoomX, playerRoomY].TileMap[roomMap[playerRoomX, playerRoomY].Boss.PosX, roomMap[playerRoomX, playerRoomY].Boss.PosY].Rectangle.X - 20, roomMap[playerRoomX, playerRoomY].TileMap[roomMap[playerRoomX, playerRoomY].Boss.PosX, roomMap[playerRoomX, playerRoomY].Boss.PosY].Rectangle.Y - 5), Color.White);

                        //_spriteBatch.Draw(healthTexture2, rectangle, Color.White);

                        //rectangle = new Rectangle(100, 100, healthTexture2.Width, healthTexture2.Height);

                        //rectangle = new Rectangle(100, 100, healthTexture2.Width, healthTexture2.Height);

                        //_spriteBatch.Draw(healthTexture2, rectangle, Color.White);
                    }
                    else
                    {
                        roomMap[playerRoomX, playerRoomY].TileMap[roomMap[playerRoomX, playerRoomY].Boss.PosX, roomMap[playerRoomX, playerRoomY].Boss.PosY].Character = null;
                        roomMap[playerRoomX, playerRoomY].TileMap[roomMap[playerRoomX, playerRoomY].Boss.PosX, roomMap[playerRoomX, playerRoomY].Boss.PosY].IsLegal = true;
                        roomMap[playerRoomX, playerRoomY].TileMap[5, 5].IsExit = true;
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
            //Stats rendering
            _spriteBatch.DrawString(font, "Stats:", new Vector2(670, 10), Color.White);
            _spriteBatch.Draw(healthTexture, new Vector2(670, 30), Color.White);
            _spriteBatch.DrawString(font, "" + roomMap[playerRoomX, playerRoomY].Player.Hp, new Vector2(690, 30), Color.White);
            _spriteBatch.Draw(halfManaTexture, new Vector2(670, 50), Color.White);
            _spriteBatch.Draw(activeItemTest.Texture, new Vector2(670, 50), Color.White);
            _spriteBatch.DrawString(font, "" + roomMap[playerRoomX, playerRoomY].Player.Mana, new Vector2(690, 50), Color.White);
            _spriteBatch.Draw(damageUITexture, new Vector2(672, 70), Color.White);
            _spriteBatch.DrawString(font, "" + roomMap[playerRoomX, playerRoomY].Player.Dmg, new Vector2(690, 70), Color.White);
            _spriteBatch.Draw(rangeUITexture, new Vector2(670, 90), Color.White);
            _spriteBatch.DrawString(font, "" + roomMap[playerRoomX, playerRoomY].Player.Range, new Vector2(690, 90), Color.White);

            _spriteBatch.DrawString(font, "Inventory: ", new Vector2(770, 10), Color.White);
            _spriteBatch.DrawString(font, "Activate Item: Space", new Vector2(770, 100), Color.White);
            // Inventory

            int invY = 30;
            foreach (Item item in roomMap[playerRoomX, playerRoomY].Player.Inventory)
            {
                if (item is ActiveItem)
                {
                    _spriteBatch.DrawString(font, "" + item.ItemName, new Vector2(770, invY), Color.White);
                    invY += 20;
                }
            }

            main.Draw(_spriteBatch);
            _spriteBatch.End();
            base.Draw(gameTime);
        }
        //creating objects
        private void createBoss(string name, int roomX, int roomY)
        {
            Boss b = new Boss(name, 4, 4);
            roomMap[roomX, roomY].Boss = b;
            roomMap[roomX, roomY].TileMap[b.PosX, b.PosY].Character = b;
            roomMap[roomX, roomY].TileMap[b.PosX, b.PosY].IsLegal = false;
        }
        private void createItem(int roomX,int roomY,int posX,int posY)
        {
            Item item = new Item();
            switch (new Random().Next(3))
            {
                case 0:
                    item = new Item("Mana flask", posX, posY);
                    item.Texture = manaTexture;
                    item.AddMana = 10;
                    item.AddDmg = 0;
                    item.AddHealth = 0;
                    break;
                case 1:
                    item = new Item("Sword", posX, posY);
                    item.Texture = swordTexture;
                    item.AddMana = 0;
                    item.AddDmg = 3;
                    item.AddHealth = 0;
                    break;
                case 2:
                    item = new Item("Health", posX, posY);
                    item.Texture = healthTexture;
                    item.AddMana = 0;
                    item.AddDmg = 0;
                    item.AddHealth = 30;
                    break;
                case 3:
                    item = new Item("Half Mana", posX, posY);
                    item.Texture = halfManaTexture;
                    item.AddMana = 5;
                    item.AddDmg = 0;
                    item.AddHealth = 0;
                    break;
            }
            if (roomMap[roomX, roomY].TileMap[item.PosX, item.PosY].Item == null)
            {
                roomMap[roomX, roomY].Items.Add(item);
                roomMap[roomX, roomY].TileMap[item.PosX, item.PosY].Item = item;
            }

        }
        private void createActiveItem(int roomX, int roomY, int posX, int posY)
        {
            ActiveItem item = new ActiveItem();
            switch (2)
            {
                case 0:
                    item = new ActiveItem("Staff of Death", posX, posY, 1);
                    item.Texture = staffOfDeath;
                    break;
                case 1:
                    item = new ActiveItem("Infinite Potion", posX, posY, 2);
                    item.Texture = unendingHealing;
                    break;
                case 2:
                    item = new ActiveItem("Teleport Dagger", posX, posY, 3);
                    item.Texture = teleportDagger;
                    break;
                case 3:
                    item = new ActiveItem("Heavy Attack", posX, posY, 4);
                    item.Texture = heavyAttack;
                    break;

            }
            if (roomMap[roomX, roomY].TileMap[item.PosX, item.PosY].Item == null)
            {
                roomMap[roomX, roomY].Items.Add(item);
                roomMap[roomX, roomY].TileMap[item.PosX, item.PosY].Item = item;
            }

        }
        //checks if 2D array contains object
        private bool Contains2D(Object[,] array, Object obj)
        {
            for (int x = 0; x < array.GetLongLength(0); x++)
            {
                for (int y = 0; y < array.GetLongLength(1); y++)
                {
                    if (array != null && array[x, y] == obj)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        //generates new level , depending if the player has dies or not
        private void newLevel(bool isRestart)
        {
            Player temp;
            if (isRestart)
            {
                temp = new Player();
                currentLevel = 1;
            }
            else
            {
                temp = roomMap[playerRoomX, playerRoomY].Player;
                currentLevel++;
            }

            exploredRooms = new Room[rg.MapSize, rg.MapSize];
            rg = new RoomGeneration();
            rg.generateRoom();
            roomMap = rg.Level;
            itemDrop = true;
            canLeave = true;
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
                        if (roomMap[i, j].IsItem == true)
                        {
                            createActiveItem(i, j, 4, 5);
                        }
                    }

                }
            }
            roomMap[playerRoomX, playerRoomY].Player = temp;
        }
        /*
        Based on the A(*)(A-star) pathfinding algorithm
        It assigns either 14 or 10 depending if the related cell is diagnoal to target and/or starting point
        G is starting point
        H is target point
        F is the sum of G and H
        it returns the coordinates of the cell with the smallest value around the starting point that is a legal move
         */
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
