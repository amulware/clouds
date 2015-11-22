using amulware.Graphics;
using Bearded.Utilities.Collections;

namespace Game
{
    sealed class GameState
    {
        private readonly DeletableObjectList<GameObject> gameObjects = new DeletableObjectList<GameObject>();
        private double time = 0;
        private float timeF = 0;

        public double Time { get { return this.time; } }
        public float TimeF { get { return this.timeF; } }

        public GameState()
        {
            
        }

        public void Add(GameObject gameObject)
        {
            this.gameObjects.Add(gameObject);
        }

        public void Update(UpdateEventArgs args)
        {
            var elapsedTime = args.ElapsedTimeInS;

            this.time += elapsedTime;
            this.timeF = (float)this.time;

            var elapsedTimeF = (float)elapsedTime;

            foreach (var gameObject in this.gameObjects)
            {
                gameObject.Update(elapsedTimeF);
            }
        }

        public void Render()
        {
            foreach (var gameObject in this.gameObjects)
            {
                gameObject.Draw();
            }
        }
    }
}