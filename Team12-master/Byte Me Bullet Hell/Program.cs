using System;

namespace Byte_Me_Bullet_Hell
{
#if WINDOWS || LINUX
    /// <summary>
    /// The main class.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            ContentMaster.LoadAllContent();
            Settings settings = new Settings(1, true);

            SplashScreen splashScreen = new SplashScreen(ref settings);
            splashScreen.Run();
        }

        //enum for the gamestate
        enum GameState
        {
            MainMenu,
            Options,
            Playing,
        }
    }
#endif
}
