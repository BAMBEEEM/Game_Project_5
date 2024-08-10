using Game_Project_5.Collisions;
using Game_Project_5.ParticleManagement;
using Game_Project_5.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game_Project_5.Sprites
{

    public class Enemy
    {
        public Vector2 Position { get; set; } = new Vector2(315, 552);

        public Texture2D _attackTexture;
        /*        public Texture2D _runningTexture;
        */
        public CharacterSprite Player { get; set; }
        public float Distance { get; set; } = 250;

        public Vector2 Guard { get; set; } = new(350, 350);

        public float Speed { get; set; } = RandomHelper.NextFloat(225, 295);

        public bool isFighting { get; set; } = false;

        private bool flipped;

        private int _animationFrame;

        private double _flippingTimer;

        private double _animationTimer;

        private float _flippingSpeed = 0.35f;

        public bool Stopped = true;

        private float _animationSpeed = 0.1f;

        private bool _standing = false;

        public bool Dead = false;

        public Color Color = Color.White;

        public bool _running = true;

        public bool _charging = false;

        public float _attackTimerLength;

        public bool _runAnimation = false;

        private float _chargeTimerLength = 800;

        public bool _attacking = false;

        Vector2 direction;

        public bool Damaging = false;

        private KeyboardState keyboardState;

        public float Health = 250;

        public float AttackDamage = 7;

        public bool Shooting = false;
        public bool Show = true;

        KeyboardState _previousKeyboardState;

        public float RespawnTime;

        private BoundingRectangle _attackBounds = new BoundingRectangle(new Vector2(600 - 54, 200 - 56) * 1.6f, 160, 65);

        public bool Initializing = true;

        /// <summary>
        /// The bounding volume of the sprite's attack
        /// </summary>
        public BoundingRectangle AttackBounds => _attackBounds;

        private BoundingRectangle _characterBounds = new BoundingRectangle(new Vector2(600 - 54, 200 - 56) * 1.6f, 80, 75);

        /// <summary>
        /// The bounding volume of the sprite
        /// </summary>
        public BoundingRectangle CharacterBounds => _characterBounds;


/*        public Enemy(int position, float respawnTime)
        {
            RespawnTime = respawnTime;
            if (position == 1)
            {
                Position = new(-140, RandomHelper.NextFloat(505, 600));
            }
            else
            {
                Position = new(1270, RandomHelper.NextFloat(505, 600));
            }
        }*/

        public void LoadContent(ContentManager content)
        {
            _attackTexture = content.Load<Texture2D>("2nd");
            /*            _runningTexture = content.Load<Texture2D>("run");
            */
        }

        private float _deadTime;
        public void Update(GameTime gameTime)
        {

/*
            if (Initializing & !Stopped)
            {
                if (Position.X < 170)
                    Position += new Vector2(0.79f, 0) * Speed * (float)gameTime.ElapsedGameTime.TotalSeconds;

                else if (Position.X > 1070)
                    Position += new Vector2(-0.79f, 0) * Speed * (float)gameTime.ElapsedGameTime.TotalSeconds;

                if (Position.Y > 510) Position += new Vector2(0, -0.79f) * Speed * (float)gameTime.ElapsedGameTime.TotalSeconds;

                if ((Position.X >= 170 && Position.X <= 1070) || direction.Length() < 100)
                    Initializing = false;
            }*/

            if (Dead)
            {
                _deadTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
            }

            /*            #region Debugging Buttons
                        _previousKeyboardState = keyboardState;
                        keyboardState = Keyboard.GetState();
                        if (keyboardState.IsKeyDown(Keys.R))
                            Dead = true;
                        if (keyboardState.IsKeyDown(Keys.Q))
                        {
                            Running();
                        }
                        if (keyboardState.IsKeyDown(Keys.W))
                        {
                            Charging();
                        }
                        if (keyboardState.IsKeyDown(Keys.E))
                        {
                            Attacking();
                        }
                        if (keyboardState.IsKeyDown(Keys.F))
                        {
                            flipped = (flipped) ? flipped = false : flipped = true;
                        }

                        #endregion
            */

            if (Health <= 0)
            {
                Dead = true;
                Damaging = false;
            }

/*            if (Player is null) return;
*/
            /*            if (!(_charging || _attacking)) // if not running, get direction.
                            direction = Player.CurrentPosition - Position;
                        else //else, reduce timer for dash attack
                        {
                            _attackTimer -= (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                        }*/

/*            if (_running) // if not running, get direction.
                direction = Player.CurrentPosition - Position;
            else if (_charging) //else, reduce timer for dash attack
            {
                *//*                direction = Player.CurrentPosition - Position;
                *//*
                _chargeTimerLength -= (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            }
            else if (_attacking)
                _attackTimerLength -= (float)gameTime.ElapsedGameTime.TotalMilliseconds;

*/


            /*            if (!_attacking && !_charging) // if not attacking, get direction.
                        {*/
            float maximumDistance = 0;

            #region Flipping And Bounds Logic
/*            if (flipped)
            {
                maximumDistance = RandomHelper.NextFloat(25, 105);
                _attackBounds.X = Position.X - 111f; //1.6f
                _attackBounds.Y = Position.Y + 31f;    //1.6f
                _characterBounds.X = Position.X + 40f; //1.6f
                _characterBounds.Y = Position.Y + 5f;    //1.6f

            }
            else
            {
                maximumDistance = RandomHelper.NextFloat(40, 190);
                _attackBounds.X = Position.X + 118.2f; //1.6f
                _attackBounds.Y = Position.Y + 31f;    //1.6f
                _characterBounds.X = Position.X + 40f; //1.6f
                _characterBounds.Y = Position.Y + 5f;    //1.6f
            }


            if (direction.X > 0)
            {
                if (!_attacking && _animationFrame > 7 && !Dead)
                    flipped = false;
            }

            else
            {
                if (!_attacking && _animationFrame > 7 && !Dead)
                    flipped = true;
            }
*/
            #endregion


            // else {do the meleeSmash()}
            /*            }*/



            /*            if (_charging == true && _chargeTimer > 18) // if we initiated attack and timer is not ready, we stand still.
                        {
            *//*                _charging = true;
                            _attacking = false;
                            _running = false;*//*
                        }
                        else*/
            _previousKeyboardState = keyboardState;
            keyboardState = Keyboard.GetState();

            if ((keyboardState.IsKeyDown(Keys.G)))
            {
                Shooting = true;
            }


                //Update the bounds

                if (keyboardState.IsKeyDown(Keys.B))
            {
                Shooting = false;
            }
            if (!Stopped)


                if (_animationFrame >= 0 && _animationFrame <= 6)
                Damaging = true;
            else
                Damaging = false;
        }







        /// <summary>
        /// Draws the sprite using the supplied SpriteBatch
        /// </summary>
        /// <param name="gameTime">The game time</param>
        /// <param name="spriteBatch">The spritebatch to render with</param>
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {

            //if (Slowed) Color = Color.SandyBrown;



            _animationTimer += gameTime.ElapsedGameTime.TotalSeconds;
            if (!Dead)
            {
                if (Shooting)
                {
                    if (_animationTimer > _animationSpeed)
                    {
                        if (_animationFrame == 3)
                            ;

                        else if (_animationFrame < 0 || _animationFrame >= 4)
                            _animationFrame = 0;

                        else
                        {
                            _animationFrame++;
                        }






                        _animationTimer -= _animationSpeed;
                    }
                }
                else
                {
                    _animationFrame = 0;
                }

            }
            else if (Dead) // stop at animation frame #3 when dead (looks the best) or stay standing if wasn't moving
            {
                //Color = Color.SkyBlue;

                if (_animationTimer > _animationSpeed)
                {
                    if (_animationFrame == 15)
                        ; //do nothing

                    else if (_animationFrame < 6 || _animationFrame >= 16)
                        _animationFrame = 7;

                    else
                    {
                        _animationFrame++;
                    }




                    _animationTimer -= _animationSpeed;
                }
            }
            Rectangle source = new();
                source = new Rectangle(0, _animationFrame * 52, 350, 52);

            Vector2 origin = (flipped) ? new Vector2(-72, 0) : new Vector2(0, 0);
            if (_animationFrame >= 20 && _animationFrame <= 27)
                origin.Y += 30;

            if (_animationFrame >= 0 && _animationFrame <= 19 && flipped)
            {
                origin.X += 290;
            }



            SpriteEffects spriteEffects = (flipped) ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            if (Show)
            spriteBatch.Draw(_attackTexture, Position, source, Color, 0, origin, 2.4f, SpriteEffects.FlipHorizontally, 0.2f);

        }



    }
}
