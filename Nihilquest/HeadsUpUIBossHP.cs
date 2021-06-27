using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Nihilquest.Components;



/// <summary>
/// class represnting a heads up feature for enemy gs
/// </summary>
namespace Nihilquest
{

    public class HUD : GUIElement
    {

        Player player;

        Texture2D HealthBar;
        Texture2D HealthBarPositive;
        Texture2D HealthBarNegative;
        Texture2D ManaBar;
        Texture2D ManaBarPositive;
        Texture2D ManaBarNegative;
        Texture2D ExpBar;
        Texture2D ExpBarPositive;

        int CurrentHealth = 100;
        int CurrentMana = 45;
        int CurrentExp = 0;

        public HUD(Game game, GameStateManager manager)
            : base(game, manager)
        {
            player = new Player(game);
        }

        public void LoadContent()
        {
            base.LoadContent();

            HealthBar = Game.Content.Load<Texture2D>(@"GUI\healthBar");
            HealthBarPositive = Game.Content.Load<Texture2D>(@"GUI\healthBarPositive");
            HealthBarNegative = Game.Content.Load<Texture2D>(@"GUI\healthBarNegative");
            ManaBar = Game.Content.Load<Texture2D>(@"GUI\manaBar");
            ManaBarPositive = Game.Content.Load<Texture2D>(@"GUI\manaBarPositive");
            ManaBarNegative = Game.Content.Load<Texture2D>(@"GUI\manaBarNegative");
            ExpBar = Game.Content.Load<Texture2D>(@"GUI\expBar");
            ExpBarPositive = Game.Content.Load<Texture2D>(@"GUI\expBarPositive");
        }

        /// <summary>
        /// Input handler
        /// </summary>
        public void Update(GameTime gameTime)
        {
            if (InputHandler.KeyDown(Keys.F1))
            {
                CurrentHealth += 1;
            }

            if (InputHandler.KeyDown(Keys.F2))
            {
                CurrentHealth -= 1;
            }

            if (InputHandler.KeyDown(Keys.F3))
            {
                CurrentMana += 1;
            }

            if (InputHandler.KeyDown(Keys.F4))
            {
                CurrentMana -= 1;
            }

            if (InputHandler.KeyDown(Keys.F5))
            {
                CurrentExp += 1;
            }

            if (InputHandler.KeyDown(Keys.F6))
            {
                CurrentExp -= 1;
            }

            CurrentHealth = (int)MathHelper.Clamp(CurrentHealth, 0, 100);
            CurrentMana = (int)MathHelper.Clamp(CurrentMana, 0, 45);
            CurrentExp = (int)MathHelper.Clamp(CurrentExp, 0, 500);
        }

        /// <summary>
        /// Drawing rectangles for updating boss hp
        /// </summary>
        public void Draw(GameTime gameTime)
        {

            GameRef.SpriteBatch.Draw(
                HealthBarNegative,
                new Rectangle((int)player.Camera.Position.X + 150, (int)player.Camera.Position.Y + 630, 150, 15),
                Color.White);

            GameRef.SpriteBatch.Draw(
                HealthBarPositive,
                new Rectangle((int)player.Camera.Position.X + 150, (int)player.Camera.Position.Y + 630, 150 * (int)CurrentHealth / 100, 15),
                Color.White);

            GameRef.SpriteBatch.Draw(
                HealthBar,
                new Rectangle((int)player.Camera.Position.X + 150, (int)player.Camera.Position.Y + 630, 150, 15),
                Color.White);

            GameRef.SpriteBatch.Draw(
                ManaBarNegative,
                new Rectangle((int)player.Camera.Position.X + 150, (int)player.Camera.Position.Y + 650, 150, 15),
                Color.White);

            GameRef.SpriteBatch.Draw(
                ManaBarPositive,
                new Rectangle((int)player.Camera.Position.X + 150, (int)player.Camera.Position.Y + 650, 150 * (int)CurrentMana / 45, 15),
                Color.White);

            GameRef.SpriteBatch.Draw(
                ManaBar,
                new Rectangle((int)player.Camera.Position.X + 150, (int)player.Camera.Position.Y + 650, 150, 15),
                Color.White);

            GameRef.SpriteBatch.Draw(
                ExpBarPositive,
                new Rectangle((int)player.Camera.Position.X + 10, (int)player.Camera.Position.Y + 680, 1260 * (int)CurrentExp / 500, 15),
                Color.White);

            GameRef.SpriteBatch.Draw(
                ExpBar,
                new Rectangle((int)player.Camera.Position.X + 10, (int)player.Camera.Position.Y + 680, 1260, 15),
                Color.White);
        }
    }
}