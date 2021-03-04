using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Snake
{
    public class Snake
    {
        public Vector2 Position;
        public Texture2D Texture { get; set; }
        public int Size { get; set; }
        public Color Tint { get; set; }
        public Directions CurrentDirection { get; set; }
        public int Speed { get; set; }
        public Rectangle Hitbox => new Rectangle((int) Position.X, (int) Position.Y, Size, Size);

        public Snake(Texture2D texture, Vector2 position, int size, Color tint, int speed)
        {
            CurrentDirection = Directions.None;
            Texture = texture;
            Position = position;
            Size = size;
            Tint = tint;
            Speed = speed;
        }

        public void UpdateDirection()
        {
            KeyboardState ks = Keyboard.GetState();

            if (ks.IsKeyDown(Keys.Up))
            {
                CurrentDirection = Directions.Up;
            }
            else if (ks.IsKeyDown(Keys.Down))
            {
                CurrentDirection = Directions.Down;
            }
            else if (ks.IsKeyDown(Keys.Left))
            {
                CurrentDirection = Directions.Left;
            }
            else if (ks.IsKeyDown(Keys.Right))
            {
                CurrentDirection = Directions.Right;
            }
        }

        public void Move(GameTime gameTime)
        {
            switch (CurrentDirection)
            {
                case Directions.None:
                    break;
                case Directions.Up:
                    Position.Y -= Speed;
                    break;
                case Directions.Down:
                    Position.Y += Speed;
                    break;
                case Directions.Left:
                    Position.X -= Speed;
                    break;
                case Directions.Right:
                    Position.X += Speed;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void Draw(SpriteBatch batch)
        {
            batch.Draw(Texture, Hitbox, Tint);
        }
    }
}