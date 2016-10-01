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
    // A Class responsible for managing the input and termination of enemy and boss characters into and out of the game. 
    public class AI : INotifyPropertyChanged
    {
        private List<Enemy_Abstract> enemies;
        private Boss boss;
        private Player player;                          // A refrence to the plater
        private int wave;                               // How many stages are there? This number should be played with 
        private const int waveTimerBase = 3000;         // Base wave time until next wave, total wave time depends on current wave number
        private int waveTimer;
        private uint currentTick;
        private bool bossOut = false;
        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        private int difficultyAdjuter;
        private int difficulty = 1;
        private Settings settings;
        private bool bossDead = false;

        // The AI needs a refrence to the player, to know what it is the player is up to
        public AI(ref Player player, ref Settings Settings) 
        {
            this.enemies = new List<Enemy_Abstract>();
            this.player = player;
            wave = 0; 
            currentTick = 0;
            waveTimer = waveTimerBase;
            difficultyAdjuter =  (int)(player.Level * 10) * 2 * difficulty;
            this.settings = Settings;
            difficulty = settings.Difficulty;
        }

        //Create the appropriate number and type of enemies
        private void createEnemies()
        {
            int enemy1 = 0;
            int enemy2 = 0;
            int enemy3 = 0;
            bool summonBoss = false;

            // A highly sophisticated algorithm could be implemented here consuming only two lines and is highly flexible to changes
            // Or a case statement could be used... consuming 1/10 the brian power
            switch(this.wave)
            {
                case 0:
                    enemy1 = 6;
                    
                    break;
                case 1:
                    enemy1 = 11;
                    break;
 
                case 2:
                    enemy1 = 20;
                    enemy2 = 3;

                    break;

                case 3:
                    enemy1 = 20;
                    enemy2 = 4;
                    enemy3 = 1;
                    break; 

                case 4:
                    enemy1 = 25;
                    enemy2 = 4;
                    enemy3 = 3;
                    break; 
                    
                case 5:
                    enemy1 = 20;
                    enemy2 = 10;
                    enemy3 = 6;
                    break;

                case 6:
                    enemy1 = 55;
                    break;

                case 7:
                     enemy1 = 20;
                    enemy2 = 10;
                    enemy3 = 6;
                    break;
                case 8:
                    summonBoss = true;
                    break;

                default:
                    break;
            }

            for (int i = 0; i < enemy1*difficulty; i++)            // Spawn appropriate number of enemy1
            {
                Enemy_Peon smallEnemy =
                    new Enemy_Peon(new Vector2(ContentMaster.randy.Next() % ContentMaster.playableWidth, 1), player);
                smallEnemy.ShootInterval -= ContentMaster.randy.Next() % difficultyAdjuter;
                smallEnemy.Health += ContentMaster.randy.Next() % difficultyAdjuter;
                smallEnemy.Armor += ContentMaster.randy.Next() % difficultyAdjuter;
                smallEnemy.ArmorModifier += ContentMaster.randy.Next() % difficultyAdjuter/100;
                smallEnemy.Speed += ContentMaster.randy.Next() % difficultyAdjuter;
                smallEnemy.BulletMultiplier = ContentMaster.randy.Next() % difficultyAdjuter;
                //smallEnemy.PointValue += (int)(this.player.Level * 3);
                enemies.Add(smallEnemy);
               
                smallEnemy.PropertyChanged += EnemyDied;
            }

            for (int i=0; i< enemy2*difficulty; i++)               // Spawn appropriate number of enemy2
            {
                Enemy_Lurker BiggerEnemy = 
                          new Enemy_Lurker(new Vector2(ContentMaster.randy.Next() % ContentMaster.playableWidth, 1), player);
                BiggerEnemy.Health += ContentMaster.randy.Next() % difficultyAdjuter;
                BiggerEnemy.Armor +=  ContentMaster.randy.Next() % difficultyAdjuter;
                BiggerEnemy.ArmorModifier += ContentMaster.randy.Next() % difficultyAdjuter/100;
                //BiggerEnemy.ArmorModifier += ContentMaster.randy.Next() % difficultyAdjuter / 50;

                BiggerEnemy.BulletMultiplier = ContentMaster.randy.Next() % difficultyAdjuter;
                //BiggerEnemy.PointValue += (int)(this.player.Level * 3);
                enemies.Add(BiggerEnemy);
                BiggerEnemy.PropertyChanged += EnemyDied;
            }

            for (int i=0; i<enemy3*difficulty; i++)                // Spawn appropriate number of enemy3 TODO: there is no enemy 3 is there?
            {
                Enemy_Large biggestEnemy = 
                    new Enemy_Large(new Vector2(ContentMaster.randy.Next() % ContentMaster.playableWidth, 1),  player);
                biggestEnemy.Health += ContentMaster.randy.Next() % difficultyAdjuter*3;
                biggestEnemy.Armor += ContentMaster.randy.Next()%difficultyAdjuter;
                biggestEnemy.ArmorModifier += ContentMaster.randy.Next() % difficultyAdjuter;
                biggestEnemy.BulletMultiplier = ContentMaster.randy.Next() % difficultyAdjuter/100;
                //biggestEnemy.PointValue += (int)(player.Level * 100);
                biggestEnemy.Health = 1;
                biggestEnemy.Armor = 1;
                enemies.Add(biggestEnemy);
                biggestEnemy.PropertyChanged += EnemyDied;
            }

            if (summonBoss && !bossOut)                 // Spawn boss if the case requires so and the boss is not already out
            {
                bossOut = true;
                boss = new Boss(new Vector2((ContentMaster.playableWidth / 2) , (ContentMaster.playableHeight / 2) ), player);
                boss.PropertyChanged += EnemyDied;
            }
        }

        //update called by the Game ckass
        public void update(GameTime gameTime)
        {
            currentTick++;

            if (enemies.Count == 0 && !bossOut) {       //If there are no more enemies left on the screen: // 1) increase the wave count // 2)  create more enemies // 3) updae wave timer
                waveTimer = waveTimerBase - wave * 100;
                createEnemies();
                wave++;
            }

            if (currentTick % waveTimer == 0) {         //If the timer is up, create enemies, do not update wave
                createEnemies();
            }

            if (bossOut)
            {
                boss.Update(gameTime);
                if (boss.IsDead)
                {
                    bossDead = true;
                }
            }     // update boss

            for (int i = 0; i < Enemies.Count; i++)     // updated enemies
            {
                Enemies[i].Update(gameTime);
                if (!Enemies[i].IsVisible || Enemies[i].Health <= 0)
                {
                    Enemies.RemoveAt(i);
                    i--;
                }
            }
        }

        //developed primarily for testing purposes
        public void removeAllEnemies()
        {
            for(int i =enemies.Count-1; i>=0; i--)
            {
                enemies.RemoveAt(i);
            }
        }

        // While it is possible to set the enemies, in theory this property should be read only...
        public List<Enemy_Abstract> Enemies
        {
            get { return enemies; }
            set { enemies = value; }
        } 

        public Boss Boss
        {
            get { return boss;  }
            set { boss = value;  }
        }

        public bool isBossOut
        {
            get { return bossOut;  }
        }

        public bool BossDead
        {
            get { return bossDead; }
        }

        private void EnemyDied(object sender, PropertyChangedEventArgs e)
        {
            Character enemy = sender as Character;

            player.AddScore(enemy.PointValue);
            player.EnemiesKilled++;
        }
    }
}
