using amulware.Graphics;
using Bearded.Utilities.Collections;
using Bearded.Utilities.Math;
using OpenTK;

namespace Clouds.Game
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
            var ship = new Ship(this, new KeyboardShipController(this));
            new PlayerView(this, ship);

            ship.AddEquipment(new Cannon(this, new Vector2(2, 2), Angle.FromDegrees(90), GunControlGroup.Left));
            ship.AddEquipment(new Cannon(this, new Vector2(0.66f, 2), Angle.FromDegrees(90), GunControlGroup.Left));
            ship.AddEquipment(new Cannon(this, new Vector2(-0.66f, 2), Angle.FromDegrees(90), GunControlGroup.Left));
            ship.AddEquipment(new Cannon(this, new Vector2(-2, 2), Angle.FromDegrees(90), GunControlGroup.Left));

            ship.AddEquipment(new Cannon(this, new Vector2(2, -2), Angle.FromDegrees(-90), GunControlGroup.Right));
            ship.AddEquipment(new Cannon(this, new Vector2(0.66f, -2), Angle.FromDegrees(-90), GunControlGroup.Right));
            ship.AddEquipment(new Cannon(this, new Vector2(-0.66f, -2), Angle.FromDegrees(-90), GunControlGroup.Right));
            ship.AddEquipment(new Cannon(this, new Vector2(-2, -2), Angle.FromDegrees(-90), GunControlGroup.Right));

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