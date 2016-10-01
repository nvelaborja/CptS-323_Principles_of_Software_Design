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
    // Game1 will own this Collision handler and have a reference to player and all enemies. Each frame, it will check to see if any player bullets have 
    //      collided with any enemies, or if any enemy bullets have collided with the player. By this logic, enemies cannot hurt other enemies.

    class CollisionHandler                                                                                  
    {
        private List<Enemy_Abstract> enemyList;
        private Player player;
        private SoundManager SM;
        private AI hal9000;
        private int physicalDamage = 2;
        private int physicalDamageDelay = 100;

        public CollisionHandler(ref List<Enemy_Abstract> enemyList, ref Player player, ref SoundManager SM, ref AI hal9000)
        {
            this.enemyList = enemyList;                                         // Inherit a reference to Game1's list of enemies. Since this is a refrence, it should only contain current enemies
            this.player = player;                                               // Inherit a reference to the player
            this.SM = ContentMaster.SM;                                                       // Use Game1's SM to save some memory
            this.hal9000 = hal9000;
        }

        public void CheckCollisions()
        {
            // First check to see if player's bullets have hit an enemy. By this logic, the player will win any tie scenarios with enemies.
            foreach (Bullet_Abstract bullet in player.BulletList)
            {
                foreach (Enemy_Abstract enemy in enemyList)
                {
                    if (Intersects(bullet.HitBox, enemy.HitBox))
                    {
                        if (enemy is Enemy_Large)
                        {
                            //enemy.Health = 1;
                            int x = 5;
                        }
                        SM.playImpact();
                        enemy.TakeDamage((int) (bullet.Power * player.AttackModifier));
                        bullet.IsVisible = false;
                        player.ShotsHit++;
                    }
                }
            }

            foreach (Bullet_Abstract bomb in player.BombList)
            {
                foreach (Enemy_Abstract enemy in enemyList)
                {
                    if (Intersects(bomb.HitBox, enemy.HitBox))
                    {
                        //SM.playImpact(); //make explosion sound
                        enemy.TakeDamage((int)(bomb.Power * player.AttackModifier));
                        bomb.IsVisible = false;
                        //player.BombsHit++;
                    }
                }
<<<<<<< HEAD
            }

            // For some reason, each enemy has all enemies bullets in their personal bullet lists, so only go through the first enemies list
            if (enemyList.Count > 0)
            {
                foreach (Enemy_Abstract enemy in enemyList)
                {
                    foreach (Bullet_Abstract bullet in enemy.EnemyBullets)
                    {
                        if (Intersects(bullet.HitBox, player.HitBox))
                        {
                            //if(shieldActive == false)
                            //{
                            //    SM.playHurt();
                            //    player.TakeDamage((int)(bullet.Power * enemy.AttackModifier));
                            //    bullet.IsVisible = false;
                            //}
                            //else
                            //{
                            //    bullet.IsVisible = false;
                            //}
                            SM.playHurt();
                            player.TakeDamage((int)(bullet.Power * enemy.AttackModifier));
                            bullet.IsVisible = false;
                        }
                    }

=======
            }

            // For some reason, each enemy has all enemies bullets in their personal bullet lists, so only go through the first enemies list
            if (enemyList.Count > 0)
            {
                foreach (Enemy_Abstract enemy in enemyList)
                {
                    foreach (Bullet_Abstract bullet in enemy.EnemyBullets)
                    {
                        if (Intersects(bullet.HitBox, player.HitBox))
                        {
                            SM.playHurt();
                            player.TakeDamage((int)(bullet.Power * enemy.AttackModifier));
                            bullet.IsVisible = false;
                        }
                    }

>>>>>>> b6d8685a10858716d6a68a71b02869343d9b0a45
                    //Check if there is physical damage
                    if (Intersects(enemy.HitBox, player.HitBox))
                    {
                        this.player.takePhysicalDamage = true;
                        enemy.takePhysicalDamage = true;
                    }

                    foreach (Enemy_Abstract lurker in enemyList)
                    {
                        if (ReferenceEquals(lurker, enemy)) { continue; }
                        if (!(lurker is Enemy_Lurker)) { continue; }
                        if (Intersects(enemy.HitBox, lurker.HitBox))
                        {
                            (lurker as Enemy_Lurker).changePosition();
                        }
                    }

                }


            }

            // Finally, if the boss is out, check to see if it is hitting the player and vice versa
            if (hal9000.isBossOut)
            {
                Boss boss = hal9000.Boss;

                foreach (Bullet_Abstract bullet in player.BulletList)
                {
                    if (Intersects(bullet.HitBox, boss.HitBox))
                    {
                        SM.playBulletHit();
                        boss.TakeDamage((int)(bullet.Power * player.AttackModifier));
                        bullet.IsVisible = false;
                    }
                }

                foreach (Bullet_Abstract bullet in boss.Bullets)
                {
                    if (Intersects(bullet.HitBox, player.HitBox))
                    {
                        SM.playImpact();
                        SM.playHurt();
                        player.TakeDamage((int)(bullet.Power * boss.AttackModifier));
                        bullet.IsVisible = false;
                    }
                }

                foreach (Bullet_Abstract bomb in player.BombList)
                {
                    if (Intersects(bomb.HitBox, boss.HitBox))
                    {
                        //SM.playBombHit();
                        boss.TakeDamage((int)(bomb.Power * player.AttackModifier));
                        bomb.IsVisible = false;
                    }
                }


            }
        }

        private bool Intersects(Rectangle one, Rectangle two)
        {
            if (two.Intersects(one) || one.Intersects(two))
            {
                return true;
            }

            return false;
        }
    }
}
