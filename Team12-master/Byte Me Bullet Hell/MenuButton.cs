using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Byte_Me_Bullet_Hell
{
    class MenuButton
    {
        //did not declare in Game1 because these apply to button
        Texture2D buttonTexture;
        Vector2 buttonPosition;
        Rectangle buttonRectangle;
        private bool currentHover = false;
        Color buttonColor = new Color(255, 255, 255, 255);

        public Vector2 size;

        public MenuButton(Texture2D newButtonTexture, GraphicsDevice graphics)
        {
            buttonTexture = newButtonTexture;

            //ScreenW = 800, ScreenH = 600a
            //ImgW = 100, ImgH = 20
            size = new Vector2(newButtonTexture.Width, newButtonTexture.Height);

        }

        bool down;
        public bool isClicked;

        public void Update(MouseState mouse)
        {
            buttonRectangle = new Rectangle((int)buttonPosition.X, (int)buttonPosition.Y, (int)size.X, (int)size.Y);
            
            Rectangle mouseRectangle = new Rectangle(mouse.X, mouse.Y, 1, 1);

            if (mouseRectangle.Intersects(buttonRectangle))
            {
                if (!currentHover)
                {
                    currentHover = true;
                    ContentMaster.SM.playButtonHover();
                }

                if (buttonColor.A == 255) down = false;
                if (buttonColor.A == 0) down = true;
                if (down) buttonColor.A += 3; else buttonColor.A -= 3;
                if (mouse.LeftButton == ButtonState.Pressed) isClicked = true;

            }
            else if (buttonColor.A < 255)
            {
                buttonColor.A += 3;
                if (currentHover)
                {
                    currentHover = false;
                }
            }
            else if (currentHover)
            {
                currentHover = false;
            }
        }

        public void setPosition(Vector2 newButtonPosition)
        {
            buttonPosition = newButtonPosition;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(buttonTexture, buttonRectangle, buttonColor);
        }
    }
}
