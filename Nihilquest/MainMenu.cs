using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using System.Drawing;
using Microsoft.Xna.Framework.Input;
using System.Xml.Linq;
using System.Linq;

namespace Nihilquest
{
    class MainMenu
    {

        public GameState gameState;

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
                        Game1.P.Name = xml.Element("Root").Element("Player").Element("name").Value;
                        Game1.P.PosX = Convert.ToInt32(xml.Element("Root").Element("Player").Element("PosX").Value);
                        Game1.P.PosY = Convert.ToInt32(xml.Element("Root").Element("Player").Element("PosY").Value);
                        Game1.P.Dmg = Convert.ToInt32(xml.Element("Root").Element("Player").Element("Dmg").Value);
                        Game1.P.Hp = Convert.ToInt32(xml.Element("Root").Element("Player").Element("Hp").Value);
                        Game1.P.Mana = Convert.ToInt32(xml.Element("Root").Element("Player").Element("Mana").Value);
                        Game1.P.Range = Convert.ToInt32(xml.Element("Root").Element("Player").Element("Range").Value);
                        Game1.P.ManaRegen = Convert.ToInt32(xml.Element("Root").Element("Player").Element("ManaRegen").Value);

                        //Load player inventory
                        Game1.P.Inventory.Clear();
                        List<XElement> ItemsList = xml.Element("Root").Element("Player").Descendants("Inventory").Descendants("Item").ToList();
                        foreach (XElement xe in ItemsList)   Game1.P.Inventory.Add(new Item(xe.Element("ItemName").Value));

                        //Loading current level
                        Game1.currentLevel = Convert.ToInt32(xml.Element("Root").Element("CurrentLevel").Value);

                        //Loading Rooms
                        List<XElement> xElementList = xml.Element("Root").Element("Player").Descendants("Inventory").Descendants("Item").ToList();
                        foreach (Room room in Game1.roomMap) {
                            
                        }
                    }
                }

                if (element == "button_save")
                {

                    XDocument xml = new XDocument(
                                            new XElement("Root",
                                                new XElement("Player",
                                                    new XElement("name", Game1.P.Name),
                                                    new XElement("PosX", Game1.P.PosX),
                                                    new XElement("PosY", Game1.P.PosY),
                                                    new XElement("Dmg", Game1.P.Dmg),
                                                    new XElement("Hp", Game1.P.Hp),
                                                    new XElement("Mana", Game1.P.Mana),
                                                    new XElement("Range", Game1.P.Range),
                                                    new XElement("ManaRegen", Game1.P.ManaRegen),
                                                    new XElement("Inventory", "")
                                                    ),
                                                new XElement("CurrentLevel", Game1.currentLevel),
                                                new XElement("Level", "")
                                                )
                                            );

                    foreach (Item item in Game1.P.Inventory)
                    {
                        xml.Element("Root").Element("Player").Element("Inventory").Add(new XElement("Item", new XElement("ItemName", item.ItemName)));
                    }

                    foreach (Room room in Game1.roomMap)
                    {
                        if (room != null)
                        {
                            if (room.IsBoss)
                            {
                                xml.Element("Root").Element("Level").Add(new XElement("Room",
                                                           new XElement("Boss",
                                                               new XElement("Name", room.Boss.Name),
                                                               new XElement("PosX", room.Boss.PosX),
                                                               new XElement("PosY", room.Boss.PosY),
                                                               new XElement("Dmg", room.Boss.Dmg),
                                                               new XElement("Hp", room.Boss.Hp),
                                                               new XElement("Range", room.Boss.Range)
                                                               ),
                                                           new XElement("IsStart", room.IsStart),
                                                           new XElement("IsBoss", room.IsBoss),
                                                           new XElement("IsItem", room.IsItem),
                                                           new XElement("Items", "")));
                            }
                            else {
                                xml.Element("Root").Element("Level").Add(new XElement("Room",
                                                               new XElement("Boss",""),
                                                               new XElement("IsStart", room.IsStart),
                                                               new XElement("IsBoss", room.IsBoss),
                                                               new XElement("IsItem", room.IsItem),
                                                               new XElement("Items", ""),
                                                               new XElement("Enemies", "")
                                                               
                                                               ));
                            }

                            if (room.Items.Count() != 0)
                            {
                                foreach (Item item in room.Items)
                                {
                                    xml.Element("Root").Element("Level").Element("Room").Element("Items").Add(new XElement("Item",
                                                                                                                    new XElement("ItemName", item.ItemName),
                                                                                                                    new XElement("PosX", item.PosX),
                                                                                                                    new XElement("PosY", item.PosY)
                                                                                                                    ));

                                }
                            }

                            if (room.Enemies.Count() != 0)
                            {
                                foreach (Enemy enemy in room.Enemies)
                                {
                                    xml.Element("Root").Element("Level").Element("Room").Element("Enemies").Add(new XElement("Enemy",
                                                                                                                       new XElement("Name", enemy.Name),
                                                                                                                       new XElement("PosX", enemy.PosX),
                                                                                                                       new XElement("PosY", enemy.PosY),
                                                                                                                       new XElement("Dmg", enemy.Dmg),
                                                                                                                       new XElement("Hp", enemy.Hp),
                                                                                                                       new XElement("Range", enemy.Range)
                                                                                                                    ));

                                }
                            }
                        } else xml.Element("Root").Element("Level").Add(new XElement("Room", ""));
                    }




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
