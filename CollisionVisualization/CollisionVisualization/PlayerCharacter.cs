using CollisionVisualization.Collision;
using Lecture7Examples;
using LectureExamples5;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace CollisionVisualization
{
    public class PlayerCharacter : GameObject
    {
        private MouseState _prev;
        private MouseState _cur;

        private Vector2 _startPos;

        public PlayerCharacter(Vector2 startingPosition)
        {
            _startPos = startingPosition;
        }

        public override void Initialize(Game game)
        {
            base.Initialize(game);
            _prev = _cur = Mouse.GetState();

            Texture2D bugArt = game.Content.Load<Texture2D>("Enemy Bug");
            CollisionBody = new Collider(this, new Rectangle((int)_startPos.X, (int)_startPos.Y,bugArt.Width,75));
            ((CollisionManager)game.Services.GetService(typeof(CollisionManager))).AddCollider(CollisionBody);
            Rectangle destination = bugArt.Bounds;
            destination.X = CollisionBody.CollisionRectangle.X;
            destination.Y = CollisionBody.CollisionRectangle.Y;
            Visuals = new DrawData(bugArt,bugArt.Bounds);
            Visuals.Origin = new Vector2(bugArt.Width / 2, bugArt.Height / 2.0f + 37.5f);
            ((IDrawSprites) game.Services.GetService(typeof (IDrawSprites))).AddDrawable(Visuals);
        }

        public override void Update(GameTime gameTime)
        {
            _cur = Mouse.GetState();
            base.Update(gameTime);
            Rectangle temp = Visuals.Destination;
            temp.X = (int)CollisionBody.Position.X;
            temp.Y = (int)CollisionBody.Position.Y;
            Visuals.Destination = temp;
            if (_cur.LeftButton == ButtonState.Pressed && _prev.LeftButton == ButtonState.Released)
            {
                CollisionBody.Position = new Vector2(_cur.X, _cur.Y);
            }
            _prev = _cur;
        }
    }
}