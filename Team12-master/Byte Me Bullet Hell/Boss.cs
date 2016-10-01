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
    public class Boss : Character
    {
        #region Class Members

        private int speedBullet;                                                            // Bullet speed (distance in pixels between each draw function after bullet fire)
        private int bulletDelay;                                                            // Makes player wait x amount of frames before being able to fire again
        private int maxBullets;                                                             // Maximum number of bullets that can be on a screen at one time
        private int shootInterval;
        private List<Bullet_Abstract> bulletList;                                           // List of bullets on the screen
        private int ticks = 0;
        private int verticalSpeed;                                                          //the vertical spee dof the boss 
        private int horizontalSpeed;                                                        // Horizontal speed of the boxx
        private Player player;                                                              // Boss should have informaiton of the player
        private int phase;
        private int bulletsPerWave;
        private int currentTick;
        private int reAppearTime;
        private bool isDead = false;

        // the elapsed amount of time the frame has been shown for
        float time;
        // duration of time to show each frame
        float frameTime = 0.05f;
        // an index of the current frame being shown
        int frameIndex;
        // total number of frames in our spritesheet
        const int totalFrames = 79;
        // define the size of our animation frame
        int frameHeight = 120;
        int frameWidth = 120;

        #endregion
        
        public Boss(Vector2 spawnPosition, Player theplayer)                                // Player constructor, some initializations should be done here rather than in load content
        {
            texture = ContentMaster.boss_texture;
            bulletList = new List<Bullet_Abstract>();
            position = spawnPosition;
            previousPosition = spawnPosition;
            speed = 2;
            bulletDelay = 5;
            maxBullets = 20;
            speedBullet = -20;
            shootInterval = 70;
            horizontalSpeed = ContentMaster.randy.Next(0, 5);
            player = theplayer;
            reAppearTime = 400;
            currentTick = 0;
            phase = 1;
            
            bulletsPerWave = 18;

            SM = ContentMaster.SM;  
            origin = new Vector2(10, 10);
            origin.X = position.X + (frameWidth / 2);
            origin.Y = position.Y + (frameHeight / 2);
            crest = position;
            crest.X = position.X + (frameWidth / 2);
            hitBox = new Rectangle((int)position.X, (int)position.Y, frameWidth, frameHeight);
        }

        public List<Bullet_Abstract> Bullets
        {
            get { return bulletList; }
        } 

        public bool IsDead
        {
            get { return isDead; }
        }

        //setting up the phases
        private void managePhases()
        {
            if (health <= 0)
            {
                phase++;
                if (phase < 4)
                {
                    health = 100;
                    armor = 100;
                }
            }
            if (phase == 1)
            {
                armorModifier = .1;
                phase1();
            }
            else if (phase == 2)
            {
                armorModifier = .075;
                phase2();
            }
            else if (phase == 3)
            {
                armorModifier = .05;
                phase3();
            }
            else
            {
                isDead = true;
            }
        } 
        
        // Phases will manage things like shooting and movement 
        private void phase1() 
        {
            managePosition1();
            //Shoot at an interval
            if (0 == (ticks % shootInterval))
            {
                Shoot1();
                Shoot1();
                Shoot1();
            } 
        }

        private void phase2()
        {
            managePosition2();
            if (0 == (ticks % shootInterval))
            {
                Shoot2();
                Shoot2();
            }
        }

        private void phase3()
        {
            managePosition2();
            //Shoot at an interval
            if (0 == (ticks % shootInterval))
            {
                reAppearTime = 250;
                 Shoot3();
            }
        }
        
        public void Update(GameTime gameTime)                                               // Called every frame, must update player movement, bullet movement, and firing / jumping
        {
            currentTick++;
            UpdateBullets(gameTime);                                                        // Call function located below, updates each bullet based on their position and speed

            managePhases();                                                                         // manage the current phase

            origin.X = position.X + frameWidth / 2;                                // Update player's origin (center) to match new position
            origin.Y = position.Y + frameHeight / 2;
            crest = position;
            crest.X = position.X + (frameWidth / 2);                               // Update player crest for bullet spawning

            if (OutOfBounds(position))                                                      // Checks to see if player's new position is outside of the playable area
            {
                position = previousPosition;                                                // If so, return position to position from the beginning of this frame
            }

            previousPosition = position;                                                    // Set previous position to new position, for use on next frame
            speed = 2;                                                                // Set player speed back to normal for next frame
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

        public void Draw(SpriteBatch spriteBatch)
        {
            Rectangle source = new Rectangle(frameIndex * frameWidth, 0, frameWidth, frameHeight);

            Vector2 center = new Vector2(frameWidth / 2.0f, frameHeight);

            Vector2 newPosition = position;
            newPosition.X = newPosition.X + 60;
            newPosition.Y = newPosition.Y + 60;

            spriteBatch.Draw(texture, newPosition, source, Color.White, 0.0f, center, 1.0f, SpriteEffects.None, 0.0f);                         // Draw Player

            foreach (Bullet_Abstract bullet in bulletList)                                            // Draw Bullets in bullet list
            {
                bullet.Draw(spriteBatch);
            }
        }


        #region GamePlay Functions

        //manage movement (for phase 1) 
        private void managePosition1()
        {
            int buffer = player.HitBox.Height + 100 ;   // TODO: Make this based off the player's hitbox 

            // For now, follow player

            if (player.Position.X + buffer <= position.X)
            {
                position.X -= horizontalSpeed;
            }
            else if (player.Position.X - buffer > position.X)
            {
                position.X += horizontalSpeed;
            }
            else // cruise 
            {
                int random = ContentMaster.randy.Next() % 2;
                if (random == 0)
                {
                    position.X += horizontalSpeed;
                }
                else
                {
                    position.X -= horizontalSpeed;
                }

            }

            if (player.Position.Y + buffer <= position.Y)
            {
                position.Y -= verticalSpeed;
            }
            else if (player.Position.Y - buffer >= position.Y)
            {
                position.Y += verticalSpeed;
            }
            else
            {
                int random = ContentMaster.randy.Next() % 2;
                if (random == 0)
                {
                    position.Y += verticalSpeed;
                }
                else
                {
                    position.Y -= verticalSpeed;
                }
            }
        }

        private void managePosition2()
        {
            if (currentTick % reAppearTime == 0)
            {
                // This enemy will re-appear itself on random positions on the screen, whenever spawn time is replenished
                position.X = ContentMaster.randy.Next() % ContentMaster.playableWidth;
                position.Y = ContentMaster.randy.Next() % ContentMaster.playableHeight;
            }
        }

        //There should be multiple different shoot functions for the boss, enabling the boss to shoot in multiple different ways 
        //I will label this function shoot1
        public void Shoot1()                                                                 // Spawns a bullet and adds it to bullet list
        {
            SM.playBullet();
            if (bulletDelay > 0) { bulletDelay--; }                                         // Only execute if we've waited for our bullet delay, otherwise decrement delay
            
            else if (bulletList.Count < maxBullets)                                         // Only spawn a bullet if we haven't reached our bullet cap
            {
                Bullet_Abstract newBullet1 = new Bullet_Med(Content, crest, speedBullet, -speedBullet);      // Create new bullet located at the player's crest with the current bullet speed (could change bullet to bullet)
                Bullet_Abstract newBullet2 = new Bullet_Med(Content, crest, -speedBullet, speedBullet);      // Create new bullet located at the player's crest with the current bullet speed (could change bullet to bullet)        
                Bullet_Abstract newBullet3 = new Bullet_Med(Content, crest, -speedBullet, -speedBullet);      // Create new bullet located at the player's crest with the current bullet speed (could change bullet to bullet)        
                Bullet_Abstract newBullet4 = new Bullet_Med(Content, crest, speedBullet, speedBullet);
                Bullet_Abstract newBullet5 = new Bullet_Med(Content, crest, -speedBullet / 2, -speedBullet);      // Create new bullet located at the player's crest with the current bullet speed (could change bullet to bullet)        
                Bullet_Abstract newBullet6 = new Bullet_Med(Content, crest, speedBullet, speedBullet / 2); 
                // TODO: We need a method to calculate differed radial positions of bullets, bullets that aren't just in the 4 corners,  somethinf to do with tig 
           

                  bulletList.Add(newBullet1);                                                  // Add bullet to the list
                  bulletList.Add(newBullet2);
                  bulletList.Add(newBullet3);
                  bulletList.Add(newBullet4);
                  bulletList.Add(newBullet5);
                  bulletList.Add(newBullet6);
                  bulletDelay = (ContentMaster.randy.Next() % 5);   
            }
        }

        public void Shoot2()
        {
            SM.playBullet();

            double angle = (2 * Math.PI) / bulletsPerWave;
            for (double theta = 0; theta < (Math.PI * 2); theta += angle)
            {
                double xSpeed = Math.Sin(theta);
                double ySpeed = Math.Cos(theta);
                Bullet_Abstract newBullet = new Bullet_Med(Content, crest, xSpeed, ySpeed);
                bulletList.Add(newBullet);
            }


        }

        public void Shoot3()
        {
            SM.playBullet();

            for (double x = -1; x <= 1; x = x + .6)
            {
                for (double y = -1; y <= 1; y = y + .6)
                {
                    if (x == 0 && y == 0)
                        break;
                    Bullet_Abstract newBullet = new Bullet_Med(Content, crest, x, y);
                    bulletList.Add(newBullet);
                }
            }
        }

       public void calcRadInt(ref int x, ref int y) 
       {
                //TODO: Math, htis funciton will be used for the x and y positions fo the bullets, except we need a function that can
                // calculate this radially (I'm thinking 360 bullets)
        }
                       
        public void UpdateBullets(GameTime gameTime)                                        // Iterates through bullets in bullet list and updates each one
        {
            foreach ( Bullet_Abstract b in bulletList)                                                // Update each bullet's position
            {
                b.Update(gameTime);                                                                             // This is now an abstract method! Whoo. 
            }

            for (int i = 0; i < bulletList.Count; i++)                                      // Delete bullets that are no longer visible (off the screen)
            {
                if (!bulletList[i].IsVisible)
                {
                    bulletList.RemoveAt(i);
                    i--;
                }
            }
        }

        #endregion

        #region Helper Functions

        private bool OutOfBounds(Vector2 position)                                          // Determines if the given position is out of the playable area
        {
            bool isOutOfBounds = false;

            if (position.X < 0 || position.X > (playableWidth + borderWidth) ||
                position.Y < 0 || position.Y > (playableHeight))                           // Looking vertically, there are two borders to account for
            {
                isOutOfBounds = true;
            }
            return isOutOfBounds;
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