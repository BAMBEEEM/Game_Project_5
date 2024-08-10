using Game_Project_5.Collisions;
using Game_Project_5.ParticleManagement;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SharpDX.Direct2D1.Effects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game_Project_5.GameButtons
{
    /// <summary>
    /// A class that represents the menu buttons.
    /// </summary>
    public class StartButton
    {

        private Texture2D _texture;

        private BoundingRectangle bounds => new BoundingRectangle(new Vector2(286 + 16, 196) * 1.52f, 92 * 2.3f * 1.52f, 33 * 2.3f * 1.52f);
        private Vector2 _position => new Vector2(286 + 16, 196) * 1.52f;
        public Color Shade = Color.White;
        public bool InitialClick = false;

        public bool IsSelected = true;
        private Color _color = new Color((uint)RandomHelper.Next(0, 255), (uint)RandomHelper.Next(0, 255), (uint)RandomHelper.Next(0, 255));
        /// <summary>
        /// The bounding rectangle of the StartButton
        /// </summary>
        public BoundingRectangle Bounds => bounds;

        public void LoadContent(ContentManager content)
        {
            _texture = content.Load<Texture2D>("Button");
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            if (IsSelected == true)
                Shade = Color.Thistle;
/*            float tt = (float)Math.Sin(gameTime.TotalGameTime.TotalSeconds);
            Shade.A *=(byte)(255*tt);*/
            spriteBatch.Draw(_texture, _position, new Rectangle(0, 0, 400, 170), Shade, 0, new Vector2(0, 0), 0.8f, SpriteEffects.None, 0);

        }
    }

}
