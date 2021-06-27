using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;

namespace Nihilquest
{
    class slideBar
    {
        public GUIElement bar;
        public GUIElement slider;
        public int lowerLimitX;
        public int upperLimitX;
        private int inputX;
        private int inputY;

        public slideBar(int inputX, int inputY)
        {
            bar = new GUIElement("volbar");
            slider = new GUIElement("slider");
            this.inputX = inputX;
            this.inputY = inputY;
        }

        public void LoadContent(ContentManager content)
        {
            bar.AssetName = "volbar";
            slider.AssetName = "slider";
            bar.LoadContent(content);
            slider.LoadContent(content);

            bar.CenterElement(Game1.windowWidth, Game1.windowHeight);
            bar.Y += inputY;
            bar.X += inputX;

            lowerLimitX = bar.X - bar.Width;
            upperLimitX = bar.X + bar.Width;

            
            slider.X += lowerLimitX+(int)(Convert.ToDouble(MediaPlayer.Volume) * 100 + bar.Width - 5);
            slider.Y = bar.Y-2;
            slider.clickEvent += onClick;
            bar.clickEvent += onClick;

        }

        public void Update()
        {
            
            bar.Update();
            slider.Update();

        }
        public void Draw(SpriteBatch spriteBatch)
        {

            bar.Draw(spriteBatch);
            slider.Draw(spriteBatch);
            spriteBatch.DrawString(Game1.font, "Music Volume: "+(int)(Convert.ToDouble(MediaPlayer.Volume) * 100), new Vector2(bar.X - 135,bar.Y-3),Color.White);
        }

        private void onClick(string element)
        {
            if (MainMenu.gameState == GameState.mainMenu)
            {
                MediaPlayer.Volume = ((float)Convert.ToDouble(Mouse.GetState().X - lowerLimitX)/100)-1;
                slider.X = lowerLimitX + (int)(Convert.ToDouble(MediaPlayer.Volume)*100+bar.Width-5);
            }
        }
        public void centerSliderbar()
        {
            bar.CenterElement(Game1.windowWidth, Game1.windowHeight);
        }

    }
}
