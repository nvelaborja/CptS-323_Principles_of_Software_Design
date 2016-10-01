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
    class Enemy_Lurker : Enemy_Abstract
    {
        #region Class Members
        
        private int horizontalSpeed;                                                  //Horizontal speed of enemy (added for ease of changing )
        private int verticalSpeed;                                                        //Vertical speed of enemy
 

        // the elapsed amount of time the frame has been shown for
        float time;
        // duration of time to show each frame
        float frameTime = 0.01f;
        // an index of the current frame being shown
        int frameIndex;
        // total number of frames in our spritesheet
        const int totalFrames = 59;
        // define the size of our animation frame
        int frameHeight = 55;
        int frameWidth = 55;
        private int greaterDamage = 35;

        #endregion

        // Condensing some common functionality between the two constructors
        private void inititialize()
        {
            level = 1;
            speed = 3;
            bulletDelay = 5;
            maxBullets = 100;
            speedBullet = -20;
            shootInterval = 15;

            horizontalSpeed = ContentMaster.randy.Next() % 5 +1;
            verticalSpeed = ContentMaster.randy.Next() % 5 +1;
            SM = ContentMaster.SM;
            attackModifier = 0.6 * level;
            armorModifier = 0.8 * level;
            pointValue = 3242;
        }


        private Enemy_Lurker(Player player)                                                                     // Player constructor, some initializations should be done here rather than in load content
        { // Don't use for now
            bulletList = new List<Bullet_Abstract>();
            centerScreen = new Vector2((ContentMaster.randy.Next()%ContentMaster.playableWidth) + borderWidth, 0 );
            position = centerScreen;
            previousPosition = position;
            spawnPoint = position;
            inititialize();
            this.player = player;
            greaterDamage +=  (int)(player.Level * 10);
        }

        public Enemy_Lurker(Vector2 spawnPosition, Player player)             // Player constructor, some initializations should be done here rather than in load content
        {
            spawnPosition.Y = 0;
            bulletList = new List<Bullet_Abstract>();
            position = spawnPosition;
            previousPosition = spawnPosition;
            spawnPoint = position;
            inititialize();
            this.player = player;
            SM = ContentMaster.SM;
            texture = ContentMaster.enemy_lurker_texture;
            origin = new Vector2(10, 10);
            origin.X = position.X + (texture.Width / 2);
            origin.Y = position.Y + (texture.Height / 2);
            crest = position;
            crest.X = position.X + (texture.Width / 2);
            hitBox = new Rectangle((int)position.X, (int)position.Y, frameWidth, frameHeight);
           BulletMultiplier= (int)(player.Level * 5); 

        }

      //  public override void LoadContent(ContentManager Content)                                     // Load texture content and initialize variables that rely on texture information
       // {

       // }

        
        public override void Update(GameTime gameTime)                                               // Called every frame, must update player movement, bullet movement, and firing / jumping
        {
            UpdateBullets(gameTime);                                                        // Call function located below, updates each bullet based on their position and speed

            managePosition(gameTime);                                                                       // manage enemy position 

            if (takephysicaldamage) { doPhysicalDamage(); }

            //Shoot at an interval
            if (0 == (ticks % shootInterval))
                Shoot();

            origin.X = position.X + texture.Width / 2;                                // Update player's origin (center) to match new position
            origin.Y = position.Y + texture.Height / 2;
            crest = position;
            crest.X = position.X + (texture.Width / 2);                               // Update player crest for bullet spawning


            if (OutOfBounds(position) == "xbounds")                                                      // Checks to see if player's new position is outside of the playable area
            { position = previousPosition; }                                              // If so, return position to position from the beginning of this frame
            else if (OutOfBounds(position) ==  "ybounds" )
            { position.Y = 0 ; } // If enemey exceeds y bounds, set it back up at the top

            previousPosition = position;                                                    // Set previous position to new position, for use on next frame                                                             // Set player speed back to normal for next frame
            ticks++;

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
            spriteBatch.Draw(texture, position, source, Color.White, 0.0f, center, 1.0f, SpriteEffects.None, 0.0f);   

                      // Draw Player

            //Draw bullets specifically in enemyBullets
            foreach (Bullet_Abstract b in enemyBullets)                                                                // Draw Bullets in bullet list
            {
                if (b is Bullet_Small) { (b as Bullet_Small).Draw(spriteBatch); }
                if (b is Bullet_Med) { (b as Bullet_Med).Draw(spriteBatch); }
                if (b is Bullet_Tracking) { (b as Bullet_Tracking).Draw(spriteBatch); }
                if (b is Bullet_Laser)
                {
                    (b as Bullet_Laser).Draw(spriteBatch);
                }
            }
        }



        #region GamePlay Functions


        protected override void managePosition(GameTime gameTime)
        {// A function created specifically for managing the new position of the enemy per frame, modifies positionx and positiony
            int buffer = 0;   // TODO: Make this based off the player's hitbox 

            int xSpeed = 0;
            int ySpeed = 0;
            if (Math.Abs(player.Position.X - position.X) > Math.Abs(player.Position.Y - position.Y))
            {
                xSpeed = speed;
                ySpeed = speed / 2;
            }
            else
            {
                xSpeed = speed / 2;
                ySpeed = speed;
            }

            if (player.Position.X + buffer <= position.X)
            {
                position.X -= xSpeed;
            }
            else if (player.Position.X - buffer > position.X)
            {
                position.X += xSpeed;
            }
         

            if (player.Position.Y + buffer <= position.Y)
            {
                position.Y -= ySpeed;
            }

            else if (player.Position.Y - buffer >= position.Y)
            {
                position.Y += ySpeed;
            }
            
         
        }

        public void Shoot()                                                                 // Spawns a bullet and adds it to bullet list
        {
            if (bulletList.Count < maxBullets)                                                 // Only spawn a bullet if we haven't reached our bullet cap
            {
                // Add the right amount of tracker bullets! 
                int variable = 1;
                int angleIncrease = 0;
                while (ContentMaster.randy.Next() % 2 == 0)
                { // Add a new bullet for every even number we encounter.

                    Bullet_Abstract newBullet = new Bullet_Tracking(Content, speedBullet, speedBullet, crest, player.Position, variable * (20 + angleIncrease) * Math.PI / 180);
                    newBullet.Power += BulletMultiplier;
                    this.bulletList.Add(newBullet);                                                  // Add bullet to the universal list
                    this.enemyBullets.Add(newBullet);
                    angleIncrease = 1 + (ContentMaster.randy.Next() % 25); // Increase the angle every iteration
                    if (ContentMaster.randy.Next() % 2 == 0) { angleIncrease = 0; }
                    variable *= -1;

                }

                SM.playBullet();

            }
        }

        //Take physical Damage
        public override void doPhysicalDamage()
        {//TODO: place sprite here
            takephysicaldamage = false;
            player.TakeDamage(greaterDamage + BulletMultiplier);
           this. health = 0;
            SM.playImpact();
            SM.playImpact();
            //SM.PL
            SM.playHurt();
        }

        public void changePosition()
        {
           if (ContentMaster.randy.Next() % 2 == 0)
            {
                this.position.X += ContentMaster.randy.Next() % speed;
            }
            else { this.position.X -= ContentMaster.randy.Next() % speed; }

           if (ContentMaster.randy.Next() % 2 == 0)
           {
               this.position.Y += ContentMaster.randy.Next() % speed;
           }
           else { this.position.Y -= 5; }
        }

        #endregion

        #region Helper Functions

        //Returns no if not out of bounds
        //returns xbounds if x is out of bounds
        //returns ybounds if y is out of bounds
        private string OutOfBounds(Vector2 position)                                          // Determines if the given position is out of the playable area
        {
            if (position.X < -40 || position.X > (ContentMaster.playableWidth) )
            {     return "xbounds"; }
            else if  ( position.Y < 0  || position.Y > (ContentMaster.playableHeight )) 
            {    return "ybounds"; }

            return "no";
        }

        private double DistanceFromCenter(Vector2 position)                                 // Returns distance from given position to center screen
        {
            double distance = 0.0;

            distance = Math.Sqrt(Math.Pow(position.X - centerScreen.X, 2) + Math.Pow(position.Y - centerScreen.Y, 2));

            return distance;
        }


        #endregion
    }


}
