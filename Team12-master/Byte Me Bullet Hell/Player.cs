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
    public class Player : Character
    {
        #region Class Members

        private int speedBullet;                                                            // Bullet speed (distance in pixels between each draw function after bullet fire)
        private int bulletDelay;                                                            // Makes player wait x amount of frames before being able to fire again
        private int jumpDelay;                                                              // Makes player wait x amount of frames before being able to jump again
        private int jumpSize;                                                               // Size in pixels of player jump
        private int maxBullets;                                                             // Maximum number of bullets that can be on a screen at one time
        private List<Bullet_Abstract> bulletList;                                                    // List of bullets on the screen
        private bool isDead;
        private int score = 0;
        private uint shotsFired = 0;
        private uint enemiesKilled = 0;
        private uint shotsHit = 0;
<<<<<<< HEAD
        //private int health;
        private int healthWeight = 300;
=======
        private Settings settings;
>>>>>>> b6d8685a10858716d6a68a71b02869343d9b0a45

        //bomupdateb
        private int speedBomb;// Bomb speed (distance in pixels between each draw function after bombs fire)
        private int bombDelay;// Makes player wait x amount of frames before being able to fire again
        private int maxBombs; // Maximum number of bombs that can be on a screen at one time
        private List<Bomb> bombList; //list of bombs on the screen
        private int playerBombs =0;
        private int currentTick = 0;
        private int playerHealthPacks = 1;
        //health
        private int healthDelay;
        private int maxHealth;
        private bool addedHealthPack = false;
        private bool addedBomb = false;
        #endregion

        #region Constructors
        
        public Player(ref Settings Settings)                                                                     // Player constructor, some initializations should be done here rather than in load content
        {
            settings = Settings;

            bulletList = new List<Bullet_Abstract>();
            centerScreen = new Vector2((playableWidth / 2) + borderWidth, (playableHeight / 2) + borderWidth);
            position = centerScreen;
            previousPosition = position;
            speed = 8;
            bulletDelay = 5;
            jumpDelay = 0;
            jumpSize = 150 ;
            maxBullets = 20;
            speedBullet = 20;
            SM = ContentMaster.SM;
            isDead = false;
            texture = ContentMaster.player_texture;
            origin = new Vector2(0, 0);
            origin.X = position.X + (texture.Width / 2);
            origin.Y = position.Y + (texture.Height / 2);
            crest = position;
            crest.X = position.X + (texture.Width / 2);
            hitBox = new Rectangle((int)position.X, (int)position.Y, texture.Width, texture.Height);
            attackModifier = 1.0;
            armorModifier = 1.0;
            level = 1;

            //bomb
            bombList = new List<Bomb>(); //list of bombs on the screen
            bombDelay = 1; //makes player wait x amount of frames before firing again
            maxBombs = 1; // Maximum number of bombs that can be on a screen at one time
            speedBomb = 10; //Bomb speed (distance in pixels between each draw function after bombs fire)
            playerBombs = 3;

            //health
            health = 100;
            healthDelay = 10;
            maxHealth = 2;

            // Debug traits

            //health = 10000;
            //attackModifier = 10.0;
            //armorModifier = 100.0;

    }

        #endregion

        #region Properties

        public int Difficulty
        {
            get { return settings.Difficulty; }
        }

        public bool SoundOn
        {
            get { return settings.SoundOn; }
        }

        public List<Bullet_Abstract> BulletList
        {
            get { return bulletList; }
        }

        public List<Bomb> BombList
        {
            get { return bombList; }
        }

        public bool IsDead
        {
            get { return isDead; }
        }

        public int Score
        {
            get { return score; }
            set
            {
                score = value;
                
                if (score / level > (8500 + level * 1500))
                {
                    LevelUp();
                }
            }
        }

        public uint ShotsFired
        {
            get { return shotsFired; }
        }

        public uint ShotsHit
        {
            get { return shotsHit; }
            set { shotsHit = value; }
        }

        public uint EnemiesKilled
        {
            get { return enemiesKilled; }
            set { enemiesKilled = value; }
        }

        public int JumpDelay
        {
            get { return jumpDelay; }
        }


        #endregion

        #region MonoGame Functions


        public void Update(GameTime gameTime)                                               // Called every frame, must update player movement, bullet movement, and firing / jumping
        {

            currentTick++;
            #region Control Guide

            /****************************************\
             *              Controls:
             *       Movement: WASD
             *       Shoot: Left Click
             *       Slow Shoot Mode: Right Click
             *       Jump: Space
             *       Slow Movement: Shift
             *       Use Equipment: Numpad 1, 2, 3 (TODO)
             *       
             * **************************************/

            #endregion                                                          

 


            if (!isDead)
            {
                if (takephysicaldamage) { doPhysicalDamage(); }

                KeyboardState keyState = Keyboard.GetState();                                   // Get Keyboard state (can read multiple inputs)
                MouseState mouseState = Mouse.GetState();                                       // Get Mouse state (can read multiple inputs)

                if (keyState.IsKeyDown(Keys.P))
                {
                    while (level < 10)
                        LevelUp();

                }

                if (mouseState.LeftButton == ButtonState.Pressed)                               // Player Shoot
                {
                    if (mouseState.RightButton == ButtonState.Pressed)
                    {
                        speedBullet /= 2;                                                       // Reduce bullet speed by 1/2 if right click down
                    }
                    Shoot();                                                                    // Call shoot function, creates bullet based off position and bullet speed
                    speedBullet = 20;                                                           // Reset bullet speed for next frame
                }
                UpdateBullets(gameTime);                                                        // Call function located below, updates each bullet based on their position and speed

                if (keyState.IsKeyDown(Keys.LeftShift))                                         // Precision movement mode, temporarily reduces speed by half
                {
                    speed /= 2;
                }

                if (keyState.IsKeyDown(Keys.W))                                                 // Player Movement
                    position.Y -= speed;
                if (keyState.IsKeyDown(Keys.A))
                    position.X -= speed;
                if (keyState.IsKeyDown(Keys.S))
                    position.Y += speed;
                if (keyState.IsKeyDown(Keys.D))
                    position.X += speed;
                if (keyState.IsKeyDown(Keys.Space))                                             // Player jump, calls jump function located below
                    Jump(keyState);

     

                if (keyState.IsKeyDown(Keys.Z)) //press Z to fire bomb, can change later
                    ShootBomb();//calls ShootBomb function below
                UpdateBombs(gameTime);


                UpdateHealth();
                if (keyState.IsKeyDown(Keys.H)) //press H to use Health
                    UseHealth();

                if (jumpDelay > 0) { jumpDelay--; }

                origin.X = position.X + texture.Width / 2;                                // Update player's origin (center) to match new position
                origin.Y = position.Y + texture.Height / 2;
                crest = position;
                crest.X = position.X + (texture.Width / 2);                               // Update player crest for bullet spawning

                if (OutOfBounds(position))                                                      // Checks to see if player's new position is outside of the playable area
                {
                    position = previousPosition;                                                // If so, return position to position from the beginning of this frame
                }

                previousPosition = position;                                                    // Set previous position to new position, for use on next frame
                speed = 8;                                                                // Set player speed back to normal for next frame

                hitBox.X = (int)position.X;
                hitBox.Y = (int)position.Y;

                if (health < 1)
                {
                    isDead = true;
                    isVisible = false;
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (isVisible)
            {
                spriteBatch.Draw(texture, position, Color.White);                         // Draw Player
                
            }

            foreach(Bullet_Abstract bullet in bulletList)                                    // Draw Bullets in bullet list, bullets will continue to live after player dies
            {
                if (bullet is Bullet_Small)
                {
                    (bullet as Bullet_Small).Draw(spriteBatch);
                }
                if(bullet is Bullet_Laser_Player)
                {
                    (bullet as Bullet_Laser_Player).Draw(spriteBatch);
                }
                
            }

            foreach (Bomb bomb in bombList)
            {
                bomb.Draw(spriteBatch);
            }


        }

        #endregion

        #region GamePlay Functions

        public void Shoot()                                                                 // Spawns a bullet and adds it to bullet list
        {
            if (bulletDelay > 0)
            {
                bulletDelay--;
                return;
            }                                         // Only execute if we've waited for our bullet delay, otherwise decrement delay
            
                    switch (level)
                    {
                        case 1:
                           Bullet_Small bullet11 = new Bullet_Small(Content, crest, speedBullet);     // Create new bullet located at the player's crest with the current bullet speed (could change bullet to bullet)
                            SM.playBullet();
                            bulletList.Add(bullet11);                                                  // Add bullet to the list
                            bulletDelay = 5;                                                            // Reset bullet delay to whatever it started as 
                            shotsFired++;
                            break;

                        case 2:
                            Bullet_Small bullet21 = new Bullet_Small(Content, new Vector2(crest.X - 3, crest.Y), speedBullet);     // Create new bullet located at the player's crest with the current bullet speed (could change bullet to bullet)
                            Bullet_Small bullet22 = new Bullet_Small(Content, new Vector2(crest.X + 3, crest.Y), speedBullet);
                            SM.playBullet();
                            bulletList.Add(bullet21);
                            bulletList.Add(bullet22); // Add bullets to the list
                            bulletDelay = 5;                                                            // Reset bullet delay to whatever it started as 
                            shotsFired += 2;
                            break;
                        case 3:
                            Bullet_Small bullet31 = new Bullet_Small(Content, new Vector2(crest.X - 5, crest.Y), speedBullet);     // Create new bullet located at the player's crest with the current bullet speed (could change bullet to bullet)
                            Bullet_Small bullet32 = new Bullet_Small(Content, new Vector2(crest.X, crest.Y), speedBullet);
                            Bullet_Small bullet33 = new Bullet_Small(Content, new Vector2(crest.X + 5, crest.Y), speedBullet);
                            SM.playBullet();
                            bulletList.Add(bullet31);
                            bulletList.Add(bullet32); // Add bullets to the list
                            bulletList.Add(bullet33);
                            bulletDelay = 5;                                                            // Reset bullet delay to whatever it started as 
                            shotsFired += 3;
                            break;
                        case 4:
                            Bullet_Small bullet41 = new Bullet_Small(Content, new Vector2(crest.X - 9, crest.Y), speedBullet);     // Create new bullet located at the player's crest with the current bullet speed (could change bullet to bullet)
                            Bullet_Small bullet42 = new Bullet_Small(Content, new Vector2(crest.X - 3, crest.Y), speedBullet);
                            Bullet_Small bullet43 = new Bullet_Small(Content, new Vector2(crest.X + 3, crest.Y), speedBullet);
                            Bullet_Small bullet44 = new Bullet_Small(Content, new Vector2(crest.X + 9, crest.Y), speedBullet);
                            SM.playBullet();
                            bulletList.Add(bullet41);
                            bulletList.Add(bullet42); // Add bullets to the list
                            bulletList.Add(bullet43);
                            bulletList.Add(bullet44);
                            bulletDelay = 5;                                                            // Reset bullet delay to whatever it started as 
                            shotsFired += 4;
                            break;
                        case 5:
                            Bullet_Laser_Player lazer = new Bullet_Laser_Player(this, -1);
                            SM.playLazer();
                            bulletList.Add(lazer);
                            shotsFired += 1;
                            bulletDelay = 16;
                            // Lazer
                            break;
                        case 6:
                            Bullet_Laser_Player lazer2 = new Bullet_Laser_Player(this, -1);
                            Bullet_Small bullety1 = new Bullet_Small(Content, new Vector2(crest.X - 9, crest.Y), speedBullet);
                            Bullet_Small bullety2 = new Bullet_Small(Content, new Vector2(crest.X + 10, crest.Y), speedBullet);
                            SM.playLazer();
                            SM.playBullet();
                            bulletList.Add(lazer2);
                            bulletList.Add(bullety1);
                            BulletList.Add(bullety2);
                            shotsFired += 3;
                            bulletDelay = 16;

                            // Lazer + one small on each side
                            break;
                        case 7:
                            Bullet_Laser_Player lazer3 = new Bullet_Laser_Player(this, 5);
                            Bullet_Laser_Player lazer4 = new Bullet_Laser_Player(this, -7);
                            SM.playLazer();
                            bulletList.Add(lazer3);
                            bulletList.Add(lazer4);
              
                            shotsFired += 2;
                            bulletDelay = 16;

                            // Two Lazer
                            break;
                        case 8:
                            Bullet_Laser_Player lazer5 = new Bullet_Laser_Player(this, -10);
                            Bullet_Laser_Player lazer6 = new Bullet_Laser_Player(this, 8);
                            Bullet_Small small = new Bullet_Small(Content, new Vector2(crest.X + 1, crest.Y), speedBullet);
                            SM.playLazer();
                            SM.playBullet();
                            bulletList.Add(lazer5);
                            bulletList.Add(lazer6);
                            bulletList.Add(small);
                            shotsFired += 2;
                            bulletDelay = 14;
                            break;

                        default:
                             Bullet_Laser_Player lazer7 = new Bullet_Laser_Player(this, -1);
                             Bullet_Laser_Player lazer8 = new Bullet_Laser_Player(this, -10);
                             Bullet_Laser_Player lazer9 = new Bullet_Laser_Player(this, 8);
                             SM.playLazer();
                             bulletList.Add(lazer7);
                             bulletList.Add(lazer8);
                             bulletList.Add(lazer9);
                             bulletDelay = 14;
                             break;
                };
            }
        
        public void Jump(KeyboardState keyState)                                            // Causes player to "jump", even though its more like a sudden shift
        {
            bool jumped = false;
            if (jumpDelay > 0) { jumpDelay--; }                                             // Only execute if we've waited for our jump delay, other decrement jump delay

            else
            {
                if (keyState.IsKeyDown(Keys.W) && (Position.Y - jumpSize > 20))             // Jump in the direction of the player's current movement
                {
                    position.Y -= jumpSize;
                    jumped = true;
                }
                if (keyState.IsKeyDown(Keys.A) && (Position.X - jumpSize > 10))
                {
                    position.X -= jumpSize;
                    jumped = true;
                }
                if (keyState.IsKeyDown(Keys.S) && (Position.Y + jumpSize < playableHeight))
                {
                    position.Y += jumpSize;
                    jumped = true;
                }
                if (keyState.IsKeyDown(Keys.D) && (Position.X + jumpSize < playableWidth))
                {
                    position.X += jumpSize;
                    jumped = true;
                }

                if (jumped)
                {
                    SM.playJump();
                    jumpDelay = 270;                                                             // Reset delay to whatever it started as
                }
            }
        }

        public void ShootBomb()                                                                 // Spawns a bomb and adds it to bomb list
        {
            if (bombDelay > 0) { bombDelay--; } //{ bombDelay--; }                                         // Only execute if we've waited for our bomb delay, otherwise decrement delay
            else if (bombList.Count < maxBombs)                                         // Only spawn a bomb if we haven't reached our bullet cap
            {
                addedBomb = false;
                if (playerBombs > 0)
                {
                  
                    Bomb bomb11 = new Bomb(Content, crest, speedBomb);     // Create new bullet located at the player's crest with the current bullet speed (could change bullet to bullet)
                    //SM.playBullet();
                    bombList.Add(bomb11);                                                  // Add bullet to the list
                    bombDelay = 5;                                                            // Reset bullet delay to whatever it started as 
                    //shotsFired++;
                    playerBombs--;
                }
              
            }
        }

        public void UpdateBombs(GameTime gameTime) //iterates through bombs in bombList and updates each one
        {
            foreach (Bomb z in bombList)
            {
                z.Update(gameTime);
            }

            for (int i = 0; i < bombList.Count; i++)                                      // Delete bullets that are no longer visible (off the screen)
            {
                if (!bombList[i].IsVisible)
                {
                    bombList.RemoveAt(i);
                    i--;
                }
            }

            //if (enemiesKilled > 2)
            //{
            //    if (playerBombs < 1)
            //    {
            //        playerBombs++;
            //    }
            //}
            if(enemiesKilled % 100 ==0 && !addedBomb ) //each x number of enemies killed in enemyList, give two more bombs
            {
                addedBomb = true;
                playerBombs++;
            }
        }

        public void UpdateBullets(GameTime gameTime)                                        // Iterates through bullets in bullet list and updates each one
        {
            foreach (Bullet_Abstract b in bulletList)                                          // Update each bullet's position
            {
                b.Update(gameTime);
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

        public void UseHealth()
        {
            //if (currentTick % healthWeight != 0) { return;  }
            if (playerHealthPacks <= 0) { return; }
            playerHealthPacks--;
            addedHealthPack = false;
            if(armor < 100 || health < 100)
            {
                this.armor += 100;
                this.health += 100;
            }
        }

        public void UpdateHealth()
        {

            //if (enemiesKilled > 2)
            //{
            //    if (playerBombs < 1)
            //    {
            //        playerBombs++;
            //    }
            //}
            if (enemiesKilled % 70 == 0 && !addedHealthPack) //each x number of enemies killed in enemyList, give two more bombs
            {
                playerHealthPacks++;
                addedHealthPack = true;
               
            }
        }

        #endregion

        #region Helper Functions

        private bool OutOfBounds(Vector2 position)                                          // Determines if the given position is out of the playable area
        {
            bool isOutOfBounds = false;

            if (position.X < 0 || position.X > (playableWidth + borderWidth) || 
                position.Y < 0 || position.Y > (playableHeight ))                           // Looking vertically, there are two borders to account for
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

        public void AddScore(int score)
        {
            Score += score;
        }

        private void LevelUp()
        {
            this.level++;       // Increment Level
            maxBullets += 20;

            // Now adjust stats to match level

            attackModifier *= 1.05;
            armorModifier *= 0.95;

            SM.playLevelUp();
        }

        #endregion
    }
}
