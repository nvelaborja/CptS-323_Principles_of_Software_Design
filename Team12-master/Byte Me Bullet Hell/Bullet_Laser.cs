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
    /**
     * A Laser-like bullet as defined in the specifications
     * 
     * */

    class Bullet_Laser : Bullet_Abstract
    {
        protected Vector2 target; // keep track of the target
        double directionOfTarget;
        bool rotate;
        int duration; // The duration of the laser beam
        int currentTick;
        Vector2 refrencePoint;
        Character shooter; // The refrence to the character who is shooting
        Character victim; // the refrence to the chatacter who is being attacked
        double angle; // angle offset in radians
        int negate; // randomly reverse direction
        string direction; // The direction the laser should point in


        // Constructor, the targe is the position we are trying to aim for
        public Bullet_Laser(ContentManager Content, Character shooter, ref Character target, bool rotate)           // Constructor takes texture (let's us create different types of bullets from player class)
        {
            this.power = 100;
            this.shooter = shooter;
            this.currentTick = 0;
            this.Content = Content;
            this.duration = 300;
            this.directionOfTarget = 0;
            this.target = target.ThePosition;
            texture = ContentMaster.bullet_lazer; // render the correct texture
            isVisible = true;       // Set visibility to true
            angle = 0;
            this.rotate = rotate;
            //retrieve appropriate type of shooter
            this.victim = target;
            negate = 1;
            if (ContentMaster.randy.Next() % 2 == 0)
            {
                negate = -1;
            }

            if (shooter is Enemy_Abstract) { this.refrencePoint = (shooter as Enemy_Abstract).Position; }
            if (shooter is Player) { this.refrencePoint = (shooter as Player).Position; }


            //hitBox = new Rectangle((int)position.X, (int)position.Y, texture.Width +20, texture.Height/2);

        }

        //Update the position of the bullet
        override public void Update(GameTime gameTime)
        {
            target = victim.ThePosition;
            // Increment angle
            if (this.rotate)
            {
                angle += 0.017 * negate;
            }

            currentTick++;
            //this.position = target; 

            //retrieve appropriate type of shooter
            if (shooter is Enemy_Abstract)
            {
                this.refrencePoint = (shooter as Enemy_Abstract).Position;
                this.refrencePoint.X += shooter.Texture.Width / 2;
                this.refrencePoint.Y += shooter.Texture.Height / 2;
            }

            if (shooter is Player)
            {
                this.refrencePoint = (shooter as Player).Position;
                this.refrencePoint.X += shooter.Texture.Width / 2;
                this.refrencePoint.Y += shooter.Texture.Height / 2;
            }

            // We don't want to accidentally divide by zero
            // if (refrencePoint.X == target.X) { refrencePoint.X += (float)0.1; }

            /* not needed here
              // Obtain the direction the laser will point in
              this.directionOfTarget = Math.Atan(((refrencePoint.Y - target.Y)) / ((refrencePoint.X - target.X)));
              if (refrencePoint.X >= target.X) { directionOfTarget += Math.PI; }
             * 
             * */


            position.X = refrencePoint.X;
            position.Y = refrencePoint.Y;


            //obtain the end of the laser
            position.X += (shooter.Texture.Height) * (float)Math.Cos(angle); // vertex of the laser
            position.Y += (shooter.Texture.Height) * (float)Math.Sin(angle);
            //position = refrencePoint;

            //this took me forever to figure out
            if (refrencePoint.X == target.X || refrencePoint.X == position.X)
            {
                refrencePoint.X = 0.02f;
            }

            //obtain the angle of the character with respect to the shooter
            float angleOfChar = (float)Math.Atan((this.refrencePoint.Y - target.Y) / (this.refrencePoint.X - this.target.X));
            if (this.refrencePoint.X >= target.X)
            {
                angleOfChar += (float)Math.PI;
            }

            //obtain angle of laser with respect to the shooter
            float angleOfLaser = (float)Math.Atan((this.refrencePoint.Y - this.position.Y) / (this.refrencePoint.X - this.position.X));
            if (this.refrencePoint.X >= this.position.X)
            {
                angleOfLaser += (float)Math.PI;
            }

            // If the angles are identical, strike down that ship!
            // We can ignore hitbox for now, it wasn't helping me
            if (angleOfLaser <= angleOfChar + 0.03 && angleOfLaser > angleOfChar - 0.03)
            {

                victim.TakeDamage(30);
                ContentMaster.SM.playHurt();
                // ContentMaster.SM.laserHit;
            }



            //check if time has expired. 
            if (currentTick % duration == 0) { isVisible = false; }

            //Don;t worry about hitbox for now;

        }

        public override void Draw(SpriteBatch spriteBatch)
        { // TODO: Make different colored bullets

            // Tilt the laser with respect to the player's position Currently not working
            spriteBatch.Draw(texture, refrencePoint, null, Color.White, ((float)(angle - Math.PI / 2)), new Vector2(), 1, SpriteEffects.None, 0);

        }




    }
}
