using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Byte_Me_Bullet_Hell
{
    public class Bullet_Small : Bullet_Abstract
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
        int frameHeight = 16;
        int frameWidth = 4;

        public Bullet_Small(ContentManager Content, Vector2 Position, int bulletSpeed)           // Constructor takes texture (let's us create different types of bullets from player class)
        {
            this.Content = Content;
            texture = ContentMaster.bullet_small;
            speed = bulletSpeed;
            position = Position;
            isVisible = true;
            hitBox = new Rectangle((int)position.X, (int)position.Y, frameWidth, frameHeight);
            power = 10;
        }
        
        public override void  Update(GameTime gameTime)
        {
            // Could implement Rotation here

            position.Y = position.Y - (float)speed;                                         // Move bullet upwards by (speed) pixels
            hitBox.Location = new Vector2(position.X, position.Y).ToPoint();                // Update hitbox location to match new bullet position

            if (position.Y < -50 || position.Y> ContentMaster.playableHeight)                   // Set invisible to false if bullet is far off the top of the screen
            {
                isVisible = false;
            }

            hitBox.X = (int)position.X;
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

            spriteBatch.Draw(texture, position, source, Color.White, 0.0f, center, 1.0f, SpriteEffects.None, 0.0f);                              // Draw the bullet at it's position
        }
    }
}