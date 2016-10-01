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
    class Bullet_Med: Bullet_Abstract // Bullet 2 is like bullet one except it can move in any direction 
        // I know this should be done using abstraction but for demonstration purposes I will simply onherit from bullet for now
    {
        protected double xSpeed;                                                               // Distance in pixels that the bullet will shift each draw()
        protected double ySpeed;

        public Bullet_Med(ContentManager Content, Vector2 Position, double xSpeed, double ySpeed)           // Constructor takes texture (let's us create different types of bullets from player class)
        {
            this.Content = Content;
            texture = ContentMaster.bullet_tiny;
            this.xSpeed = xSpeed;
            this.ySpeed = ySpeed;
            position = Position;
            isVisible = true;
            hitBox = new Rectangle((int)position.X, (int)position.Y, texture.Width, texture.Height);
            this.power = 20;
        }

        public override void Update(GameTime gameTime)
        {
            // Could implement Rotation here

            position.Y = position.Y - (float)ySpeed;                                         // Move bullet upwards by (speed) pixels
            position.X = position.X - (float)xSpeed;
            hitBox.Location = new Vector2(position.X, position.Y).ToPoint();                // Update hitbox location to match new bullet position

            if (position.Y < -50 || position.Y > ContentMaster.playableHeight || position.X < -50 || position.X > ContentMaster.playableWidth)                                                           // Set invisible to false if bullet is far off the top of the screen
            {
                isVisible = false;
            }

            hitBox.X = (int)position.X;
            hitBox.Y = (int)position.Y;

        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, Color.White);                               // Draw the bullet at it's position
        }
    }
}

