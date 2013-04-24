using System;
using System.Collections.Generic;
using System.Linq;
using CollisionVisualization.Collision;
using Lecture7Examples;
using LectureExamples5;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace CollisionVisualization
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        List<GameObject> _gameObjects = new List<GameObject>(); 

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            CollisionManager _collisionManager = new CollisionManager(this, 0.4f);
            Components.Add(_collisionManager);
            Services.AddService(typeof(CollisionManager), _collisionManager);

            SpriteComponent spriteComponent = new SpriteComponent(this);
            Components.Add(spriteComponent);
            Services.AddService(typeof(IDrawSprites), spriteComponent);
            IsMouseVisible = true;

            graphics.PreferredBackBufferHeight = 800;
            graphics.ApplyChanges();
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            Texture2D rockArt = Content.Load<Texture2D>("Rock");
            
            for (int i = 0; i < 5; i++)
            {
                DrawData drawable = new DrawData(rockArt, new Rectangle(i*rockArt.Width,600,rockArt.Width, rockArt.Height));
                _gameObjects.Add(new GameObject(new Rectangle(i*rockArt.Width + 6, 664,86,90), drawable));
                ((IDrawSprites)Services.GetService(typeof(IDrawSprites))).AddDrawable(drawable);
                ((CollisionManager)Services.GetService(typeof(CollisionManager))).AddCollider(_gameObjects[i].CollisionBody);
            }
   
            _gameObjects.Add(new PlayerCharacter(new Vector2(100,300)));
            _gameObjects[_gameObjects.Count-1].Initialize(this);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            foreach (GameObject gameObject in _gameObjects)
            {
                gameObject.Update(gameTime);
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
