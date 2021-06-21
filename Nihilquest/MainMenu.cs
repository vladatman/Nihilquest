using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using System.Drawing;
using Microsoft.Xna.Framework.Input;
using System.Xml.Linq;

namespace Nihilquest
{
    class MainMenu
    {
        enum GameState
        {
            mainMenu,
            inGame,
            exit
        }

        GameState gameState;

        private bool isKeyPressed = false;
        public Save save;
        private XDocument xml;



        List<GUIElement> main = new List<GUIElement>();

        public MainMenu()
        {
            main.Add(new GUIElement("bg"));
            main.Add(new GUIElement("button_play"));
            main.Add(new GUIElement("button_load"));
            main.Add(new GUIElement("button_save"));
            main.Add(new GUIElement("button_exit"));
        }

        public void LoadContent(ContentManager content)
        {
            foreach (GUIElement element in main)
            {
                element.LoadContent(content);
                element.CenterElement(Game1.windowWidth, Game1.windowHeight);
                element.clickEvent += onClick;
            }

            main.Find(x => x.AssetName == "button_play").MoveElement(0, -200);
            main.Find(x => x.AssetName == "button_load").MoveElement(0, -50);
            main.Find(x => x.AssetName == "button_save").MoveElement(0, 100);
            main.Find(x => x.AssetName == "button_exit").MoveElement(0, 250);
            gameState = GameState.mainMenu;

            save = new Save(1, "Nihilquest");
            

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
                case GameState.inGame:
                    break;
                default:
                    break;
            }


        }

        public void onClick(string element)
        {
            if (gameState == GameState.mainMenu)
            {
                if (element == "button_play")
                {
                    gameState = GameState.inGame;
                }

                if (element == "button_load")
                {
                    xml = save.GetFile("XML\\nihilquestSave.xml");
                    if (xml != null)
                    {
                        //Load player stats
                        Game1.P.Name = xml.Element("Player").Element("name").Value;
                        Game1.P.PosX = Convert.ToInt32(xml.Element("Player").Element("PosX").Value);
                        Game1.P.PosY = Convert.ToInt32(xml.Element("Player").Element("PosY").Value);
                        Game1.P.Dmg = Convert.ToInt32(xml.Element("Player").Element("Dmg").Value);
                        Game1.P.Hp = Convert.ToInt32(xml.Element("Player").Element("Hp").Value);
                        Game1.P.Mana = Convert.ToInt32(xml.Element("Player").Element("Mana").Value);
                        Game1.P.Range = Convert.ToInt32(xml.Element("Player").Element("Range").Value);
                        Game1.P.ManaRegen = Convert.ToInt32(xml.Element("Player").Element("ManaRegen").Value);
                    }
                }

                if (element == "button_save")
                {

                    XDocument xml = new XDocument(
                                                new XElement("Player",
                                                    new XElement("name", Game1.P.Name),
                                                    new XElement("PosX", Game1.P.PosX),
                                                    new XElement("PosY", Game1.P.PosY),
                                                    new XElement("Dmg", Game1.P.Dmg),
                                                    new XElement("Hp", Game1.P.Hp),
                                                    new XElement("Mana", Game1.P.Mana),
                                                    new XElement("Range", Game1.P.Range),
                                                    new XElement("ManaRegen", Game1.P.ManaRegen))
                                                );

                    save.HandleSaveFormates(xml, "nihilquestSave.xml");
                }

                if (element == "button_exit")
                {
                    gameState = GameState.exit;
                }
            }
        }
    }
}
