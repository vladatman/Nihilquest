using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

/// <summary>
/// boss healthbar class
/// </summary>
namespace Nihilquest
{
    /// <summary>
    /// private fields for boss healthbar class
    /// </summary>
    private Texture2D container, lifebar;
    public Vector2 position;
    public int fullHealth;
    public int currentHealth;
    public Healthbar(ContentManager content)
    {
        LoadContent(content);
        fullHealth = lifebar.Width;
        currentHealth = fullHealth;
    }

    /// <summary>
    /// Loading in the conent
    /// </summary>
    private void LoadContent(ContentManager content)
    {
        container = content.Load<Texture2D>("health1px");
        lifebar = content.Load<Texture2D>("Health100px");

    }

    /// <summary>
    /// check keyboard presses upon player attack
    /// </summary>
    public void Update()
    {
        if (Keyboard.GetState().IsKeyDown(Keys.Right))
            position.X += 3;
        if (Keyboard.GetState().IsKeyDown(Keys.Left))
            position.X -= 3;
    }

    /// <summary>
    /// Draw a new rectangle for boss hp
    /// </summary>
    public void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(container, position, Color.Red);
        spriteBatch.Draw(lifebar, position, new Rectangle((int)position.X, (int)position.Y, currentHealth, lifebar.Height), Color.Pink);
    }
}
