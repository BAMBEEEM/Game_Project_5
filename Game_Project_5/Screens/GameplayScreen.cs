using System;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Game_Project_5.StateManagement;
using static System.TimeZoneInfo;
using Game_Project_5.Background;
using Game_Project_5.Sprites;
using System.Collections.Generic;
using System.Reflection.Metadata;
using Game_Project_5.Collisions;
using SharpDX.Direct2D1;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TaskbarClock;
using System.DirectoryServices.ActiveDirectory;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TrayNotify;
using Game_Project_5.ParticleManagement;
using Game_Project_5.Misc;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using Game_Project_5.Enums;

namespace Game_Project_5.Screens
{
    // This screen implements the actual game logic. It is just a
    // placeholder to get the idea across: you'll probably want to
    // put some more interesting gameplay in here!
    public class GameplayScreen : GameScreen
    {
        private ContentManager _content;
        private SpriteFont _gameFont;

        private Vector2 _playerPosition = new Vector2(100, 100);
        private Vector2 _enemyPosition = new Vector2(100, 100);

        private readonly Random _random = new Random();

        private float _pauseAlpha;
        private readonly InputAction _pauseAction;

        private GraphicsDeviceManager graphics;

        private bool _hasBegun = false;
        private bool _lost = false;

        private CharacterSprite _mainCharacter;
        private Enemy _enemy;
        private SecondEnemy _secondEnemy;
        private CharacterSprite _lastEnemy;
        private SpriteFont spriteFont;

        private bool _hittingTime = false;

        private bool _won = false;

        private TimeSpan _elapsedTime = new TimeSpan();

        private Color _timeColor;

        private float _shakeTime;

        private float _drownTime;

        private bool _isDrowning;
        private float _timeoffset;

        private float _startTimer = 0;

        TimeSpan introProgress;
        Song _ingameSong;

        private GraphicsDeviceManager _graphics;


        private int _level = 1;

        private Texture2D _background1_MainLayer;
        private Texture2D _background1_Ground;

        private Texture2D _background1_Layer1;
        private Texture2D _background1_Layer2;
        private Texture2D _background1_Layer3;
        private Texture2D _background1_Layer4;

        private Texture2D _background2_MainLayer;
        private Texture2D _background2_Layer1;
        private Texture2D _background2_Layer2;
        private Texture2D _background2_Layer3;

        private Texture2D _background2_Ground;

        private Texture2D _background3_MainLayer;
        private Texture2D _background3_RedSun;



        private SoundEffect _mainWind;

        private SoundEffect[] _highWind = new SoundEffect[3];

        private SoundEffect _whistle;

        private SoundEffect _laserSound;

        private SoundEffect _enemyLaserSound;




        private double _pressTimer;

        public GameplayScreen()
        {
            TransitionOnTime = TimeSpan.FromSeconds(1.5);
            TransitionOffTime = TimeSpan.FromSeconds(1.5);
            _hasBegun = true;
            /*
                        _pauseAction = new InputAction(
                            new[] { Buttons.Start, Buttons.Back },
                            new[] { Keys.Back }, true);*/
        }




