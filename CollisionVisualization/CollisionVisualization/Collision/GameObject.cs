using LectureExamples5;
using Microsoft.Xna.Framework;

namespace CollisionVisualization.Collision
{
    public class GameObject
    {

        public Collider CollisionBody { get; protected set; }
        public DrawData Visuals { get; set; }

        public GameObject()
        {
            
        }

        public GameObject(Rectangle bounds, DrawData toDraw)
        {
            CollisionBody = new Collider(this, bounds);
            Visuals = toDraw;
        }

        public virtual void Update(GameTime gameTime)
        {
            
        }

        public virtual void Initialize(Game game)
        {
            
        }

        public virtual void OnCollision(Collider collidedWith)
        {
            
        }
    }
}