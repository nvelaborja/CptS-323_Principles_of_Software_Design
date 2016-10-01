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
    // A mean-ass-Motha-f***
    class Enemy_Large : Enemy_Abstract
    {
        private int horizontalSpeed;                                                //Horizontal speed of enemy (added for ease of changing )
        private int verticalSpeed;
        private int reAppearTime;
        private int currentTick;

        private int laserTimer;

        // Condensing some common declarations between the constructors
        private void initialize()
        {
            // Some numbers I've found that work 
            speed = ContentMaster.randy.Next() % 15 + 1;
            bulletDelay = 15;
            maxBullets = 100;

            speedBullet = ContentMaster.randy.Next() % 13 + 1;
            horizontalSpeed = 130;
            verticalSpeed = ContentMaster.randy.Next() % 50 + 15;
            dropTime = ContentMaster.randy.Next() % 100 + 60;
            laserTimer = bulletDelay * 15;
            laserOut = false;
            this.pointValue = 6000;
        }

        public Enemy_Large(Vector2 spawnPosition, Player player)                                                                     // Player constructor, some initializations should be done here rather than in load content
        {
            // Position.Y is decremented in oorder to give the enemy  a more
            // "fluid" feeling when it is spawning into the screen
            position.Y -= ContentMaster.randy.Next() % 15 + 1;
            this.player = player;
            currentTick = 0;
            reAppearTime = ContentMaster.randy.Next() % 160 + 50;
            bulletList = new List<Bullet_Abstract>();
            position = spawnPosition;
            previousPosition = spawnPosition;
            spawnPoint = position;
            initialize();
            enemyBullets = new List<Bullet_Abstract>(); // A list of bullets this enemy specifically creates
            SM = ContentMaster.SM;
            //TODO: This monster needs a new texture. 
            texture = ContentMaster.enemy_large_texture;
            origin = new Vector2(10, 10);
            origin.X = position.X + (texture.Width / 2);
            origin.Y = position.Y + (texture.Height / 2);
            crest = position;
            crest.X = position.X + (texture.Width / 2);
            hitBox = new Rectangle((int)position.X, (int)position.Y, texture.Width, texture.Height);
           BulletMultiplier= (int)(player.Level * 5);
           this.armor = 300;
           this.health = 300;

        }

        //Override the update
        public override void Update(GameTime gameTime)
        {
            currentTick++;
            previousPosition = position;
            managePosition(gameTime);

            if (takephysicaldamage) { doPhysicalDamage(); }

            origin.X = position.X + texture.Width / 2;                                // Update player's origin (center) to match new position
            origin.Y = position.Y + texture.Height / 2;
            crest = position;
            crest.X = position.X + (texture.Width / 2);
            //Shoot at an interval, Has been modified so peons don't all shoot at the same time
            Shoot();
            UpdateBullets(gameTime);

            if (OutOfBounds(position) == "xbounds")                                                      // Checks to see if player's new position is outside of the playable area
            { position = previousPosition; }                                              // If so, return position to position from the beginning of this frame
            else if (OutOfBounds(position) == "ybounds")
            { position.Y = 0; } // If enemey exceeds y bounds, set it back up at the top

            hitBox.X = (int)position.X;
            hitBox.Y = (int)position.Y - 24;
        }

        // Shoot functionality in this enemy 
        public void Shoot()                                                                 // Spawns a bullet and adds it to bullet list
        {
            if (currentTick % bulletDelay != 0) { return; }                                        // Only execute if we've waited for our bullet delay, otherwise decrement delay
            else if (bulletList.Count < maxBullets)                                                 // Only spawn a bullet if we haven't reached our bullet cap
            {
                // Add the right amount of tracker bullets! 
                int variable = 1;
                int angleIncrease = 0;
                while (ContentMaster.randy.Next() % 2 == 0)
                { // Add a new bullet for every even number we encounter.

                    Bullet_Abstract newBullet = new Bullet_Tracking(Content, speedBullet, speedBullet, crest, player.Position, variable * (20 + angleIncrease) * Math.PI / 180);
                    newBullet.Power += bulletMultiplier;
                    this.bulletList.Add(newBullet);                                                  // Add bullet to the universal list
                    this.enemyBullets.Add(newBullet);
                    angleIncrease = 1 + (ContentMaster.randy.Next() % 25); // Increase the angle every iteration
                    if (ContentMaster.randy.Next() % 2 == 0) { angleIncrease = 0; }
                    variable *= -1;

                }

                SM.playBullet();

                // If it is the correct time, fire the el-laser-o
                if (currentTick % (laserTimer) == 0 && !laserOut)
                {
                    Character leplayer = (player as Character);
                    Bullet_Abstract theBullet = new Bullet_Laser(Content, this, ref leplayer, true);
                    this.bulletList.Add(theBullet);                                                  // Add bullet to the universal list
                    this.enemyBullets.Add(theBullet);
                    laserOut = true;
                    SM.playLazer();
                }
                // Add Bullet to the enemy Bullet List
                // Reset bullet delay to whatever it started as 
            }
        }

   





        //Manage the position of the enemy
        protected override void managePosition(GameTime gameTime)
        {// A function created specifically for managing the new position of the enemy per frame, modifies positionx and positiony

            if (currentTick % reAppearTime == 0 && !laserOut)
            {
                // This enemy will re-appear itself on random positions on the screen, whenever spawn time is replenished
                position.X = ContentMaster.randy.Next() % ContentMaster.playableWidth;
                position.Y = ContentMaster.randy.Next() % ContentMaster.playableHeight;
            }

        }

   

        //Returns no if not out of bounds
        //returns xbounds if x is out of bounds
        //returns ybounds if y is out of bounds
        private string OutOfBounds(Vector2 position)                                          // Determines if the given position is out of the playable area
        {

            if (position.X < 0 || position.X > (ContentMaster.playableWidth))
            { return "xbounds"; }
            else if (position.Y < 0 || position.Y > (ContentMaster.playableHeight))
            { return "ybounds"; }

            return "no";
        }






    }
}