        // Load graphics content for the game
        public override void Activate()
        {
            if (_content == null)
                _content = new ContentManager(ScreenManager.Game.Services, "Content");

            _background1_MainLayer = _content.Load<Texture2D>("cloud8_mainlayer");
            _background1_Layer1 = _content.Load<Texture2D>("cloud8_layer1");
            _background1_Layer2 = _content.Load<Texture2D>("cloud8_layer2");
            _background1_Layer3 = _content.Load<Texture2D>("cloud8_layer3");
            _background1_Layer4 = _content.Load<Texture2D>("cloud8_layer4");
            _background1_Ground = _content.Load<Texture2D>("cloud8_ground");

            _background2_MainLayer = _content.Load<Texture2D>("cloud6_mainlayer");
            _background2_Layer1 = _content.Load<Texture2D>("cloud6_layer1");
            _background2_Layer2 = _content.Load<Texture2D>("cloud6_layer2");
            _background2_Layer3 = _content.Load<Texture2D>("cloud6_layer3");

            _background2_Ground = _content.Load<Texture2D>("cloud6_ground");

            _background3_MainLayer = _content.Load<Texture2D>("lastbackground");
            _background3_RedSun = _content.Load<Texture2D>("extra_assets");


            _mainCharacter = new CharacterSprite();

            _mainCharacter.LoadContent(_content);

            _enemy = new Enemy();

            _enemy.LoadContent(_content);

            _secondEnemy = new SecondEnemy();

            _secondEnemy.LoadContent(_content);

            _lastEnemy = new CharacterSprite();

            _lastEnemy.LoadContent(_content);


            _highWind[0] = _content.Load<SoundEffect>("sound2");
            _highWind[1] = _content.Load<SoundEffect>("sound3");
            _highWind[2] = _content.Load<SoundEffect>("sound4");


            _mainWind = _content.Load<SoundEffect>("sound1");
            _whistle = _content.Load<SoundEffect>("whistle");
            _laserSound = _content.Load<SoundEffect>("Laser");
            _enemyLaserSound = _content.Load<SoundEffect>("EnemyLaser");


            /*
                        for (int i = 0; i < 18; i++)
                        {
                            _enemy[i] = new Enemy(RandomHelper.Next(1, 3), i * 2.45f);
                            _enemy[i].LoadContent(_content);
                            _enemy[i].Player = _mainCharacter;
                        }*/

            _mainCharacter.MaxOffsetX = (ScreenManager.GraphicsDevice.Viewport.Width);

            _mainCharacter.MaxOffsetY = (ScreenManager.GraphicsDevice.Viewport.Height);



            spriteFont = _content.Load<SpriteFont>("retro");





            /*            // _gameFont = _content.Load<SpriteFont>("gamefont");

                        // A real game would probably have more content than this sample, so
                        // it would take longer to load. We simulate that by delaying for a
                        // while, giving you a chance to admire the beautiful loading screen.
                        Thread.Sleep(1000);

                        // once the load has finished, we use ResetElapsedTime to tell the game's
                        // timing mechanism that we have just finished a very long frame, and that
                        // it should not try to catch up.*/
            ScreenManager.Game.ResetElapsedTime();



            //MediaPlayer.Play(_ingameSong);
            MediaPlayer.IsRepeating = true;
        }

        /*        public override void Deactivate()
                {
                    base.Deactivate();
                }*/

        /*        public override void Unload()
                {
                    _content.Unload();
                }*/
        private float _mainTimer;
        private int _killed = 0;
        private bool _saved = false;
        private float randomTimer = RandomHelper.NextFloat(2.8f, 11.5f);
        private bool _enemyWin = false;

        private bool _pressToContinue;
        private float _transitionTimer = 0;

        private bool _wonRound;
        private KeyboardState keyboardState;

        private void transition(GameTime gameTime)
        {

        }
        private double _soundTimer;
        private double _mainSoundTimer = 9500;
        private double _whitsleTimer = 5000;
        private double _laserTimer;
        private double _enemyLaserTimer = 40000000;

        private int _transitioningToNextLevel = 0;
        float[] ShootTimer = { RandomHelper.NextFloat(0.65f, 0.85f), RandomHelper.NextFloat(0.48f, 0.57f), RandomHelper.NextFloat(0.30f, 0.38f) };
        private float _currentTimeLimit = 0;

        private float _timeUntilSpaceCounted;
        // This method checks the GameScreen.IsActive property, so the game will
        // stop updating when the pause menu is active, or if you tab away to a different application.
        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            if (_level == 1)
            {
                _enemy.Show = false;
                _secondEnemy.Show = true;
                _lastEnemy.Show = false;
                _currentTimeLimit = ShootTimer[0];
            }
            else if (_level == 2)
            {
                _enemy.Show = true;
                _secondEnemy.Show = false;
                _lastEnemy.Show = false;
                _currentTimeLimit = ShootTimer[1];

            }
            else
            {
                _enemy.Show = false;
                _secondEnemy.Show = false;
                _lastEnemy.Show = true;
                _currentTimeLimit = ShootTimer[2];

            }
            _soundTimer += gameTime.ElapsedGameTime.TotalMilliseconds;

