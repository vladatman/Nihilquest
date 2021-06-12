using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace NihilQuest
{
    public class RoomGeneration
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        public static int screenWidth;
        public static int screenHeight;
        private Texture2D rectangle;

        public void generateRoom()
        {
            // Initial coordinates for spawning rooms
            int x = 600;
            int y = 350;

            Random rnd = new Random();
            // Last direction in which a room was generated
            int last = 1;
            // Amount of rooms to generate
            int roomsToGenerate = 5;

            // Generate random number for each special room
            int mainRoom = rnd.Next(roomsToGenerate);
            int endRoom = rnd.Next(roomsToGenerate);
            int itemRoom = rnd.Next(roomsToGenerate);

            while ((endRoom == mainRoom) || (endRoom == itemRoom))
            {
                endRoom = rnd.Next(roomsToGenerate);
            }
            while ((itemRoom == mainRoom) || (itemRoom == endRoom))
            {
                itemRoom = rnd.Next(roomsToGenerate);
            }

            // Check if a special room has already been generated
            bool mainRoomFlag = false;
            bool itemRoomFlag = false;
            bool endRoomFlag = false;

            int roomsGenerated = 0;

            // Store X and Y coordinates in two lists
            List<int> xCoord = new List<int>();
            List<int> yCoord = new List<int>();

            while (roomsGenerated < roomsToGenerate)
            {
                bool create = true;
                int value = rnd.Next(1, 5);
                if (value != last)
                {
                    switch (value)
                    {
                        case 1:
                            y -= 50;
                            break;
                        case 2:
                            x += 50;
                            break;
                        case 3:
                            y += 50;
                            break;
                        case 4:
                            x -= 50;
                            break;
                    }
                    for (int j = 0; j < xCoord.Count; j++)
                    {
                        if ((xCoord[j] == x) && (yCoord[j] == y))
                        {
                            create = false;
                        }
                    }
                    if (create)
                    {
                        if ((roomsGenerated == mainRoom) && (mainRoomFlag == false))
                        {
                            _spriteBatch.Begin();
                            _spriteBatch.Draw(rectangle, new Rectangle(x, y, 50, 50), Color.Green);
                            mainRoomFlag = true;
                            xCoord.Add(x);
                            yCoord.Add(y);
                            _spriteBatch.End();
                            roomsGenerated++;
                        }
                        else
                        {
                            if ((roomsGenerated == itemRoom) && (itemRoomFlag == false))
                            {
                                _spriteBatch.Begin();
                                _spriteBatch.Draw(rectangle, new Rectangle(x, y, 50, 50), Color.Blue);
                                itemRoomFlag = true;
                                xCoord.Add(x);
                                yCoord.Add(y);
                                _spriteBatch.End();
                                roomsGenerated++;
                            }
                            else
                            {
                                if ((roomsGenerated == endRoom) && (endRoomFlag == false))
                                {
                                    _spriteBatch.Begin();
                                    _spriteBatch.Draw(rectangle, new Rectangle(x, y, 50, 50), Color.Red);
                                    xCoord.Add(x);
                                    yCoord.Add(y);
                                    _spriteBatch.End();
                                    roomsGenerated++;
                                    endRoomFlag = true;
                                }
                                else
                                {
                                    _spriteBatch.Begin();
                                    _spriteBatch.Draw(rectangle, new Rectangle(x, y, 50, 50), Color.White);
                                    xCoord.Add(x);
                                    yCoord.Add(y);
                                    _spriteBatch.End();
                                    roomsGenerated++;
                                }
                            }
                        }
                        last = value;
                    }
                }
            }
        }
    }
}