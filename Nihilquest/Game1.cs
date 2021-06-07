using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Nihilquest
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        Texture2D tileTexture;
        private int gridSize = 10;

        private bool playerTurn = true;

        private int charPosX = 0;
        private int charPosY = 0;

        Texture2D charTexture;
        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            _graphics.IsFullScreen = false;
            _graphics.PreferredBackBufferWidth = 640;  // set this value to the desired width of your window
            _graphics.PreferredBackBufferHeight = 640;   // set this value to the desired height of your window
            _graphics.ApplyChanges();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            tileTexture = this.Content.Load<Texture2D>("floorTexture");
            charTexture = this.Content.Load<Texture2D>("testchar");
            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            if (Keyboard.GetState().IsKeyDown(Keys.T))
            {
                playerTurn = true;
            }
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            int tileSize = 64;
            _spriteBatch.Begin();
            Rectangle[,] tileMap = new Rectangle[gridSize, gridSize];
            MouseState mouseState = Mouse.GetState();

            int mouseX = mouseState.X;
            int mouseY = mouseState.Y;
            for (int i = 0; i < gridSize; ++i)
            {
                for (int j = 0; j < gridSize; ++j)
                {
                    Rectangle rectangle = new Rectangle(i * tileSize, j * tileSize, tileSize, tileSize);
                    tileMap[i, j] = rectangle;
                    _spriteBatch.Draw(tileTexture, tileMap[i, j], Color.White);
                    if (tileMap[i, j].Contains(mouseX, mouseY) && playerTurn)
                    {
                        _spriteBatch.Draw(tileTexture, tileMap[i, j], Color.Gray);
                        if (mouseState.LeftButton == ButtonState.Pressed)
                        {
                            charPosX = i;
                            charPosY = j;
                            playerTurn = false;
                        }
                    }
                }
            }
            _spriteBatch.Draw(charTexture, tileMap[charPosX, charPosY], Color.White);
            _spriteBatch.End();
            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
