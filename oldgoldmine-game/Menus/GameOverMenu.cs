﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace oldgoldmine_game.Menus
{
    class GameOverMenu : Menu
    {

        public override void Initialize(GraphicsDevice device, Texture2D background,
            SpriteFont font, Texture2D normalButton, Texture2D highlightedButton)
        {
            
        }


        public override void Update(OldGoldMineGame application)
        {
            
        }

        public override void Draw(GraphicsDevice screen)
        {
            screen.Clear(Color.DarkRed);
        }
    }
}
