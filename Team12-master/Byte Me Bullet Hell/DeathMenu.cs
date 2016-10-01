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
    public class DeathMenu : Game
    {
        #region Class Members

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        MenuButton btnMenu;
        MenuButton btnExit;
        private Texture2D buttonExit;
        private Texture2D buttonMenu;
        private Texture2D backdrop;
        private Texture2D title;
        private bool currentClick = false;
        private bool displayMouse;
        private Settings settings;
        private Texture2D mouseTexture;
        private Vector2 mousePosition;

        #endregion

        public DeathMenu(ref Settings Settings)
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
            this.Window.IsBorderless = true;                                           // Hide top title bar

            settings = Settings;

            #endregion

            settings = Settings;

            displayMouse = true;
        }

        protected override void Initialize()                                            // Initialize member variables
        {
            ContentMaster.LoadAllContent(Content, ref settings);
            //ContentMaster.SM.IsSoundOn = settings.SoundOn;
            ContentMaster.SM.LoopDeathMenu();

            

            base.Initialize();
        }

        protected override void LoadContent()                                           // Load all member's content using their LoadContent functions
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            buttonMenu = Content.Load<Texture2D>("ButtonMenu");
            buttonExit = Content.Load<Texture2D>("ButtonExit");
            backdrop = Content.Load<Texture2D>("Backdrop");
            title = Content.Load<Texture2D>("GameOverTitle");
            mouseTexture = Content.Load<Texture2D>("PlayerTexture");

            //to see the mouse in main menu
            displayMouse = true;

            // Set up menu buttons
            btnMenu = new MenuButton(buttonMenu, graphics.GraphicsDevice);
            btnMenu.setPosition(new Vector2((1080 / 2) - buttonMenu.Width / 2, (720 / 4) * 1));
            btnExit = new MenuButton(buttonExit, graphics.GraphicsDevice);
            btnExit.setPosition(new Vector2((1080 / 2) - buttonExit.Width / 2, (720 / 4) * 2));
        }


        protected override void UnloadContent()                                         // Unload Content before closing game ( I think only needed when multiple scenes are used )
        {
            Content.Unload();
            base.Exit();
        }

        protected override void Update(GameTime gameTime)                               // Calls all member's update functions (happens once per frame, caps at 60fps)
        {
            MouseState mouse = Mouse.GetState();
            mousePosition = mouse.Position.ToVector2();
            mousePosition.X -= mouseTexture.Width / 2;

            btnMenu.Update(mouse);
            btnExit.Update(mouse);

            if (btnMenu.isClicked)
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

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)                                 // Actually draws elements onto the window so we can see them
        {
            GraphicsDevice.Clear(Color.Gray);                                           // Clears window every frame with Gray background

            spriteBatch.Begin();                                                        // Starts spritebatch, the object that contains all the drawing functions
            spriteBatch.Draw(backdrop, ContentMaster.zeroPosition, Color.White);

            btnMenu.Draw(spriteBatch);
            btnExit.Draw(spriteBatch);

            spriteBatch.Draw(title, ContentMaster.zeroPosition, Color.White);

            if (displayMouse)
            {
                spriteBatch.Draw(mouseTexture, mousePosition, Color.White);
            }

            spriteBatch.End();                                                          // Must end the spritebatch after each frame (not sure why)

            base.Draw(gameTime);                                                        // TBH, no idea what this does
        }
    }
}
