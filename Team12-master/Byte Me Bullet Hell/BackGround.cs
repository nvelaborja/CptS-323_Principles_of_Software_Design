using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Byte_Me_Bullet_Hell
{
    public class BackGround
    {
        private Texture2D overlayTexture;                                                   // Texture that will be the game's overlay
        private Texture2D tileBomb;
        private Texture2D tileJump;
        private Texture2D tileCooldown;
        private Texture2D tileShield;
        private Vector2 zeroPosition;                                                       // Literally just position (0, 0), higher time efficiency if we just keep this as a member
        private SpriteFont healthFont;
        private GraphicsDevice graphicsDevice;
        private Player player1;
        private Boss boss1;
        private const int BarPosX = 810;
        private const int pHealthPosY = 35;
        private const int pArmorPosY = 80;
        private const int bHealthPosY = 600;
        private const int bArmorPosY = 645;
        private const int textPosX = 750;
        private const int scoreDigitGap = 20;
        private Settings settings;

        public BackGround(GraphicsDevice graphicsDevice, ref Player player1, ref Settings Settings)                // Constructor, initialize member variables
        {
            this.player1 = player1;
            zeroPosition = new Vector2(0, 0);                                               // Set zero position to position (0, 0)
            this.graphicsDevice = graphicsDevice;
            settings = Settings;
        }

        public void LoadContent(ContentManager Content)                                     
        {
            overlayTexture = Content.Load<Texture2D>("Overlay");                            // Load Overlay png file for overlay Texture
            tileBomb = Content.Load<Texture2D>("TileBomb");
            tileJump = Content.Load<Texture2D>("TileJump");
            tileCooldown = Content.Load<Texture2D>("TileCooldown");
            tileShield = Content.Load<Texture2D>("TileShield");
            healthFont = Content.Load<SpriteFont>("HealthFont");
            
        }

        public void Update(GameTime gameTime)
        {

        }

        public void Draw(SpriteBatch spriteBatch)                                           
        {
            spriteBatch.Draw(overlayTexture, zeroPosition, Color.White);

            if (player1 != null)
            {
                DrawPlayerBars(spriteBatch);
            }

            if (boss1 != null)
            {
                DrawBossBars(spriteBatch);
            }

            DrawScore(spriteBatch);

            DrawInfo(spriteBatch);

            DrawTiles(spriteBatch);

            //DrawDebug(spriteBatch);
        }

        private void DrawDebug(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(healthFont, "Sound : " + settings.SoundOn.ToString(), new Vector2(500, 500), Color.Black);
            spriteBatch.DrawString(healthFont, "Difficulty : " + settings.Difficulty.ToString(), new Vector2(500, 520), Color.Black);
        }

        private void DrawTiles(SpriteBatch spriteBatch)
        {
            if (player1.JumpDelay != 270)
            {
                Rectangle jumpSource = new Rectangle(0, 0, tileJump.Width, 90 - player1.JumpDelay / 3);

                Vector2 jumpCenter = new Vector2(tileJump.Width / 2.0f, tileJump.Height);

                spriteBatch.Draw(tileJump, new Vector2(792, 330 + tileJump.Height), jumpSource, Color.White, 0.0f, jumpCenter, 1.0f, SpriteEffects.None, 0.0f);
            }
            spriteBatch.Draw(tileBomb, new Vector2(860, 330), Color.White);
            spriteBatch.Draw(tileShield, new Vector2(993, 350), Color.White);
        }

        private void DrawInfo(SpriteBatch spriteBatch)
        {
            // Display Level

            int levelDigit1 = 0, levelDigit2 = 0;

            levelDigit2 = (int) player1.Level % 10;
            levelDigit1 = (int) player1.Level % 100 / 10;

            spriteBatch.DrawString(healthFont, "Level : ", new Vector2(textPosX, 210), Color.LightSlateGray);
            spriteBatch.DrawString(healthFont, levelDigit1.ToString(), new Vector2(textPosX + 50, 210), Color.White);
            spriteBatch.DrawString(healthFont, levelDigit2.ToString(), new Vector2(textPosX + 60, 210), Color.White);

            // Display Damage and Armor Mods

            spriteBatch.DrawString(healthFont, "Damage : ", new Vector2(textPosX, 250), Color.LightSlateGray);
            spriteBatch.DrawString(healthFont, string.Format("{0:0.##}", player1.AttackModifier * 100) + "%", new Vector2(textPosX + 72, 250), Color.White);

            spriteBatch.DrawString(healthFont, "Armor : ", new Vector2(textPosX, 290), Color.LightSlateGray);
            spriteBatch.DrawString(healthFont, string.Format("{0:0.##}", player1.ArmorModifier * 100) + "%", new Vector2(textPosX + 53, 290), Color.White);

            // Display Statistics

            double accuracy = 0.0;

            if (player1.ShotsHit > 0)
            {
                accuracy = ((double) player1.ShotsHit / (double) player1.ShotsFired * 100);
            }

            spriteBatch.DrawString(healthFont, "Shots Taken : ", new Vector2(textPosX + 160, 210), Color.LightSlateGray);
            spriteBatch.DrawString(healthFont, player1.ShotsFired.ToString(), new Vector2(textPosX + 260, 210), Color.White);

            if (!double.IsInfinity(accuracy))
            {
                spriteBatch.DrawString(healthFont, "Accuracy : ", new Vector2(textPosX + 160, 250), Color.LightSlateGray);
                spriteBatch.DrawString(healthFont, string.Format("{0:0.#}", accuracy) + "%", new Vector2(textPosX + 240, 250), Color.White);

                spriteBatch.DrawString(healthFont, "Enemies Killed : ", new Vector2(textPosX + 160, 290), Color.LightSlateGray);
                spriteBatch.DrawString(healthFont, player1.EnemiesKilled.ToString(), new Vector2(textPosX + 275, 290), Color.White);
            }
        }

        private void DrawScore(SpriteBatch spriteBatch)
        {
            int digit1 = 0, digit2 = 0, digit3 = 0, digit4 = 0, digit5 = 0, digit6 = 0, digit7 = 0;

            digit7 = player1.Score % 10;
            digit6 = player1.Score % 100 / 10;
            digit5 = player1.Score % 1000 / 100;
            digit4 = player1.Score % 10000 / 1000;
            digit3 = player1.Score % 100000 / 10000;
            digit2 = player1.Score % 1000000 / 100000;
            digit1 = player1.Score % 10000000 / 1000000;

            spriteBatch.DrawString(healthFont, "S c o r e  : ", new Vector2(textPosX + 50, 175), Color.LightSlateGray);
            spriteBatch.DrawString(healthFont, digit1.ToString(), new Vector2(textPosX + 110 + (scoreDigitGap * 1), 175), Color.White);
            spriteBatch.DrawString(healthFont, digit2.ToString(), new Vector2(textPosX + 110 + (scoreDigitGap * 2), 175), Color.White);
            spriteBatch.DrawString(healthFont, digit3.ToString(), new Vector2(textPosX + 110 + (scoreDigitGap * 3), 175), Color.White);
            spriteBatch.DrawString(healthFont, digit4.ToString(), new Vector2(textPosX + 110 + (scoreDigitGap * 4), 175), Color.White);
            spriteBatch.DrawString(healthFont, digit5.ToString(), new Vector2(textPosX + 110 + (scoreDigitGap * 5), 175), Color.White);
            spriteBatch.DrawString(healthFont, digit6.ToString(), new Vector2(textPosX + 110 + (scoreDigitGap * 6), 175), Color.White);
            spriteBatch.DrawString(healthFont, digit7.ToString(), new Vector2(textPosX + 110 + (scoreDigitGap * 7), 175), Color.White);
        }

        private void DrawPlayerBars(SpriteBatch spriteBatch)
        {
            Texture2D healthBar_Back = new Texture2D(graphicsDevice, 250, 30);
            Texture2D armorBar_Back = new Texture2D(graphicsDevice, 250, 30);

            spriteBatch.DrawString(healthFont, "Health: ", new Vector2(textPosX, pHealthPosY + 2), Color.LightSlateGray);
            spriteBatch.DrawString(healthFont, "Armor: ", new Vector2(textPosX, pArmorPosY + 2), Color.LightSlateGray);

            Color[] healthBackPixels = new Color[250 * 30];
            for (int i = 0; i < healthBackPixels.Length; ++i) healthBackPixels[i] = Color.Black;
            healthBar_Back.SetData(healthBackPixels);

            Color[] armorBackPixels = new Color[250 * 30];
            for (int i = 0; i < armorBackPixels.Length; ++i) armorBackPixels[i] = Color.Black;
            armorBar_Back.SetData(armorBackPixels);

            if (player1.Health > 0)
            {
                Texture2D playerHealthBar = new Texture2D(graphicsDevice, (int)(player1.Health * 2.4), 20);

                Color[] healthPixels = new Color[(int)(player1.Health * 2.4 * 20)];
                for (int i = 0; i < healthPixels.Length; ++i) healthPixels[i] = Color.DarkRed;
                playerHealthBar.SetData(healthPixels);
                spriteBatch.Draw(healthBar_Back, new Vector2(BarPosX - 5, pHealthPosY - 5), Color.White);
                spriteBatch.Draw(playerHealthBar, new Vector2(BarPosX, pHealthPosY), Color.White);
            }
            else { spriteBatch.Draw(healthBar_Back, new Vector2(BarPosX - 5, pHealthPosY - 5), Color.White); }

            if (player1.Armor > 0)
            { 
                Texture2D playerArmorBar = new Texture2D(graphicsDevice, (int)(player1.Armor * 2.4), 20);

                Color[] armorPixels = new Color[(int)(player1.Health * 2.4 * 20)];
                for (int i = 0; i < armorPixels.Length; ++i) armorPixels[i] = Color.DarkGreen;
                playerArmorBar.SetData(armorPixels);
                spriteBatch.Draw(armorBar_Back, new Vector2(BarPosX - 5, pArmorPosY - 5), Color.White);
                spriteBatch.Draw(playerArmorBar, new Vector2(BarPosX, pArmorPosY), Color.White);
            }
            else { spriteBatch.Draw(armorBar_Back, new Vector2(BarPosX - 5, pArmorPosY - 5), Color.White); }
        }

        private void DrawBossBars(SpriteBatch spriteBatch)
        {
            Texture2D healthBar_Back = new Texture2D(graphicsDevice, 250, 30);
            Texture2D armorBar_Back = new Texture2D(graphicsDevice, 250, 30);

            spriteBatch.DrawString(healthFont, "Health: ", new Vector2(textPosX, bHealthPosY + 2), Color.LightSlateGray);
            spriteBatch.DrawString(healthFont, "Armor: ", new Vector2(textPosX, bArmorPosY + 2), Color.LightSlateGray);

            Color[] healthBackPixels = new Color[250 * 30];
            for (int i = 0; i < healthBackPixels.Length; ++i) healthBackPixels[i] = Color.Black;
            healthBar_Back.SetData(healthBackPixels);

            Color[] armorBackPixels = new Color[250 * 30];
            for (int i = 0; i < armorBackPixels.Length; ++i) armorBackPixels[i] = Color.Black;
            armorBar_Back.SetData(armorBackPixels);

            if (boss1.Health > 1)
            {
                Texture2D bossHealthBar = new Texture2D(graphicsDevice, (int)(boss1.Health * 2.4), 20);

                Color[] healthPixels = new Color[(int)(boss1.Health * 2.4 * 20)];
                for (int i = 0; i < healthPixels.Length; ++i) healthPixels[i] = Color.DarkBlue;
                bossHealthBar.SetData(healthPixels);
                spriteBatch.Draw(healthBar_Back, new Vector2(BarPosX - 5, bHealthPosY - 5), Color.White);
                spriteBatch.Draw(bossHealthBar, new Vector2(BarPosX, bHealthPosY), Color.White);
            }
            else { spriteBatch.Draw(healthBar_Back, new Vector2(BarPosX - 5, bHealthPosY - 5), Color.White); }

            if (boss1.Armor > 1)
            {
                Texture2D bossArmorBar = new Texture2D(graphicsDevice, (int)(boss1.Armor * 2.4), 20);

                Color[] armorPixels = new Color[(int)(boss1.Health * 2.4 * 20)];
                for (int i = 0; i < armorPixels.Length; ++i) armorPixels[i] = Color.Olive;
                bossArmorBar.SetData(armorPixels);
                spriteBatch.Draw(armorBar_Back, new Vector2(BarPosX - 5, bArmorPosY - 5), Color.White);
                spriteBatch.Draw(bossArmorBar, new Vector2(BarPosX, bArmorPosY), Color.White);
            }
            else { spriteBatch.Draw(armorBar_Back, new Vector2(BarPosX - 5, bArmorPosY - 5), Color.White); }
        }
        
        public Boss Boss
        {
            set { boss1 = value; }
        }
    }
}