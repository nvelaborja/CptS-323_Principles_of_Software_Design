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
using System.ComponentModel;

namespace Byte_Me_Bullet_Hell
{

    // Abstract class for the enemey 
    // TODO: Clean up code so the abstract class has more common functionality (make debugging easier)

    public abstract class Enemy_Abstract : Character
    {
        protected Vector2 spawnPoint; 
        protected int speedBullet;                                                              // Bullet speed (distance in pixels between each draw function after bullet fire)
        protected int bulletDelay;                                                              // Makes player wait x amount of frames before being able to fire again
        protected int maxBullets;                                                               // Maximum number of bullets that can be on a screen at one time
        protected int shootInterval;
        protected List<Bullet_Abstract> bulletList;                                             // List of bullets on the screen
        protected List<Bullet_Abstract> enemyBullets = new List<Bullet_Abstract>();
        protected int ticks;
        protected int dropTime;                                                            // count of ticks before enemy drops
        protected bool laserOut = false;
        protected Player player;
        protected int bulletMultiplier =1;


        public int BulletMultiplier {
            get 
            { return bulletMultiplier;
            }
            set { bulletMultiplier = value;  }
        }
        public List<Bullet_Abstract> Bullets
        {
            get { return bulletList; }
        }

        public int ShootInterval
        {
            get { return shootInterval; }
            set { shootInterval = value; }
        }


        // Draw the enemy and the bullets
        public virtual void Draw(SpriteBatch spriteBatch)
        {
            //Draw the enemy
            spriteBatch.Draw(texture, position, Color.White);                         // Draw Player

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

        //Update the bullets lists
        public virtual void UpdateBullets(GameTime gameTime)                                        // Iterates through bullets in bullet list and updates each one
        {
            // Make sure to iterate over the enemyBullet list, and not the universal
            // Bullet list
            foreach (Bullet_Abstract b in enemyBullets)                                                // Update each bullet's position
            {
                if (b is Bullet_Small) { (b as Bullet_Small).Update(gameTime); }
                if (b is Bullet_Med) { (b as Bullet_Med).Update(gameTime); }
                if (b is Bullet_Tracking) { (b as Bullet_Tracking).Update(gameTime); }
                if (b is Bullet_Laser)
                {
                    (b as Bullet_Laser).Update(gameTime);

                }

            }

            //Be sure to update all the bullets in enemyBullets, not 'bulletList'
            for (int i = 0; i < enemyBullets.Count; i++)
            {
                if (!enemyBullets[i].IsVisible)
                {
                    if (enemyBullets[i] is Bullet_Laser) { laserOut = false; }
                    enemyBullets.RemoveAt(i);
                    bulletList.RemoveAt(i);
                }

            }

        }

        protected abstract void managePosition(GameTime gameTime); 
        public abstract void Update(GameTime gameTime);
        public List<Bullet_Abstract> EnemyBullets { get { return enemyBullets; } set { enemyBullets = value; } }
        //public abstract void LoadContent(ContentManager Content);

        
    }
}
