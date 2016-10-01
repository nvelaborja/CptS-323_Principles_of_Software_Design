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
    class ContentMaster
    {
        #region Member Variables

        // Important values that many entities in the game may need to access should go here
        public static int playableHeight = 740;
        public static int playableWidth = 740;
        public static Random randy = new Random(); //Important this goes here
        public ContentMaster() { }

        // Textures should be loaded here, this way the picture is saved in game ememory and will not need to be re-processed 
        // every time the image is needed
        public static Texture2D enemy_peon_texture;
        public static Texture2D enemy_lurker_texture;
        public static Texture2D boss_texture;
        public static Texture2D player_texture;
        public static Texture2D bullet_small;
        public static Texture2D bullet_tiny;
        public static Texture2D bullet_lazer;
        public static Texture2D buttonPlay;
        public static Texture2D buttonExit;
        public static Texture2D buttonHighScores;
        public static Texture2D buttonSettings;
        public static Texture2D buttonResume;
        public static Texture2D backdrop;
        public static Texture2D smoke;
        public static Texture2D bomb;
        public static Texture2D tileBomb;
        public static Texture2D tileJump;
        public static Texture2D tileCooldown;
        public static Texture2D enemy_large_texture;
        public static SoundManager SM;
        public static Vector2 zeroPosition;

        #endregion

        public static void LoadAllContent(ContentManager content, ref Settings Settings)// A function called to load all pictures, this the game does not have to re-rende rpictures every time a new enemy enters the field. 
        {
            enemy_peon_texture = content.Load<Texture2D>("PeonTexture");
            enemy_lurker_texture = content.Load<Texture2D>("LurkerSpriteSheet");
            enemy_large_texture = content.Load<Texture2D>("third");
            boss_texture = content.Load<Texture2D>("Boss1-1SpriteSheet");
            player_texture = content.Load<Texture2D>("PlayerTexture");
            bullet_small = content.Load<Texture2D>("Bullet_SmallSpriteSheet");
            bullet_tiny = content.Load<Texture2D>("tiny");
            bullet_lazer = content.Load<Texture2D>("Bullet_Lazer");
            buttonPlay = content.Load<Texture2D>("ButtonPlay");
            buttonExit = content.Load<Texture2D>("ButtonExit");
            buttonHighScores = content.Load<Texture2D>("ButtonHighScores");
            buttonSettings = content.Load<Texture2D>("ButtonSettings");
            buttonResume = content.Load<Texture2D>("ButtonResume");
            smoke = content.Load<Texture2D>("Smoke");
            backdrop = content.Load<Texture2D>("Backdrop");
            bomb = content.Load<Texture2D>("Bomb");
            tileBomb = content.Load<Texture2D>("TileBomb");
            tileJump = content.Load<Texture2D>("TileJump");
            tileCooldown = content.Load<Texture2D>("TileCoolDown");

            zeroPosition = new Vector2(0, 0);

            SM = new SoundManager(ref Settings);
            SM.LoadContent(content);
        }

        public static void LoadAllContent() 
        {
        }
    }
}
