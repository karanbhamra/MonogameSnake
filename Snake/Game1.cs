using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Snake
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private Snake _food;
        private List<Snake> _snakePieces;
        private Texture2D _pixel;

        private const int Side = 50;
        private const int NumTiles = 10;
        private TimeSpan _delay = TimeSpan.FromMilliseconds(250);
        private TimeSpan _elapsed;

        private static Random _gen = new Random();
        private Rectangle Screen => GraphicsDevice.Viewport.Bounds;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            _graphics.PreferredBackBufferWidth = Side * NumTiles;
            _graphics.PreferredBackBufferHeight = Side * NumTiles;
            _graphics.ApplyChanges();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            _pixel = GetPixel(GraphicsDevice);
            _elapsed = TimeSpan.Zero;

            var foodx = _gen.Next(NumTiles) * Side;
            var foody = _gen.Next(NumTiles) * Side;
            _food = new Snake(_pixel, new Vector2(foodx, foody), Side, Color.Orange, 0);
            // TODO: use this.Content to load your game content here
            _snakePieces = new List<Snake>();
            _snakePieces.Add(new Snake(_pixel, Vector2.Zero, Side, Color.Green, Side));
        }

        protected override void Update(GameTime gameTime)
        {
            _elapsed += gameTime.ElapsedGameTime;

            _snakePieces[0].UpdateDirection();

            if (_elapsed >= _delay)
            {
                _elapsed = TimeSpan.Zero;

                for (int i = _snakePieces.Count - 1; i >= 0; i--)
                {
                    if (i == 0)
                    {
                        _snakePieces[i].Move(gameTime);
                    }
                    else
                    {
                        _snakePieces[i].Position = _snakePieces[i - 1].Position;
                    }
                }
            }

            // collision check with food
            if (_snakePieces[0].Hitbox.Intersects(_food.Hitbox))
            {
                var foodx = _gen.Next(NumTiles) * Side;
                var foody = _gen.Next(NumTiles) * Side;
                _food.Position = new Vector2(foodx, foody);

                Color randomColor = new Color(_gen.Next(256), _gen.Next(256), _gen.Next(256));
                // add the snake piece anywhere
                _snakePieces.Add(new Snake(_pixel, _snakePieces[0].Position, Side, randomColor, 0));
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            _spriteBatch.Begin();

            DrawGrid(_spriteBatch);

            _food.Draw(_spriteBatch);

            foreach (var piece in _snakePieces)
            {
                piece.Draw(_spriteBatch);
            }

            _spriteBatch.End();

            base.Draw(gameTime);
        }

        private void DrawGrid(SpriteBatch spriteBatch)
        {
            int startx = 0;
            int starty = 0;

            for (int x = 0; x < NumTiles; x++)
            {
                Rectangle line = new Rectangle(startx, starty, 1, Screen.Height);
                spriteBatch.Draw(_pixel, line, Color.Black);
                startx += Side;
            }

            startx = 0;

            for (int y = 0; y < NumTiles; y++)
            {
                Rectangle line = new Rectangle(startx, starty, Screen.Width, 1);
                spriteBatch.Draw(_pixel, line, Color.Black);
                starty += Side;
            }
        }

        private Texture2D GetPixel(GraphicsDevice device)
        {
            var tex = new Texture2D(device, 1, 1);
            tex.SetData(new Color[] {Color.White});
            return tex;
        }
    }
}