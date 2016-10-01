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
    public class SettingsMenu
    {
        #region Class Members

        private Settings settings;
        private bool exit;
        private Texture2D menu;
        private Texture2D exitSelector;
        private Texture2D selector;
        private Vector2 zeroPosition = new Vector2(0, 0);
        private Vector2 audioOnPosition = new Vector2(340, 305);
        private Vector2 audioOffPosition = new Vector2(580, 305);
        private Vector2 Difficulty1Position = new Vector2(340, 510);
        private Vector2 Difficulty2Position = new Vector2(580, 510);
        private Vector2 ExitPosition = new Vector2(880, 80);
        private Rectangle audioOnPositionRect;
        private Rectangle audioOffPositionRect;
        private Rectangle Difficulty1PositionRect;
        private Rectangle Difficulty2PositionRect;
        private Rectangle ExitPositionRect;
        private bool exitHover = false;
        private bool audioOnHover = false;
        private bool audioOffHover = false;
        private bool difficulty1Hover = false;
        private bool difficulty2Hover = false;

        #endregion

        #region Constructors

        public SettingsMenu(ref Settings Settings, ContentManager Content)                                                              
        {
            settings = Settings;
            exit = false;

            menu = Content.Load<Texture2D>("SettingsMenu");
            selector = Content.Load<Texture2D>("SettingsSelector");
            exitSelector = Content.Load<Texture2D>("SettingsExitSelector");

            audioOnPositionRect = new Rectangle((int)audioOnPosition.X, (int)audioOnPosition.Y, selector.Width, selector.Height);
            audioOffPositionRect = new Rectangle((int)audioOffPosition.X, (int)audioOffPosition.Y, selector.Width, selector.Height);
            Difficulty1PositionRect = new Rectangle((int)Difficulty1Position.X, (int)Difficulty1Position.Y, selector.Width, selector.Height);
            Difficulty2PositionRect = new Rectangle((int)Difficulty2Position.X, (int)Difficulty2Position.Y, selector.Width, selector.Height);
            ExitPositionRect = new Rectangle((int)ExitPosition.X, (int)ExitPosition.Y, exitSelector.Width, exitSelector.Height);
    }

        #endregion

        #region Properties
        
        public bool Exit
        {
            get { return exit; }
        }

        public int property {
            get { return settings.Difficulty; }
        }

        #endregion

        #region MonoGame Functions


        public void Update(GameTime gameTime)                                             
        {
            MouseState mouse = Mouse.GetState();
            Rectangle mousePosition = new Rectangle(mouse.X, mouse.Y, 1, 1);

            exitHover = false;
            audioOnHover = false;
            audioOffHover = false;
            difficulty1Hover = false;
            difficulty2Hover = false;

            if (mousePosition.Intersects(audioOnPositionRect))
            {
                audioOnHover = true;
                if (mouse.LeftButton == ButtonState.Pressed && !settings.SoundOn)
                {
                    settings.SoundOn = true;
                    ContentMaster.SM.IsSoundOn = true;
                    ContentMaster.SM.playButtonPress();
                }
            }
            else if (mousePosition.Intersects(audioOffPositionRect))
            {
                audioOffHover = true;
                if (mouse.LeftButton == ButtonState.Pressed && settings.SoundOn)
                {
                    settings.SoundOn = false;
                    ContentMaster.SM.playButtonPress();
                    ContentMaster.SM.IsSoundOn = false;
                }
            }
            else if (mousePosition.Intersects(Difficulty1PositionRect))
            {
                difficulty1Hover = true;
                if (mouse.LeftButton == ButtonState.Pressed && settings.Difficulty != 1)
                {
                    settings.Difficulty = 1;
                    ContentMaster.SM.playButtonPress();
                }
            }
            else if (mousePosition.Intersects(Difficulty2PositionRect))
            {
                difficulty2Hover = true;
                if (mouse.LeftButton == ButtonState.Pressed && settings.Difficulty != 2)
                {
                    settings.Difficulty = 2;
                    ContentMaster.SM.playButtonPress();
                }
            }
            else if (mousePosition.Intersects(ExitPositionRect))
            {
                exitHover = true;
                if (mouse.LeftButton == ButtonState.Pressed && !exit)
                {
                    exit = true;
                    ContentMaster.SM.playButtonPress();
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (exitHover || exit)
            {
                spriteBatch.Draw(exitSelector, ExitPosition, Color.White);
            }
            if (audioOnHover || settings.SoundOn)
            {
                spriteBatch.Draw(selector, audioOnPosition, Color.White);
            }
            if (audioOffHover || !settings.SoundOn)
            {
                spriteBatch.Draw(selector, audioOffPosition, Color.White);
            }
            if (difficulty1Hover || settings.Difficulty == 1)
            {
                spriteBatch.Draw(selector, Difficulty1Position, Color.White);
            }
            if (difficulty2Hover || settings.Difficulty == 2)
            {
                spriteBatch.Draw(selector, Difficulty2Position, Color.White);
            }

            spriteBatch.Draw(menu, zeroPosition, Color.White);
        }

        #endregion
        
        #region Helper Functions
        

        #endregion
    }
}
