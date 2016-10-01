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
    public class MainMenu : Game
    {
        #region Class Members

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        MenuButton btnPlay;
        MenuButton btnExit;
        MenuButton btnHighScores;
        MenuButton btnSettings;
        int screenWidth = 1080, screenHeight = 720;
        private Texture2D buttonPlay;
        private Texture2D buttonExit;
        private Texture2D buttonHighScores;
        private Texture2D buttonSettings;
        private Texture2D backdrop;
        private Texture2D smoke;
        private Texture2D title;
        private Texture2D mouse;
        private Vector2 mousePosition;
        private Vector2 smokePosition;
        private int smokeDirection;
        private bool currentClick = false;
        private bool isSettings = false;
        private bool isHighscores = false;
        private SettingsMenu settingsMenu;
        private HighScoresMenu highScoresMenu;
        private Settings settings;
        private bool displayMouse = true;
        private bool soundOn;

        #endregion

        public MainMenu(ref Settings Settings)                                                                 
        {
            #region Graphics and Directory Setup

            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            #endregion

            #region Game Window Setup

            graphics.PreferredBackBufferWidth = 1080;                                   // Window Width
            graphics.PreferredBackBufferHeight = 720;                                   // Window Height
            graphics.IsFullScreen = false;                                              // Don't allow fullscreen mode
            this.Window.Title = "Byte Me Bullet Hell";                                  // Set window Title to game title
            this.Window.IsBorderless = true;                                            // Hide top title bar

            #endregion

            settings = Settings;

            smokePosition = new Vector2(0, 0);
            smokeDirection = 1;
        }

        protected override void Initialize()                                            // Initialize member variables
        {
            ContentMaster.LoadAllContent(Content, ref settings);
            ContentMaster.SM.LoopMenu();

           

            displayMouse = true;

            base.Initialize();
        }

        protected override void LoadContent()                                           // Load all member's content using their LoadContent functions
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            buttonPlay = Content.Load<Texture2D>("ButtonPlay");
            buttonExit = Content.Load<Texture2D>("ButtonExit");
            buttonHighScores = Content.Load<Texture2D>("ButtonHighScores");
            buttonSettings = Content.Load<Texture2D>("ButtonSettings");
            smoke = Content.Load<Texture2D>("Smoke");
            backdrop = Content.Load<Texture2D>("Backdrop");
            title = Content.Load<Texture2D>("MenuTitle");
            mouse = Content.Load<Texture2D>("PlayerTexture");

            // Set up menu buttons
            btnPlay = new MenuButton(buttonPlay, graphics.GraphicsDevice);
            btnPlay.setPosition(new Vector2((1080 / 2) - buttonPlay.Width / 2, (720 / 6) * 1 + 25));
            btnExit = new MenuButton(buttonExit, graphics.GraphicsDevice);
            btnExit.setPosition(new Vector2((1080 / 2) - buttonExit.Width / 2, (720 / 6) * 2 + 25));
            btnHighScores = new MenuButton(buttonHighScores, graphics.GraphicsDevice);
            btnHighScores.setPosition(new Vector2((1080 / 2) - buttonHighScores.Width / 2, (720 / 6) * 3 + 25));
            btnSettings = new MenuButton(buttonSettings, graphics.GraphicsDevice);
            btnSettings.setPosition(new Vector2((1080 / 2) - buttonSettings.Width / 2, (720 / 6) * 4 + 25));
        }


        protected override void UnloadContent()                                         // Unload Content before closing game ( I think only needed when multiple scenes are used )
        {
            Content.Unload();
            base.Exit();
        }

        protected override void Update(GameTime gameTime)                               // Calls all member's update functions (happens once per frame, caps at 60fps)
        {
            MouseState mouseState = Mouse.GetState();
            mousePosition = mouseState.Position.ToVector2();
            mousePosition.X -= mouse.Width / 2;

            if (!isSettings && !isHighscores)                   // Neither high scores nor settings menu open
            {
                btnPlay.Update(mouseState);
                btnExit.Update(mouseState);
                btnHighScores.Update(mouseState);
                btnSettings.Update(mouseState);

                if (btnPlay.isClicked)
                {
                    if (!currentClick)
                    {
                        currentClick = true;
                        ContentMaster.SM.playButtonPress();
                    }
                    bool done = ContentMaster.SM.FadeLoop();
                    if (done)
                    {
                        currentClick = false;
                        UnloadContent();
                        Game1 game = new Game1(ref settings);
                        game.Run();
                    }
                }
                else if (btnExit.isClicked)
                {
                    if (!currentClick)
                    {
                        currentClick = true;
                        ContentMaster.SM.playButtonPress();
                    }
                    bool done = ContentMaster.SM.FadeLoop();
                    if (done)
                    {
                        currentClick = false;
                        UnloadContent();
                        Exit();
                    }
                }
                else if (btnHighScores.isClicked)
                {
                    if (!currentClick)
                    {
                        currentClick = true;
                        ContentMaster.SM.playButtonPress();
                        highScoresMenu = new HighScoresMenu( Content);
                        isHighscores = true;
                        btnHighScores.isClicked = false;
                    }
                }
                else if (btnSettings.isClicked)
                {
                    if (!currentClick)
                    {
                        currentClick = true;
                        ContentMaster.SM.playButtonPress();
                        settingsMenu = new SettingsMenu(ref settings, Content);
                        isSettings = true;
                        btnSettings.isClicked = false;
                    }
                }
            }
            else if (!isHighscores)                                     // Settings menu open
            {
                settingsMenu.Update(gameTime);

                if (settingsMenu.Exit)
                {
                    isSettings = false;
                    settingsMenu = null;
                    currentClick = false;
                }
            }
            else                                                       // High scores menu open
            {
                highScoresMenu.Update(gameTime);

                if (highScoresMenu.Exit)
                {
                    isHighscores = false;
                    highScoresMenu = null;
                    currentClick = false;
                }
            }

            UpdateSmokePosition();

            base.Update(gameTime);
        }
        
        protected override void Draw(GameTime gameTime)                                 // Actually draws elements onto the window so we can see them
        {
            GraphicsDevice.Clear(Color.Gray);                                           // Clears window every frame with Gray background

            spriteBatch.Begin();                                                        // Starts spritebatch, the object that contains all the drawing functions

            spriteBatch.Draw(backdrop, ContentMaster.zeroPosition, Color.White);
            spriteBatch.Draw(smoke, smokePosition, Color.White);

            if (!isSettings)
            {
                btnPlay.Draw(spriteBatch);
                btnExit.Draw(spriteBatch);
                btnHighScores.Draw(spriteBatch);
                btnSettings.Draw(spriteBatch);
            }

            spriteBatch.Draw(title, new Vector2(0, 0), Color.White);

            if (isSettings)
            {
                settingsMenu.Draw(spriteBatch);
            }
            else if (isHighscores)
            {
                highScoresMenu.Draw(spriteBatch);
            }
            
            if (displayMouse)
            {
                spriteBatch.Draw(mouse, mousePosition, Color.White);
            }

            spriteBatch.End();                                                          // Must end the spritebatch after each frame (not sure why)

            base.Draw(gameTime);                                                        // TBH, no idea what this does
        }
        
        private void UpdateSmokePosition()
        {
            double position = smokePosition.Y;

            switch (smokeDirection)
            {
                case 1:                                                                // Going up
                    if (smokePosition.Y > -5 || smokePosition.Y < -715)
                    {
                        position += 0.05;
                    }
                    else if (smokePosition.Y > -10 || smokePosition.Y < -710)
                    {
                        position += 0.1;
                    }
                    else if (smokePosition.Y > -20 || smokePosition.Y < -700)
                    {
                        position += 0.2;
                    }
                    else if (smokePosition.Y > -30 || smokePosition.Y < -690)
                    {
                        position += 0.3;
                    }
                    else if (smokePosition.Y > -40 || smokePosition.Y < -680)
                    {
                        position += 0.4;
                    }
                    else position += 0.5;
                    break;
                case -1:                                                                 // Going up
                    if (smokePosition.Y > -5 || smokePosition.Y < -715)
                    {
                        position -= 0.05;
                    }
                    else if (smokePosition.Y > -10 || smokePosition.Y < -710)
                    {
                        position -= 0.1;
                    }
                    else if (smokePosition.Y > -20 || smokePosition.Y < -700)
                    {
                        position -= 0.2;
                    }
                    else if (smokePosition.Y > -30 || smokePosition.Y < -690)
                    {
                        position -= 0.3;
                    }
                    else if (smokePosition.Y > -40 || smokePosition.Y < -680)
                    {
                        position -= 0.4;
                    }
                    else position -= 0.5;
                    break;
            }

            smokePosition.Y = (float) position;

            if (smokePosition.Y > 0)
            {
                smokePosition.Y = 0;
                smokeDirection = -smokeDirection;
            }
            else if (smokePosition.Y < -720)
            {
                smokePosition.Y = -720;
                smokeDirection = -smokeDirection;
            }
        }
    }
}
