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
    class Bullet_Tracking : Bullet_Abstract
    {
        /*
         * How Bob Describes this Bullet: 
         * 
         * 
My intention wasn’t “tracking all the way”, but just aiming at the player at the moment of spawning.
 
So: it would look something like this.
 
Odd-way homing bullets:
                *spawning location
             /   |   \ 
           /     |     \
(player)   
 
 
Even-way homing bullets:
                 *spawning location
             /        \ 
           /            \
(player)   
 
Bob 
     
         * */
        protected double xSpeed;                                                               // Distance in pixels that the bullet will shift each draw()
        protected double ySpeed;
        protected Vector2 target;
        double directionOfTarget;
        double angle;               // angle is the angle between diretly hitting the player and missing, dhang said this is
        // what he wanted, note this is in radians so 0.2 is about 15 degrees

        // Constructor, the targe is the position we are trying to aim for
        public Bullet_Tracking(ContentManager Content, int xSpeed, int ySpeed, Vector2 spawnPoint, Vector2 Target, double angle)           // Constructor takes texture (let's us create different types of bullets from player class)
        {
            this.xSpeed = xSpeed;
            this.ySpeed = ySpeed;
            this.Content = Content;
            this.target = Target; // position of the target bullet is going to hit
            this.position = spawnPoint; // Spawn point of the bullet
            this.angle = angle;
            // Obtain the position of the target with respect to the bullet

            // We don't want to accidentally divide by zero
            if (spawnPoint.X == Target.X)
            {
                Target.X += (float)0.1;
            }
            this.directionOfTarget = Math.Atan(((spawnPoint.Y - target.Y)) / ((spawnPoint.X - Target.X)));
            if (spawnPoint.X >= target.X) { directionOfTarget += Math.PI; }
            texture = ContentMaster.bullet_tiny;
            isVisible = true;
            hitBox = new Rectangle((int)position.X, (int)position.Y, texture.Width, texture.Height);
        }

        //Update the position of the bullet
        override public void Update(GameTime gameTime)
        {
            //TODO: randomize variation


            position.X += ((float)(xSpeed * Math.Cos(directionOfTarget + angle)));
            position.Y += ((float)(ySpeed * Math.Sin(directionOfTarget + angle)));

            if (position.Y < 0 || position.Y > ContentMaster.playableHeight || position.X < 0 || position.X > ContentMaster.playableWidth)                                                           // Set invisible to false if bullet is far off the top of the screen
            {
                isVisible = false;
            }

            hitBox.X = (int)position.X;
            hitBox.Y = (int)position.Y;
        }

        public override void Draw(SpriteBatch spriteBatch)
        { // TODO: Make different colored bullets

            int colorNum = ContentMaster.randy.Next() % 10;
            Color randColor = Color.Aqua;
            switch (colorNum)
            {
                case 1:
                    randColor = Color.Beige;
                    break;
                case 2:
                    randColor = Color.BlueViolet;
                    break;
                case 3:
                    randColor = Color.BurlyWood;
                    break;
                case 4:
                    randColor = Color.Coral;
                    break;
                case 5:
                    randColor = Color.Crimson;
                    break;
                case 6:
                    randColor = Color.DarkMagenta;
                    break;
                case 7:
                    randColor = Color.DarkRed;
                    break;
                case 8:
                    randColor = Color.DarkSlateBlue;
                    break;
                case 9:
                    randColor = Color.DarkTurquoise;
                    break;
                case 0:
                    randColor = Color.DarkViolet;
                    break;

            }

            spriteBatch.Draw(texture, position, randColor);                               // Draw the bullet at it's position
        }

    }
}
