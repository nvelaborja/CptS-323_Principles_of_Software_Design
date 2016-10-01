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
    // An abstract class for the bullets
    public abstract class Bullet_Abstract : Projectile
    {
        #region Class Members

        protected int speed;                                                                 // Distance in pixels that the bullet will shift each draw()
        protected int originX;
        protected int originY;                                                               // need an origin x and an origin y, to assure the bullet does not shoot the entitiy calling it 

        #endregion


        #region Properties

        public int Speed
        {
            get { return speed; }
        }

        
        #endregion
    }
}
