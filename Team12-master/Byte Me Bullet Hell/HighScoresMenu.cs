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
using System.IO;

namespace Byte_Me_Bullet_Hell
{
    public class HighScoresMenu
    {
        #region Class Members
        
        private bool exit;
        private Texture2D menu;
        private Texture2D exitSelector;
        private Vector2 ExitPosition = new Vector2(880, 80);
        private Vector2 zeroPosition = new Vector2(0, 0);
        private Vector2 textStartPosition = new Vector2(200, 190);
        private Rectangle ExitPositionRect;
        private SpriteFont font;
        private bool exitHover = false;
        private List<int> HighScores;
        private bool loadError = false;

        #endregion

        #region Constructors

        public HighScoresMenu(ContentManager Content)
        {
            exit = false;

            menu = Content.Load<Texture2D>("HighScores");
            exitSelector = Content.Load<Texture2D>("SettingsExitSelector");
            font = Content.Load<SpriteFont>("HealthFont");

            ExitPositionRect = new Rectangle((int)ExitPosition.X, (int)ExitPosition.Y, exitSelector.Width, exitSelector.Height);

            HighScores = new List<int>();
            LoadHighScores();
        }

        #endregion

        #region Properties

        public bool Exit
        {
            get { return exit; }
        }

        #endregion

        #region MonoGame Functions


        public void Update(GameTime gameTime)
        {
            MouseState mouse = Mouse.GetState();
            Rectangle mousePosition = new Rectangle(mouse.X, mouse.Y, 1, 1);

            exitHover = false;

            if (mousePosition.Intersects(ExitPositionRect))
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

            spriteBatch.Draw(menu, zeroPosition, Color.White);

            if (loadError)
            {
                spriteBatch.DrawString(font, "Error Loading Save Data", textStartPosition, Color.White);
            }
            else
            {
                int columns = 0;

                for (int i = 0; i < HighScores.Count; i++)
                {
                    if (i == 85) break;

                    if (i / (17 * (columns + 1)) >= 1)
                    {
                        columns++;
                    }
                    string line = (i + 1).ToString() + ". " + HighScores[i].ToString();
                    spriteBatch.DrawString(font, line, new Vector2(textStartPosition.X + (150 * columns), textStartPosition.Y + ((i - 17 * columns) * 25)), Color.Black);
                }
            }
        }

        #endregion

        #region Helper Functions

        private void LoadHighScores()
        {
            string fileName = "HighScores.txt";
            string fullPath = AppDomain.CurrentDomain.BaseDirectory;

            for (int i = 0; i < fullPath.Length; i++)
            {
                if (fullPath[i] == 'b' && fullPath[i+1] == 'i' && fullPath[i+2] == 'n')
                {
                    fullPath = fullPath.Substring(0, i) + fileName;
                    break;
                }
            }

            if (File.Exists(fullPath))
            {
                List<string> strings = File.ReadAllLines(fullPath).ToList();

                foreach(string score in strings)
                {
                    if (score != "")
                    {
                        HighScores.Add(Convert.ToInt32(score));
                    }
                }

                HighScores.Sort();
                HighScores.Reverse();
            }
            else
            {
                loadError = true;
            }
        }

        #endregion
    }
}
