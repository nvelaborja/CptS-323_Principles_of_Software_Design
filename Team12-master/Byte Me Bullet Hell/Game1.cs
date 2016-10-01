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
    public class Game1 : Game
    {
        #region Class Members
        
        GraphicsDeviceManager graphics;
        AI hal9000;
        SpriteBatch spriteBatch;
        Player Player1;
        BackGround Overlay;
        List<Character> RemoveCharacterList;                                            // A list of character classes that are dead and need to be removed at the end of this frame
        CollisionHandler CH;
        uint waitPeriod = 100;
        uint currentWait = 100;
        bool isGameOver = false;
        bool isPaused = false;
        int screenWidth = 1080, screenHeight = 720;
        Vector2 smokePosition;
        int smokeDirection;
        Texture2D smoke;
        Texture2D darken;
        MenuButton btnResume;
        MenuButton btnExit;
        MenuButton btnSettings;
        private Texture2D buttonResume;
        private Texture2D buttonExit;
        private Texture2D buttonSettings;
        private Texture2D pauseTitle;
        private bool currentClick;
        private bool isSettings = false;
        private SettingsMenu settingsMenu;
        private bool displayMouse;
        private Texture2D mouseTexture;
        private Vector2 mousePosition;
        private bool saveError;
        private Settings settings;
        private bool bossAcquired = false;

        #endregion

        public Game1(ref Settings Settings)                                                                  // Game1 constructor, called when game is loaded by Program
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
            displayMouse = false;

            settings = Settings;

            #endregion
        }

        protected override void Initialize()                                            // Initialize member variables
        {
            ContentMaster.LoadAllContent(Content, ref settings);

            Player1 = new Player(ref settings);
            Overlay = new BackGround(graphics.GraphicsDevice, ref Player1, ref settings);
            RemoveCharacterList = new List<Character>();
            hal9000 = new AI(ref Player1, ref settings);
            ContentMaster.SM.LoopInGame();
            smokePosition = new Vector2(0, 0);
            smokeDirection = 1;

           base.Initialize();
        }

        protected override void LoadContent()                                           // Load all member's content using their LoadContent functions
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            Overlay.LoadContent(Content);                                               // please use universal class
            
            displayMouse = false;
            List<Enemy_Abstract> enemies = hal9000.Enemies;
            CH = new CollisionHandler( ref enemies, ref Player1, ref ContentMaster.SM, ref hal9000);
            smoke = Content.Load<Texture2D>("Smoke");
            darken = new Texture2D(GraphicsDevice, screenWidth, screenHeight);
            buttonResume = Content.Load<Texture2D>("ButtonResume");
            buttonExit = Content.Load<Texture2D>("ButtonExit");
            buttonSettings = Content.Load<Texture2D>("ButtonSettings");
            mouseTexture = Content.Load<Texture2D>("PlayerTexture");
            pauseTitle = Content.Load<Texture2D>("PauseTitle");

            btnResume = new MenuButton(buttonResume, graphics.GraphicsDevice);
            btnResume.setPosition(new Vector2((1080 / 2) - buttonResume.Width / 2, (720 / 4) * 1));
            btnExit = new MenuButton(buttonExit, graphics.GraphicsDevice);
            btnExit.setPosition(new Vector2((1080 / 2) - buttonExit.Width / 2, (720 / 4) * 2));
            btnSettings = new MenuButton(buttonSettings, graphics.GraphicsDevice);
            btnSettings.setPosition(new Vector2((1080 / 2) - buttonSettings.Width / 2, (720 / 4) * 3));
        }


        protected override void UnloadContent()                                         // Unload Content before closing game ( I think only needed when multiple scenes are used )
        {
            this.Exit();
        }

        protected override void Update(GameTime gameTime)                               // Calls all member's update functions (happens once per frame, caps at 60fps)
        {
            if (hal9000.isBossOut && !bossAcquired)
            {
                Overlay.Boss = hal9000.Boss;
                bossAcquired = true;
            }

            if (hal9000.BossDead)
            {
                isGameOver = true;
            }


            if (!isPaused)
            {
                if (isGameOver)
                {
                    GameOver();
                }

                if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                {
                    ContentMaster.SM.playPause();
                    Pause();
                }

                currentWait++;
                if (Keyboard.GetState().IsKeyDown(Keys.B) && currentWait >= waitPeriod)
                {
                    hal9000.removeAllEnemies();
                    currentWait = 0;
                }

                Player1.Update(gameTime);                                                   // Update Player
                hal9000.update(gameTime);                                                   // update the AI
                Overlay.Update(gameTime);                                                   // Update Overlay
                CH.CheckCollisions();
                UpdateSmokePosition();

                if (Player1.IsDead)
                {
                    isGameOver = true;
                }
            }
            else
            {
                MouseState mouse = Mouse.GetState();
                mousePosition = mouse.Position.ToVector2();
                mousePosition.X -= mouseTexture.Width / 2;

                if (!isSettings)
                {

                    btnResume.Update(mouse);
                    btnExit.Update(mouse);
                    btnSettings.Update(mouse);


                    if (btnResume.isClicked)
                    {
                        if (!currentClick)
                        {
                            currentClick = true;
                            ContentMaster.SM.playButtonPress();
                        }
                        bool done = ContentMaster.SM.FadeLoop();
                        if (done)
                        {
                            ContentMaster.SM.LoopInGame();
                            currentClick = false;
                            Pause();
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
                            MainMenu menu = new MainMenu(ref settings);
                            menu.Run();
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
                else
                {
                    settingsMenu.Update(gameTime);

                    if (settingsMenu.Exit)
                    {
                        isSettings = false;
                        settingsMenu = null;
                        currentClick = false;
                    }
                }
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)                                 // Actually draws elements onto the window so we can see them
        {
            GraphicsDevice.Clear(Color.Gray);                                           // Clears window every frame with Gray background

            spriteBatch.Begin();                                                        // Starts spritebatch, the object that contains all the drawing functions

            spriteBatch.Draw(smoke, smokePosition, Color.White);

            Player1.Draw(spriteBatch);

            if (hal9000.isBossOut)
            {
                hal9000.Boss.Draw(spriteBatch);
            }

            foreach (Enemy_Abstract Enemy in hal9000.Enemies)
            {
                if (Enemy is Enemy_Large) { (Enemy as Enemy_Large).Draw(spriteBatch); }
                if (Enemy is Enemy_Lurker) { (Enemy as Enemy_Lurker).Draw(spriteBatch); }
                if (Enemy is Enemy_Peon) { (Enemy as Enemy_Peon).Draw(spriteBatch); }
              
            }

            Overlay.Draw(spriteBatch);                                                  // Draw overlay, must be drawn last so it will be on top of everything else

            if (isPaused)
            {
                Color[] darkenData = new Color[darken.Width * darken.Height];
                for (int i = 0; i < darkenData.Length; i++) darkenData[i] = new Color(Color.Black, 150);
                darken.SetData(darkenData);

                spriteBatch.Draw(darken, new Vector2(0, 0), Color.White);

                spriteBatch.Draw(pauseTitle, new Vector2(0, 0), Color.White);

                if (isSettings)
                {
                    settingsMenu.Draw(spriteBatch);
                }
                else
                {
                    btnResume.Draw(spriteBatch);
                    btnExit.Draw(spriteBatch);
                    btnSettings.Draw(spriteBatch);
                }

                if (displayMouse)
                {
                    spriteBatch.Draw(mouseTexture, mousePosition, Color.White);
                }
               
            }

            spriteBatch.End();                                                          // Must end the spritebatch after each frame (not sure why)

            base.Draw(gameTime);                                                        // TBH, no idea what this does
        }

        private void GameOver()
        {
            bool done = ContentMaster.SM.FadeLoop();
            if (done)
            {
                SaveHighScore();

                displayMouse = true;
                UnloadContent();
                DeathMenu deathMenu = new DeathMenu(ref settings);
                deathMenu.Run();
            }
        }

        private void Quit()
        {
            ContentMaster.SM.StopCurrent();
            UnloadContent();
        }

        private void Pause()
        {
            isPaused = !isPaused;
            displayMouse = !displayMouse;

            btnExit.isClicked = false;
            btnResume.isClicked = false;
            btnSettings.isClicked = false;

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

            smokePosition.Y = (float)position;

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

        private void SaveHighScore()
        {
            string fileName = "HighScores.txt";
            string fullPath = AppDomain.CurrentDomain.BaseDirectory;

            for (int i = 0; i < fullPath.Length; i++)
            {
                if (fullPath[i] == 'b' && fullPath[i + 1] == 'i' && fullPath[i + 2] == 'n')
                {
                    fullPath = fullPath.Substring(0, i) + fileName;
                    break;
                }
            }

            if (File.Exists(fullPath))
            {
                List<string> scores = File.ReadAllLines(fullPath).ToList();

                scores.Add(Player1.Score.ToString());

                StreamWriter scoreStream = new StreamWriter(fullPath);

                foreach(string score in scores)
                {
                    scoreStream.WriteLine(score);
                }
                
                scoreStream.Close();
            }
            else
            {
                saveError = true;
            }
        }

    }
}
