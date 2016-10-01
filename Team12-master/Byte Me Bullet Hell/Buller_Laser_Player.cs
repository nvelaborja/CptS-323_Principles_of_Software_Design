using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


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
    /**
     * A Laser-like bullet as defined in the specifications
     * 
     * */

    class Bullet_Laser_Player : Bullet_Abstract
    {

        Player player; // The refrence to the character who is shooting
        Vector2 refrencePoint;
        int duration = 220;
        int currentTick = 0;
        int offset;


        // Constructor, the targe is the position we are trying to aim for
        public Bullet_Laser_Player( Player player, int offset)           // Constructor takes texture (let's us create different types of bullets from player class)
        {
            texture = ContentMaster.bullet_lazer;
            isVisible = true;
            this.power = 20;
            this.player = player;
            this.refrencePoint = player.Position;
            position.X = refrencePoint.X;
            position.Y = refrencePoint.Y - texture.Height;
            hitBox = new Rectangle((int)refrencePoint.X, (int)refrencePoint.Y - texture.Height, texture.Width, texture.Height);
            this.offset = offset;   
        }

        //Update the position of the bullet
        override public void Update(GameTime gameTime)
        {
            refrencePoint = player.Position;
            position.X = refrencePoint.X + player.Texture.Width/2 + offset;
            position.Y = refrencePoint.Y -texture.Height;
            hitBox.Location = position.ToPoint();
            
            currentTick++;
            if(currentTick >= duration) {isVisible = false;}

        }

        public override void Draw(SpriteBatch spriteBatch)
        { 
         
            spriteBatch.Draw(texture, position, Color.White);                               // Draw the bullet at it's position

        }




    }
}

