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
    public class Enemy_Peon : Enemy_Abstract 
    {
        #region Class Members

        private int horizontalSpeed;                                                //Horizontal speed of enemy (added for ease of changing )
        private int verticalSpeed;                                                  //Vertical speed of enemy
        private int prevXvelocity;
        private int curXvelocty;                                                    // convienient to keep track of previous and current x velocity
                                                                             
        #endregion

        // Condensing some common declarations between the constructors
        private void initialize()
        {
            level = 1;
            speed = 8;
            bulletDelay = 5;
            maxBullets = 100;
            shootInterval = ContentMaster.randy.Next()% 100 + 101;
            horizontalSpeed = ContentMaster.randy.Next() % (level * 50);
            verticalSpeed = ContentMaster.randy.Next() % (level * 2);
            attackModifier = 0.3 * level;
            armorModifier = 1.1 * level;
            speedBullet = -level - 1;
            ticks = 0;
            dropTime = 5;
            crest = position;
            crest.X = position.X + (texture.Width / 2);
            crest.Y += texture.Height;
            pointValue = 1173;
           
        }

        private Enemy_Peon()                                                                     // Player constructor, some initializations should be done here rather than in load content, don't use for now
        {
            SM = ContentMaster.SM;
            bulletList = new List<Bullet_Abstract>();
            centerScreen = new Vector2((ContentMaster.randy.Next()%ContentMaster.playableWidth) + borderWidth, (ContentMaster.playableHeight / 2) + borderWidth);
            position = centerScreen;
            previousPosition = position;
            spawnPoint = position;
            initialize();
            
        }

        // Shoot interval based off player's level
        public Enemy_Peon(Vector2 spawnPosition, Player player)                                                                     // Player constructor, some initializations should be done here rather than in load content
        {
            bulletList = new List<Bullet_Abstract>();
            position = spawnPosition;
            previousPosition = spawnPosition;
            spawnPoint = position;
            
            SM = ContentMaster.SM; 
            texture = ContentMaster.enemy_peon_texture;
            origin = new Vector2(10, 10);
            origin.X = position.X + (texture.Width / 2);
            origin.Y = position.Y + (texture.Height / 2);
            this.player = player;
            hitBox = new Rectangle((int)position.X, (int)position.Y, texture.Width, texture.Height);
            this.bulletMultiplier= (int)(player.Level * 4); 

            initialize();
        }

        //DEPRECATED 
        //public override void LoadContent(ContentManager Content)                                     // Load texture content and initialize variables that rely on texture information
        //{

     //   }
        
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
            crest.Y += texture.Height + 16;

            if (OutOfBounds(position) == "xbounds")                                                      // Checks to see if player's new position is outside of the playable area
            { position = previousPosition; }                                              // If so, return position to position from the beginning of this frame
            else if (OutOfBounds(position) ==  "ybounds" )
            { position.Y = 0 ; } // If enemey exceeds y bounds, set it back up at the top

            previousPosition = position;                                                    // Set previous position to new position, for use on next frame
           // speed = 8;                                                                // Set player speed back to normal for next frame
            ticks++;

            hitBox.X = (int)position.X;
            hitBox.Y = (int)position.Y;
        }



        #region GamePlay Functions

        protected override void managePosition(GameTime gameTime)
        {// A function created specifically for managing the new position of the enemy per frame, modifies positionx and positiony

            // Amount of ticks we should wait before dropping
            
            //Guesstimated values, showing concept. gameTime should be able to used
            //Instead of ticks, but I couldnt get it to work
            //Article I referenced for smooth back/fourth behaviour
            //http://gamedevelopment.tutsplus.com/tutorials/quick-tip-create-smooth-enemy-movement-with-sinusoidal-motion--gamedev-6009

            position.X = (float)(horizontalSpeed * Math.Sin((ticks + horizontalSpeed) * 0.5 * Math.PI / 100) + spawnPoint.X);

            if (ticks % dropTime == 0)
            {
                position.Y += verticalSpeed;
            }
        }

        // Shoot multiple types of bullets now
        public void Shoot()                                                                 // Spawns a bullet and adds it to bullet list
        {
             if (bulletList.Count < maxBullets)                                         // Only spawn a bullet if we haven't reached our bullet cap
            {
                  Bullet_Abstract newBullet = null;
                // I want the nemy peon ot be able to shoot two types of bullets
                if (ContentMaster.randy.Next() % 10 != 0 || player.Level <= 2)
                { newBullet = new Bullet_Small(Content, crest, speedBullet);
                newBullet.Power += bulletMultiplier;
                }     // Create new bullet located at the player's crest with the current bullet speed (could change bullet to bullet)
                else if(player.Level >2) { newBullet = new Bullet_Med(
                    Content, crest, 0, speedBullet);
                newBullet.Power += bulletMultiplier * 3; // three times stronger than regular bullet
                }
                SM.playBullet();
                this.bulletList.Add(newBullet);                                                  // Add bullet to the universal list
                this.enemyBullets.Add(newBullet);                                           // Add Bullet to the enemy Bullet List 
            }
            
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
