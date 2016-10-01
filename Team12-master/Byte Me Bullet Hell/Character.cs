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
using System.ComponentModel;

namespace Byte_Me_Bullet_Hell
{
    public abstract class Character : INotifyPropertyChanged
    {
        #region Abstract Members
   
        protected ContentManager Content;
        protected SoundManager SM;
        protected Texture2D texture;                                                          // Represents player's texture (image file)
        protected Vector2 position, previousPosition, origin, centerScreen, crest;            // Center Screen is center of the playable area, not center window. Origin is center of player, position is top left of player, crest is top middle of player
        protected Rectangle hitBox;                                                           // Invisible rectangle that will mirror player's position for collision use       
        protected int borderWidth = 5;                                                        // Width in pixels of the overlay border (applies only to the top, bottom, and left of the playable area
        protected int playableWidth = 710;                                                    // Width of the playable area box
        protected int playableHeight = 700;                                                   // Height of the playable area box
        protected int speed;                                                                  // Player speed (distance in pixels between each draw function during movement)
        protected int health = 100;
        protected int armor = 100;
        protected bool isVisible = true;
        protected int pointValue;
        protected double attackModifier=1;
        protected double armorModifier=1;
        protected int level;
        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        protected bool takephysicaldamage = false;
        protected int physicalDamage=2;


        #endregion

        #region Abstract Properties

        public Texture2D Texture
        {
            get { return texture; }
        }

        public Vector2 Position
        {
            get { return position; }
        }

        public Vector2 PreviousPosition
        {
            get { return previousPosition; }
        }

        public Vector2 Origin
        {
            get { return origin; }
        }

        public Vector2 Crest
        {
            get { return crest; }
        }

        public Rectangle HitBox
        {
            get { return hitBox; }
        }

        public int Speed
        {
            get { return speed; }
            set { speed = value; }
        }

        public int Health
        {
            get { return health; }
            set { health = value; }
        }

        public int Armor
        {
            get { return armor; }
            set { armor = value; }
        }

        public bool IsVisible
        {
            get
            {
                return isVisible;
            }

            set
            {
                isVisible = value;
                if (!isVisible)
                {
                    NotifyPropertyChanged("Dead");
                }
            }
        }

        public int PointValue
        {
            get { return pointValue; }
            set { pointValue = value; }
        }

        public double AttackModifier
        {
            get { return attackModifier; }
        }

        public double ArmorModifier
        {
            get { return armorModifier; }
            set { armorModifier = value; }
        }

        public double Level
        {
            get { return level; }
        }

        #endregion

        #region Abstract Functions

        public void TakeDamage (int damage)
        {
            damage = (int) (damage * armorModifier);

            if (damage > armor)
            {
                damage -= armor;
                health -= damage;
                armor = 0;
            }
            else
            {
                armor -= damage;
            }

            if (health < 1)
            {
                IsVisible = false;
            }
        }

        #endregion

        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public virtual void doPhysicalDamage()
        {
            takephysicaldamage = false;
           this.TakeDamage(physicalDamage);
            position.X += 1;
            position.Y += 1;
            SM.playImpact();
        }

        public bool takePhysicalDamage { get { return takephysicaldamage; } set { takephysicaldamage = value; } }

        public Vector2 ThePosition { get { return position; } set { position = value; } }
        
    }
}
