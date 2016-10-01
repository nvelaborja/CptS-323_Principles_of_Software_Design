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
    public class Bomb : Bullet_Abstract
    {
        // the elapsed amount of time the frame has been shown for
        float time;
        // duration of time to show each frame
        float frameTime = 0.05f;
        // an index of the current frame being shown
        int frameIndex;
        // total number of frames in our spritesheet
        const int totalFrames = 3;
        // define the size of our animation frame
        int frameHeight = 50;
        int frameWidth = 50;

        public Bomb(ContentManager Content, Vector2 Position, int bombSpeed)           // Constructor takes texture)
        {
            this.Content = Content;
            texture = ContentMaster.bomb;
            speed = bombSpeed;
            position = Position;
            isVisible = true;
            hitBox = new Rectangle((int)position.X, (int)position.Y, frameWidth*10 , frameHeight*10);
            power = 100; //change for damage
        }

        public override void Update(GameTime gameTime)
        {

            position.Y = position.Y - (float)speed;                                         // Move bomb upwards by (speed) pixels
            //hitBox.Location = new Vector2(position.X, position.Y).ToPoint();                // Update hitbox location to match new bomb position

            if (position.Y < -50 || position.Y > ContentMaster.playableHeight)                   // Set invisible to false if bomb is far off the top of the screen
            {
                isVisible = false;
            }

            hitBox.X = (int)position.X - 100;
            hitBox.Y = (int)position.Y;

            time += (float)gameTime.ElapsedGameTime.TotalSeconds;
            while (time > frameTime)
            {
                frameIndex++;
                time = 0f;
            }
            if (frameIndex > totalFrames) frameIndex = 1;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Rectangle source = new Rectangle(frameIndex * frameWidth, 0, frameWidth, frameHeight);

            Vector2 center = new Vector2(frameWidth / 2.0f, frameHeight);

            spriteBatch.Draw(texture, position, source, Color.White, 0.0f, center, 1.0f, SpriteEffects.None, 0.0f);                              // Draw the bomb at it's position
        }
    }
}