            _whitsleTimer += gameTime.ElapsedGameTime.TotalMilliseconds;
            _mainSoundTimer += gameTime.ElapsedGameTime.TotalMilliseconds;
            _laserTimer += gameTime.ElapsedGameTime.TotalMilliseconds;
            _enemyLaserTimer += gameTime.ElapsedGameTime.TotalMilliseconds;

            if (_soundTimer > RandomHelper.Next(3500, 6500))
            {
                if (!_hittingTime)
                {
                    _highWind[RandomHelper.Next(0, 3)].Play();
                }
                _soundTimer = 0;
            }

            if (_mainSoundTimer > RandomHelper.Next(8000, 9500))
            {
                _mainWind.Play();
                _mainSoundTimer = 0;
            }

            _startTimer -= (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            _mainTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (_startTimer > 18)
            {
                _mainCharacter.Stopped = true;
            }
            else if (_startTimer > -18) _mainCharacter.Stopped = false;

            if (randomTimer < _mainTimer)
            {
                _hittingTime = true;
                /*                _whistle.Play();
                */
            }

            if (_whitsleTimer > 5000)
            {
                if (_hittingTime && !_won && !_lost && !_wonRound)
                {
                    _whistle.Play();
                    _whitsleTimer = 0;
                }
            }

            if (_laserTimer > 6000)
            {
                if (_mainCharacter.Shooting && _hittingTime && ! _pressToContinue &&!_won)
                {
                    _laserSound.Play();
                    _laserTimer = 0;

                }
            }

            if (_enemyLaserTimer > 40000000)
            {
                if (_hittingTime && !_mainCharacter.ShotEarly && _lost)
                {
                    _enemyLaserSound.Play();
                    _enemyLaserTimer = 0;

                }
            }


            KeyboardState _lastkeyboardState = keyboardState;

            keyboardState = Keyboard.GetState();

            if (keyboardState.IsKeyDown(Keys.Tab))
            {

            }

            if (_hittingTime == true && _mainCharacter.Shooting != true && ((randomTimer + _currentTimeLimit < _mainTimer)))
            {
                _lost = true;
                _enemy.Shooting = true;
                _secondEnemy.Shooting = true;
                _lastEnemy.Shooting = true;
            }



            _transitionTimer -= (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            if (_transitionTimer > 0)
            {
                coveredByOtherScreen = true;
                if (_transitionTimer < 17 && _transitioningToNextLevel == 1)
                {
                    _mainCharacter.Shooting = false;
                    _level++;
                    randomTimer = RandomHelper.NextFloat(4.5f, 11.5f);
                    _transitioningToNextLevel = 0;
                    _timeoffset = 0;
                    _pressToContinue = false;
                    _enemy.Shooting = false;
                    _secondEnemy.Shooting = false;
                    _lastEnemy.Shooting = false;
                }
            }
            if (_mainCharacter.Shooting && _wonRound)
            {
                if (_level < 3)
                {
                    _pressToContinue = true;
                    _mainSoundTimer = 0;

                    _pressTimer += gameTime.ElapsedGameTime.TotalMilliseconds;
                    if (keyboardState.GetPressedKeys().Length > 0 && _pressTimer > 1000)
                    {
                        _pressTimer = 0;
                        _wonRound = false;
                        if (_transitioningToNextLevel <= 0)
                            _transitioningToNextLevel = 1;
                        _transitionTimer = 1500;
                        ScreenManager.Game.ResetElapsedTime();
                        _mainTimer = 0;
                        _timeUntilSpaceCounted = 0;
                        _hittingTime = false;

                    }
                }
                else
                {
                    _won = true;
                }
            }
            _timeUntilSpaceCounted += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            if (!(_lost) && (_hittingTime == false) && (keyboardState.IsKeyDown(Keys.Space) & _lastkeyboardState.IsKeyUp(Keys.Space)) && (_timeUntilSpaceCounted > 2000))
            {
                _mainCharacter.ShotEarly = true;
                _lost = true;
            }
            else if ((_hittingTime == true && (keyboardState.IsKeyDown(Keys.Space) & _lastkeyboardState.IsKeyUp(Keys.Space)) && !_lost && (_timeUntilSpaceCounted > 2000))|| (keyboardState.IsKeyDown(Keys.Tab) & _lastkeyboardState.IsKeyUp(Keys.Tab)))
            {
                _mainCharacter.Shooting = true;
                _wonRound = true;
                _hittingTime = false;
            }




            _mainCharacter.Color = Color.White; // default color
            /*            foreach (Enemy e in _enemy)
                        {
                            if (e.RespawnTime < _mainTimer)
                                e.Stopped = true;
                            else e.Stopped = true;

                            if (e.Dead)
                                _killed++;
                            e.Update(gameTime);
                        }*/
            _mainCharacter.Update(gameTime);
            _mainCharacter.CurrentPosition = new Vector2(193, 556);
            _enemy.Position = new Vector2(260, 570);
            _enemy.Update(gameTime);
            _secondEnemy.Position = new Vector2(-191, 576);
            _secondEnemy.Update(gameTime);
            _lastEnemy.CurrentPosition = new Vector2(835, 556);
            _lastEnemy.Flipped = true;
            _lastEnemy.Update(gameTime);

            if (_level < 3)
            {
                _mainCharacter.CurrentPosition.Y += 20;
                _secondEnemy.Position.Y += 20;
            }

            if (_mainCharacter.Dead)
                _lost = true;
            // very good for testing: 
            /*            if (CollisionHelper.Collides(new BoundingRectangle(Mouse.GetState().Position.X, Mouse.GetState().Position.Y, 1, 1), _mainCharacter.WeaponBounds))
                            _mainCharacter.Color = Color.Red;
                        else _mainCharacter.Color = Color.White;*/


            /*
                        if (_alienEnemy.AttackBounds.CollidesWith(_mainCharacter.CharacterBounds) && _alienEnemy.Damaging && !_mainCharacter.Dead)
                        {
                            _mainCharacter.Health -= _alienEnemy.AttackDamage;
                            _mainCharacter.Color = Color.Red;
                        }*/

            float respawnTime = 0;





            /*            introProgress += gameTime.ElapsedGameTime;
                        TimeSpan songDurationLeft = TimeSpan.Zero;
                        if (introProgress >= _ingameSong.Duration - TimeSpan.FromMilliseconds(85))
                        {
                            songDurationLeft.Add(TimeSpan.FromMilliseconds(introProgress.TotalMilliseconds - _ingameSong.Duration.TotalMilliseconds));
                        }

                        if (introProgress.TotalMilliseconds >= _ingameSong.Duration.TotalMilliseconds - 18)
                        {
                            introProgress = TimeSpan.Zero;
                            MediaPlayer.Stop();
                            MediaPlayer.Play(_ingameSong);
                        }
                        songDurationLeft = TimeSpan.Zero;*/


            // responsible for time limit
            while (!_hasBegun) gameTime.ElapsedGameTime = new TimeSpan();
            _elapsedTime += gameTime.ElapsedGameTime;

            // responsible for time limit
            /*            if (_won)
                        {
                            _mainCharacter.WinManeuver = true;
                            _mainCharacter.Stopped = true; // charecter stops when game ends.
                        }

                        else if (_lost)
                        {
                            _mainCharacter.LossManeuver = true;
                            _mainCharacter.Stopped = true; // charecter stops when game ends.
                            _mainCharacter.Dead = true; // charecter dies.

                        }
            */




            // Gradually fade in or out depending on whether we are covered by the pause screen.
            if (coveredByOtherScreen)
                _pauseAlpha = Math.Min(_pauseAlpha + 1f / 23, 2);
            else
                _pauseAlpha = Math.Max(_pauseAlpha - 1f / 35, 0);

            if (IsActive)
            {
                // Apply some random jitter to make the enemy move around.
                const float randomization = 10;

                _enemyPosition.X += (float)(_random.NextDouble() - 0.5) * randomization;
                _enemyPosition.Y += (float)(_random.NextDouble() - 0.5) * randomization;

                // Apply a stabilizing force to stop the enemy moving off the screen.
                /*                var targetPosition = new Vector2(
                                    ScreenManager.GraphicsDevice.Viewport.Width / 2 - _gameFont.MeasureString("Insert Gameplay Here").X / 2,
                                    200);*/

                /*                _enemyPosition = Vector2.Lerp(_enemyPosition, targetPosition, 0.05f);
                */
                // This game isn't very fun! You could probably improve
                // it by inserting something more interesting in this space :-)
            }

            base.Update(gameTime, otherScreenHasFocus, false);
        }

        // Unlike the Update method, this will only be called when the gameplay screen is active.
        public override void HandleInput(GameTime gameTime, InputState input)
        {
            /*            if (input == null)
                            throw new ArgumentNullException(nameof(input));

                        // Look up inputs for the active player profile.
                        int playerIndex = (int)ControllingPlayer.Value;

                        var keyboardState = input.CurrentKeyboardStates[playerIndex];
                        var gamePadState = input.CurrentGamePadStates[playerIndex];

                        // The game pauses either if the user presses the pause button, or if
                        // they unplug the active gamepad. This requires us to keep track of
                        // whether a gamepad was ever plugged in, because we don't want to pause
                        // on PC if they are playing with a keyboard and have no gamepad at all!
                        bool gamePadDisconnected = !gamePadState.IsConnected && input.GamePadWasConnected[playerIndex];

                        PlayerIndex player;
                        if (_pauseAction.Occurred(input, ControllingPlayer, out player) || gamePadDisconnected)
                        {
                           // ScreenManager.AddScreen(new PauseMenuScreen(), ControllingPlayer);
                        }
                        else
                        {
                            // Otherwise move the player position.
                            var movement = Vector2.Zero;

                            if (keyboardState.IsKeyDown(Keys.Left))
                                movement.X--;

                            if (keyboardState.IsKeyDown(Keys.Right))
                                movement.X++;

                            if (keyboardState.IsKeyDown(Keys.Up))
                                movement.Y--;

                            if (keyboardState.IsKeyDown(Keys.Down))
                                movement.Y++;

                            var thumbstick = gamePadState.ThumbSticks.Left;

                            movement.X += thumbstick.X;
                            movement.Y -= thumbstick.Y;

                            if (movement.Length() > 1)
                                movement.Normalize();

                            _playerPosition += movement * 8f;
                        }*/
        }
        Color RedSun = new Color(255, 255, 255);
        Color BackgroundThree = new Color(255, 255, 255);
        private float _zoom = 1f;

        public override void Draw(GameTime gameTime)
        {
            _shakeTime += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            var spriteBatch = ScreenManager.SpriteBatch;

            //GraphicsDevice.Clear(Color.Transparent);

            //Calculate our offset vector
            float playerX = MathHelper.Clamp(_mainCharacter.CurrentPosition.X, 630, 13300);
            float offsetX = 630 - playerX;
            if (!_won && !_lost)
            _timeoffset += (float)gameTime.ElapsedGameTime.TotalMilliseconds * 0.005f;


            /*            Matrix zoomTranslation = Matrix.CreateTranslation(-1280 / 2f, -720 / 2f, 0);
                        Matrix zoomTransform = zoomTranslation * Matrix.CreateScale(0.85f) * Matrix.Invert(zoomTranslation);
            */
            // Background

            /*            if (keyboardState.IsKeyDown(Keys.K))
                        {

                        }*/
            if (!_hittingTime)
                _zoom += _timeoffset * 0.000005f;
            else _zoom = 1f;

            Matrix zoomTranslation = Matrix.CreateTranslation(-900 / 2f, -720 / 2f, 0);
            Matrix zoomScale = Matrix.CreateScale(_zoom);
            Matrix zoomTransform = zoomTranslation * zoomScale * Matrix.Invert(zoomTranslation);

            float temp = (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            temp /= 2.5f;

            if (_hittingTime && _level == 3)
            {
                if (RedSun.G > 25)
                {
                    RedSun.G -= (byte)(temp * 1.5);
                    RedSun.B -= (byte)(temp * 1.5);
                }
                if (BackgroundThree.G > 160)
                {
                    BackgroundThree.R -= (byte)(temp);
                    BackgroundThree.G -= (byte)(temp);
                    BackgroundThree.B -= (byte)(temp);
                }
            }


            /*            else if (!_hittingTime)
                        {
                            RedSun.G = 255;
                            RedSun.B = 255;
                            BackgroundThree.R = 255;
                            BackgroundThree.G = 255;
                            BackgroundThree.B = 255;

                        }*/




            spriteBatch.Begin(transformMatrix: zoomTransform);
            if (_level == 1)
            {
                spriteBatch.Draw(_background1_MainLayer, new Vector2(0, 0), new Rectangle(0, 0, 576, 324), Color.White, 0, new Vector2(0, 0), (float)2.235, SpriteEffects.None, 0.3f);
            }
            else if (_level == 2)
            {
                spriteBatch.Draw(_background2_MainLayer, new Vector2(0, 0), new Rectangle(0, 0, 576, 324), Color.White, 0, new Vector2(0, 0), (float)2.235, SpriteEffects.None, 0.1f);
            }
            else if (_level == 3)
            {
                spriteBatch.Draw(_background3_MainLayer, new Vector2(0, 0), new Rectangle(0, 0, 512, 320), BackgroundThree, 0, new Vector2(0, 0), (float)2.502, SpriteEffects.None, 0.5f);
            }

            /*            spriteBatch.Draw(_background2_MainLayer, new Vector2(0, 0), new Rectangle(0, 0, 576, 413), Color.White, 0, new Vector2(0, 0), (float)2.235, SpriteEffects.None, 0.1f);
            spriteBatch.Draw(_background2_Layer1, new Vector2(0, 0), new Rectangle(0, 0, 576, 324), Color.White, 0, new Vector2(0, 0), (float)2.235, SpriteEffects.None, 0.2f);
            spriteBatch.Draw(_background2_Layer2, new Vector2(0, 0), new Rectangle(0, 0, 576, 324), Color.White, 0, new Vector2(0, 0), (float)2.235, SpriteEffects.None, 0.3f);
            spriteBatch.Draw(_background2_Ground, new Vector2(0, 0), new Rectangle(0, 0, 576, 104), Color.White, 0, new Vector2(0, -220), (float)2.235, SpriteEffects.None, 0.5f);
*/
            /*            
            */
            /*            _alienEnemy.Draw(gameTime, spriteBatch);*/
            /*  
                        _backgroundDetails.Draw(spriteBatch);
                                  _mainCharacter.Draw(gameTime, spriteBatch);
                        */
            spriteBatch.End();



            spriteBatch.Begin(transformMatrix: Matrix.CreateTranslation(_timeoffset * 1.3f, 0, 0) * zoomTransform);

            if (_level == 1)
            {
                spriteBatch.Draw(_background1_Layer1, new Vector2(0, 0), new Rectangle(0, 0, 576, 324), Color.White, 0, new Vector2(0, 0), (float)2.235, SpriteEffects.None, 0.3f);
            }
            else if (_level == 2)
            {
                spriteBatch.Draw(_background2_Layer1, new Vector2(0, 0), new Rectangle(0, 0, 576, 324), Color.White, 0, new Vector2(0, 0), (float)2.235, SpriteEffects.None, 0.2f);
            }
            spriteBatch.End();

            if (_level == 3)
            {  float tempy = Math.Min(_timeoffset * 0.67f, 180 * 0.42f);
                spriteBatch.Begin(transformMatrix: Matrix.CreateTranslation(0, tempy, 0) * zoomTransform);
                spriteBatch.Draw(_background3_RedSun, new Vector2(524 + 92.5f, 110 + 90), new Rectangle(290, 61, 148, 144), RedSun, 1 * _timeoffset * 0.004f, new Vector2(74, 72), (float)1.25f, SpriteEffects.None, 0.2f);
                spriteBatch.End();
            }
            spriteBatch.Begin(transformMatrix: Matrix.CreateTranslation(_timeoffset * 0.65f, 0, 0) * zoomTransform);
            if (_level == 1)
            {
                spriteBatch.Draw(_background1_Layer3, new Vector2(0, 0), new Rectangle(0, 0, 576, 324), Color.White, 0, new Vector2(0, 0), (float)2.235, SpriteEffects.None, 0.3f);
                spriteBatch.Draw(_background1_Layer2, new Vector2(0, 0), new Rectangle(0, 0, 576, 324), Color.White, 0, new Vector2(0, 0), (float)2.235, SpriteEffects.None, 0.3f);
            }
            else if (_level == 2)
            {
                spriteBatch.Draw(_background2_Layer2, new Vector2(0, 0), new Rectangle(0, 0, 576, 324), Color.White, 0, new Vector2(0, 0), (float)2.235, SpriteEffects.None, 0.3f);
            }
            spriteBatch.End();
            spriteBatch.Begin(transformMatrix: zoomTransform);
            if (_level == 1)
            {
                spriteBatch.Draw(_background1_Layer4, new Vector2(0, 0), new Rectangle(0, 0, 576, 324), Color.White, 0, new Vector2(0, 0), (float)2.235, SpriteEffects.None, 0.3f);
                spriteBatch.Draw(_background1_Ground, new Vector2(0, 0), new Rectangle(0, 0, 576, 104), Color.White, 0, new Vector2(0, -220), (float)2.235, SpriteEffects.None, 0.5f);
            }
            else if (_level == 2)
            {
                spriteBatch.Draw(_background2_Layer3, new Vector2(0, 0), new Rectangle(0, 0, 576, 324), Color.White, 0, new Vector2(0, 0), (float)2.235, SpriteEffects.None, 0.3f);
                spriteBatch.Draw(_background2_Ground, new Vector2(0, 0), new Rectangle(0, 0, 576, 104), Color.White, 0, new Vector2(0, -220), (float)2.235, SpriteEffects.None, 0.5f);
            }
            else if (_level == 3)
            {
                //spriteBatch.Draw(_background3_RedSun, new Vector2(508, 110), new Rectangle(290, 61, 148, 144), Color.White, 0, new Vector2(0, 0), (float)1.25f, SpriteEffects.None, 0.2f);

            }

            _mainCharacter.Draw(gameTime, spriteBatch);
            _enemy.Draw(gameTime, spriteBatch);
            _secondEnemy.Draw(gameTime, spriteBatch);
            _lastEnemy.Draw(gameTime, spriteBatch);
            spriteBatch.End();


/*
            spriteBatch.Begin(transformMatrix: Matrix.CreateTranslation(offsetX, 0, 0) * CameraSettings.WaveShakeEffect(_shakeTime) * CameraSettings.DrownShakeEffect(_shakeTime, _isDrowning));
            *//*            _wave.Draw(spriteBatch);
                        _outerWaveEffectOne.Draw(spriteBatch);
                        _outerWaveEffectTwo.Draw(spriteBatch);*//*
            spriteBatch.End();

            spriteBatch.Begin(blendState: BlendState.Additive, transformMatrix: Matrix.CreateTranslation(offsetX, 0, 0) * CameraSettings.WaveShakeEffect(_shakeTime) * CameraSettings.DrownShakeEffect(_shakeTime, _isDrowning));
            *//*
                        _innerWaveEffectOne.Draw(spriteBatch);
                        _innerWaveEffectTwo.Draw(spriteBatch);
            *//*
            spriteBatch.End();

            spriteBatch.Begin(transformMatrix: Matrix.CreateTranslation(offsetX, 0, 0) * CameraSettings.DrownShakeEffect(_shakeTime, _isDrowning));

            *//*            _forestFrontLayer.Draw(spriteBatch);
            *//*



            spriteBatch.End();*/


            // TODO: Add your drawing code here
            spriteBatch.Begin();
            /*            _staminaSprite.Draw(spriteBatch);
            */
            /*            if (_startTimer > 0)
                        {
                            spriteBatch.DrawString(spriteFont, "RUN! -->", new Vector2(355, 75) * 1.6f, Color.White);
                        }

                        if (_won)
                        {
                            spriteBatch.DrawString(spriteFont, "You win!", new Vector2(335, 195) * 1.6f, Color.White);
                        }*/
            /*            int score = _killed * 500;
            */
            /*            float showRoundTime = (_level > 1) ? 3000 : 1500;
            */
            if (_level <= 2)
            {
                if (_timeUntilSpaceCounted < 3500 && _timeUntilSpaceCounted > 1400)
                {
                    spriteBatch.DrawString(spriteFont, $"Round {_level}", new Vector2(335, 193) * 1.6f, Color.White);
                }
            }
            else
            {
                if (_timeUntilSpaceCounted < 3500 && _timeUntilSpaceCounted > 1400)
                {
                    spriteBatch.DrawString(spriteFont, "Final Round", new Vector2(300, 193) * 1.6f, Color.White);
                }
            }


            if (_pressToContinue)
            {
                spriteBatch.DrawString(spriteFont, "You Win This Round!", new Vector2(250, 165) * 1.6f, Color.White);
                spriteBatch.DrawString(spriteFont, "Press Any Key To Continue!", new Vector2(187, 195) * 1.6f, Color.White);

            }
            if (_lost || _won)
            {
                if (_lost)
                    spriteBatch.DrawString(spriteFont, "You Lost!", new Vector2(310, 105) * 1.6f, Color.White);
                else
                    spriteBatch.DrawString(spriteFont, "You Win!", new Vector2(330, 185) * 1.6f, Color.White);

                /*                spriteBatch.DrawString(spriteFont, $"Score: {score}", new Vector2(302, 228) * 1.6f, Color.White);
                                if (!_saved)
                                {
                                    DBModel.SaveList(score);
                                    _saved = true;
                                }*/
            }
            /*            else spriteBatch.DrawString(spriteFont, $"Score: {score}", new Vector2(310, 30) * 1.6f, Color.White);
            */

            spriteBatch.End();





            base.Draw(gameTime);
            /*
                // This game has a blue background. Why? Because!
                ScreenManager.GraphicsDevice.Clear(ClearOptions.Target, Color.CornflowerBlue, 0, 0);

                // Our player and enemy are both actually just text strings.
                var spriteBatch = ScreenManager.SpriteBatch;

                spriteBatch.Begin();

    *//*            spriteBatch.DrawString(_gameFont, "// TODO", _playerPosition, Color.Green);
                spriteBatch.DrawString(_gameFont, "Insert Gameplay Here",
                                       _enemyPosition, Color.DarkRed);

            spriteBatch.End();*/

            // If the game is transitioning on or off, fade it out to black.
            if (TransitionPosition > 0 || _pauseAlpha > 0)
            {
                float alpha = MathHelper.Lerp(1f - TransitionAlpha, 1f, _pauseAlpha / 2);

                ScreenManager.FadeBackBufferToBlack(alpha);
            }
        }
    }
}
