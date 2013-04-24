using System;
using System.Collections.Generic;
using System.Linq;
using Lecture7Examples;
using LectureExamples5;
using MS.Internal.Xml.XPath;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;


namespace CollisionVisualization.Collision
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class CollisionManager : Microsoft.Xna.Framework.DrawableGameComponent
    {
        public float CollisionStepTime { get; set; }

        private List<Collider> _colliders = new List<Collider>();
        private float _stepTimer;

        private int _currenCollisionFocus;
        private int _currentCollisionTarget;

        private DrawData _targetBox;
        private DrawData _focusBox;

        public CollisionManager(Game game, float stepTime = 0.25f)
            : base(game)
        {
            CollisionStepTime = stepTime;
            _currentCollisionTarget = 1;
        }

        public void AddCollider(Collider collider)
        {
            _colliders.Add(collider);
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            // TODO: Add your initialization code here

            base.Initialize();
        }

        protected override void LoadContent()
        {
            base.LoadContent();
            IDrawSprites drawer = (IDrawSprites)Game.Services.GetService(typeof(IDrawSprites));
            Texture2D transparentPixel = Game.Content.Load<Texture2D>("TransparentPixel");
            _targetBox = new DrawData(transparentPixel);
            _targetBox.BlendColor = Color.Green;
            _focusBox = new DrawData(transparentPixel);
            _focusBox.BlendColor = Color.Blue;
            drawer.AddDrawable(_targetBox);
            drawer.AddDrawable(_focusBox);
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            _stepTimer += (float) gameTime.ElapsedGameTime.TotalSeconds;
            if (_stepTimer >= CollisionStepTime)
            {
                step();
                _stepTimer -= CollisionStepTime;
            }

            base.Update(gameTime);
        }

        private void step()
        {
            if(_colliders.Count < 2)
                return;
            if (_colliders[_currenCollisionFocus].CollisionRectangle.Intersects(_colliders[_currentCollisionTarget].CollisionRectangle))
            {
                _colliders[_currenCollisionFocus].Parent.OnCollision(_colliders[_currentCollisionTarget]);
                _colliders[_currentCollisionTarget].Parent.OnCollision(_colliders[_currenCollisionFocus]);
                _colliders[_currenCollisionFocus].StepBack();
                _colliders[_currentCollisionTarget].StepBack();
            }

            advanceCollisionLoop();
            if(_currentCollisionTarget == _currenCollisionFocus)
                advanceCollisionLoop();

            _focusBox.Destination =  _colliders[_currenCollisionFocus].CollisionRectangle;
            _targetBox.Destination =  _colliders[_currentCollisionTarget].CollisionRectangle;
        }

        private void advanceCollisionLoop()
        {
            _currentCollisionTarget++;

            if (_currentCollisionTarget >= _colliders.Count)
            {
                _currentCollisionTarget = 0;
                _currenCollisionFocus++;
                if (_currenCollisionFocus >= _colliders.Count)
                    _currenCollisionFocus = 0;
            }
        }
    }

    public class Collider
    {
        public GameObject Parent { get; set; }

        /// <summary>
        /// Center of the collider
        /// </summary>
        public Vector2 Position
        {
            get { return new Vector2(_currentRect.Center.X, _currentRect.Center.Y); }
            set
            {
                _previousRect = _currentRect;
                _currentRect.X =  (int) value.X - _currentRect.Width/2;
                _currentRect.Y =  (int) value.Y - _currentRect.Height/2;
            }
        }

        public Rectangle CollisionRectangle { get { return _currentRect; } }
        public int Width {
            get { return _currentRect.Width; }
            set { _currentRect.Width = _previousRect.Width = value; }
        }
        public int Height { get { return _currentRect.Height; }
            set { _currentRect.Width = _previousRect.Width = value; }
        }

        private Rectangle _currentRect;
        private Rectangle _previousRect;

        public Collider(GameObject parent, Rectangle bounds)
        {
            _currentRect = _previousRect = bounds;
            Parent = parent;
        }

        /// <summary>
        /// Returns the collider to it's previous position.
        /// </summary>
        public void StepBack()
        {
            Position = new Vector2(_previousRect.Center.X, _previousRect.Center.Y);
        }

    }
}
