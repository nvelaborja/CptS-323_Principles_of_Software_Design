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
    public abstract class Projectile
    {
        #region Abstract Members

        protected ContentManager Content;
        protected Rectangle hitBox;                                                     // Invisible rectangle that will mirror player's position for collision use
        protected Texture2D texture;                                                          // Texture (image file) for the bullet
        protected Vector2 origin, position;                                                   // Coordinates for bullets position(top left) and origin(center)
        protected bool isVisible;                                                             // Boolean for whether or not the bullet is still visible, will be set to false after collision or outofbounds
        protected int power = 5;

        #endregion

        #region Abstract Properties

        public Rectangle HitBox
        {
            get { return hitBox; }
        }

        public Texture2D Texture
        {
            get { return texture; }
        }

        public Vector2 Origin
        {
            get { return origin; }
        }
        public Vector2 Position
        {
            get { return position; }
        }

        public bool IsVisible
        {
            get { return isVisible; }
            set { isVisible = value; }
        }

        public int Power
        {
            get { return power; }
            set { power = value; }
        }

        #endregion
        
        #region Abstract Functions

        public abstract void Update(GameTime gameTime);


        public abstract void Draw(SpriteBatch spriteBatch);

        #endregion
    }
}
