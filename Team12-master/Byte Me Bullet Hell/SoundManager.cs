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
    public class SoundManager
    {
        // Initialize sound effect types 
        public SoundEffect bullet, impact, ByteMeTheme, bulletHit, laserHit, death, inGame, menu, playerHurt, jump, lazer, buttonHover, buttonPress, dying, deathMenu, levelUp, pause;
        public List<SoundEffectInstance> soundList = new List<SoundEffectInstance>();
        public SoundEffectInstance currentInstance;
        private Settings settings;

        public SoundManager(ref Settings Settings)
        {
           settings = Settings;
        }

        public void LoadContent(ContentManager Content)
        {
            // Load WAV files for each sound effect type
            bullet = Content.Load<SoundEffect>("BMBH_Bullet");
            impact = Content.Load<SoundEffect>("BMBH_Impact");
            bulletHit = Content.Load<SoundEffect>("BMBH_BulletHit");
            death = Content.Load<SoundEffect>("BMBH_Death");
            inGame = Content.Load<SoundEffect>("BMBH_InGame");
            menu = Content.Load<SoundEffect>("BMBH_Menu");
            playerHurt = Content.Load<SoundEffect>("BMBH_PlayerHurt");
            jump = Content.Load<SoundEffect>("BMBH_Jump");
            lazer = Content.Load<SoundEffect>("BMBH_Lazer");
            buttonHover = Content.Load<SoundEffect>("BMBH_ButtonHover");
            buttonPress = Content.Load<SoundEffect>("BMBH_ButtonPress");
            dying = Content.Load<SoundEffect>("BMBH_Dying");
            deathMenu = Content.Load<SoundEffect>("BMBH_DeathMenu");
            levelUp = Content.Load<SoundEffect>("BMBH_LevelUp");
            pause = Content.Load<SoundEffect>("BMBH_Pause");
        }
        
        public bool IsSoundOn
        {
            get { return settings.SoundOn; }
            set
            {
                settings.SoundOn = value;
                if (!settings.SoundOn)
                {
                    if (currentInstance != null)
                    {
                        currentInstance.Pause();
                    }
                }
                else
                {
                    if (currentInstance != null)
                    {
                        currentInstance.Play();
                    }
                }
            }
        }

        // The following functions call a single iteration of the sound effect
        # region Plays

        public void playHurt()
        {
            if (settings.SoundOn)
            {
                SoundEffectInstance instance = playerHurt.CreateInstance();
                instance.IsLooped = false;
                instance.Play();
            }
        }

        public void playBullet()
        {
            if (settings.SoundOn)
            {
                SoundEffectInstance instance = bullet.CreateInstance();
                instance.IsLooped = false;
                instance.Volume = 0.2f;
                instance.Play();
            }
        }

        public void playImpact()
        {
            if (settings.SoundOn)
            {
                SoundEffectInstance instance = impact.CreateInstance();
                instance.IsLooped = false;
                instance.Volume = 0.8f;
                instance.Play();
            }
        }

        public void playBMTheme()
        {
            // Will implement once we create a theme and splash screen
        }

        public void playBulletHit()
        {
            if (settings.SoundOn)
            {
                SoundEffectInstance instance = bulletHit.CreateInstance();
                instance.IsLooped = false;
                instance.Play();
            }
        }

        public void playDeath()
        {
            if (settings.SoundOn)
            {
                SoundEffectInstance instance = death.CreateInstance();
                instance.IsLooped = false;
                instance.Play();
            }
        }

        public void playInGame()
        {
            if (settings.SoundOn)
            {
                SoundEffectInstance instance = inGame.CreateInstance();
                instance.IsLooped = false;
                instance.Play();
            }
        }

        public void playMenu()
        {
            if (settings.SoundOn)
            {
                SoundEffectInstance instance = menu.CreateInstance();
                instance.IsLooped = false;
                instance.Play();
            }
        }

        public void playJump()
        {
            if (settings.SoundOn)
            {
                SoundEffectInstance instance = jump.CreateInstance();
                instance.IsLooped = false;
                instance.Play();
            }
        }

        public void playLazer()
        {
            if (settings.SoundOn)
            {
                SoundEffectInstance instance = lazer.CreateInstance();
                instance.IsLooped = false;
                instance.Play();
            }
        }

        public void playButtonHover()
        {
            if (settings.SoundOn)
            {
                SoundEffectInstance instance = buttonHover.CreateInstance();
                instance.Volume = 0.03f;
                instance.IsLooped = false;
                instance.Play();
            }
        }

        public void playButtonPress()
        {
            if (settings.SoundOn)
            {
                SoundEffectInstance instance = buttonPress.CreateInstance();
                instance.Volume = 0.7f;
                instance.IsLooped = false;
                instance.Play();
            }
        }

        public void playDying()
        {
            if (settings.SoundOn)
            {
                SoundEffectInstance instance = dying.CreateInstance();
                instance.IsLooped = false;
                instance.Play();
            }
        }

        public void playDeathMenu()
        {
            if (settings.SoundOn)
            {
                SoundEffectInstance instance = deathMenu.CreateInstance();
                instance.IsLooped = false;
                instance.Play();
            }
        }

        public void playLevelUp()
        {
            if (settings.SoundOn)
            {
                SoundEffectInstance instance = levelUp.CreateInstance();
                instance.IsLooped = false;
                instance.Play();
            }
        }

        public void playPause()
        {
            if (settings.SoundOn)
            {
                SoundEffectInstance instance = pause.CreateInstance();
                instance.IsLooped = false;
                instance.Play();
            }
        }

        #endregion

        // The following functions call an endless loop of the sound effect
        #region Loops

        public void LoopMenu()
        {
            if (settings.SoundOn)
            {
                SoundEffectInstance instance;
                instance = menu.CreateInstance();
                instance.IsLooped = true;
                instance.Play();
                currentInstance = instance;
            }
       }

        public void LoopInGame()
        {
            if (settings.SoundOn)
            {
                SoundEffectInstance instance;
                instance = inGame.CreateInstance();
                instance.IsLooped = true;
                instance.Play();
                currentInstance = instance;
            }
        }

        public void LoopDeath()
        {
            if (settings.SoundOn)
            {
                SoundEffectInstance instance;
                instance = death.CreateInstance();
                instance.IsLooped = true;
                currentInstance = instance;
                currentInstance.Play();
            }
        }

        public void LoopDeathMenu()
        {
            if (settings.SoundOn)
            {
                SoundEffectInstance instance;
                instance = deathMenu.CreateInstance();
                instance.IsLooped = true;
                instance.Volume = 0.7f;
                currentInstance = instance;
                currentInstance.Play();
            }
        }

        public bool FadeLoop()
        {
            if (settings.SoundOn)
            {
                if (currentInstance != null)
                {
                    if (currentInstance.Volume > 0.01f)
                    {
                        currentInstance.Volume = currentInstance.Volume - 0.01f;
                        return false;
                    }
                    else
                    {
                        currentInstance.Stop();
                        return true;
                    }
                }
            }
            return true;
        }

        #endregion

        public void StopCurrent()
        {
            if (settings.SoundOn)
            {
                currentInstance.Stop();
            }
        }
    }
}



