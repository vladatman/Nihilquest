using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections;
using Microsoft.Xna.Framework.Content;
using System.Collections.Generic;

namespace Nihilquest
{
    class Room
    {
        private Player Player;
        private Cell[,] tileMap;
        private List<Enemy> Enemies;
        private int gridSize;
        private int tileSize;
        private bool playerTurn;

        internal Player Player1 { get => Player; set => Player = value; }
        internal Cell[,] TileMap { get => tileMap; set => tileMap = value; }
        internal List<Enemy> Enemies1 { get => Enemies; set => Enemies = value; }
        public int GridSize { get => gridSize; set => gridSize = value; }
        public int TileSize { get => tileSize; set => tileSize = value; }
        public bool PlayerTurn { get => playerTurn; set => playerTurn = value; }

        public Room(int gridSize,int tileSize, Cell[,] tileMap)
        {
            Player1 = new Player("Player",1,1);
            Enemies1 = new List<Enemy>();
            gridSize = gridSize;
            TileMap = tileMap;
            tileSize = tileSize;
            PlayerTurn = false;

        }

        public void Draw(GameTime gameTime, SpriteBatch _spriteBatch, Texture2D tileTexture, Texture2D playerTexture, Texture2D enemyTexture, Texture2D obstTexture)
        {

            MouseState mouseState = Mouse.GetState();

            int mouseX = mouseState.X;
            int mouseY = mouseState.Y;
            //player model rendering
            _spriteBatch.Draw(playerTexture, this.TileMap[Player1.PosX, Player1.PosY].Rectangle, Color.White);
            createEnemy("mob1", 6, 6, _spriteBatch, enemyTexture);
            for (int o = 0; o < this.GridSize; ++o)
            {
                createObstacle(o, 0, _spriteBatch, obstTexture);
                createObstacle(0, o, _spriteBatch, obstTexture);
                createObstacle(9, o, _spriteBatch, obstTexture);
                createObstacle(o, 9, _spriteBatch, obstTexture);
            }

            for (int i = 0; i < this.GridSize; ++i)
            {
                for (int j = 0; j < this.GridSize; ++j)
                {
                    if (PlayerTurn)
                    {
                        //hover highlight
                        if (this.TileMap[i, j].Rectangle.Contains(mouseX, mouseY))
                        {
                            // _spriteBatch.Draw(tileTexture, tileMap[i, j].Rectangle, null, Color.Blue, 0.0f, Vector2.Zero, SpriteEffects.None, 0);//layer depth template
                            _spriteBatch.Draw(tileTexture, this.TileMap[i, j].Rectangle, null, Color.Blue * 0.5f);
                            //mouseclick movement
                            if (mouseState.LeftButton == ButtonState.Pressed && this.TileMap[i, j].IsLegal)
                            {
                                this.TileMap[Player1.PosX, Player1.PosY].Character = null;
                                Player1.PosX = i;
                                Player1.PosY = j;
                                this.TileMap[Player1.PosX, Player1.PosY].Character = Player1;
                                PlayerTurn = false;
                            }
                            else if (mouseState.LeftButton == ButtonState.Pressed && this.TileMap[i, j].hasCharacter())
                            {
                                Player1.Attack(this.TileMap[i, j].Character);
                                PlayerTurn = false;
                            }
                        }
                    }
                }
            }
        }
        public void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.T))
            {
                PlayerTurn = true;
            }
        }
        public void createObstacle(int posX, int posY, SpriteBatch _spriteBatch, Texture2D obstTexture)
        {
            _spriteBatch.Draw(obstTexture, this.TileMap[posX, posY].Rectangle, Color.White);
            this.TileMap[posX, posY].IsLegal = false;
        }
        public void createEnemy(string name,int posX, int posY, SpriteBatch _spriteBatch,Texture2D enemyTexture)
        {
            Enemy E = new Enemy(name, posX, posY);
            this.Enemies1.Add(E);
            _spriteBatch.Draw(enemyTexture, this.TileMap[E.PosX, E.PosY].Rectangle, Color.White);
            this.TileMap[E.PosX, E.PosY].Character = E;
            this.TileMap[E.PosX, E.PosY].IsLegal = false;
        }

    }



}
