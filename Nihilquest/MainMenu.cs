using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using System.Drawing;
using Microsoft.Xna.Framework.Input;
using Nihilquest;

namespace Nihilquest
{
    class MainMenu
    {
        enum GameState
        {
            mainMenu,
            loadGame,
            creators,
            inGame,
            exit
        }

        GameState gameState;

        private bool isKeyPressed = false;

        List<GUIElement> main = new List<GUIElement>();

        public MainMenu()
        {
            main.Add(new GUIElement("bg"));
            main.Add(new GUIElement("button_start"));
        }

        public void LoadContent(ContentManager content)
        {
            foreach (GUIElement element in main)
            {
                element.LoadContent(content);
                element.CenterElement(Game1.windowWidth, Game1.windowHeight);
                element.clickEvent += onClick;
            }

            main.Find(x => x.AssetName == "button_start").MoveElement(0, -200);
            gameState = GameState.inGame;
        }

        public void Update()
        {

            switch (gameState)
            {
                case GameState.mainMenu:
                    foreach (GUIElement element in main)
                    {
                        element.Update();
                    }
                    if (Keyboard.GetState().IsKeyDown(Keys.Escape) && !isKeyPressed)
                    {
                        gameState = GameState.inGame;
                        isKeyPressed = true;
                    }
                    if (Keyboard.GetState().IsKeyUp(Keys.Escape)) isKeyPressed = false;
                    break;
                case GameState.loadGame:
                    break;
                case GameState.creators:
                    break;
                case GameState.inGame:
                    if (Keyboard.GetState().IsKeyDown(Keys.Escape) && !isKeyPressed)
                    {
                        gameState = GameState.mainMenu;
                        isKeyPressed = true;
                    }
                    if (Keyboard.GetState().IsKeyUp(Keys.Escape)) isKeyPressed = false;
                    break;
                case GameState.exit:
                    System.Environment.Exit(0);
                    break;
                default:
                    break;
            }

            foreach (GUIElement element in main)
            {
                element.Update();
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            switch (gameState)
            {
                case GameState.mainMenu:
                    foreach (GUIElement element in main)
                    {
                        element.Draw(spriteBatch);
                    }
                    break;
                case GameState.loadGame:
                    break;
                case GameState.creators:
                    break;
                case GameState.inGame:
                    break;
                default:
                    break;
            }


        }

        public void onClick(string element)
        {
            if (element == "button_start")
            {
                gameState = GameState.inGame;
            }
            if (element == "bg")
            {
                //gameState = GameState.exit;
            }
        }
    }
}
