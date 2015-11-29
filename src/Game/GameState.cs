using amulware.Graphics;
using Bearded.Utilities.Collections;
using Bearded.Utilities.Input;
using Bearded.Utilities.Math;
using OpenTK;
using OpenTK.Input;

namespace Clouds.Game
{
    sealed class GameState
    {
        private readonly DeletableObjectList<GameObject> gameObjects = new DeletableObjectList<GameObject>();
        private readonly DeletableObjectList<Ship> ships = new DeletableObjectList<Ship>();
        private readonly DeletableObjectList<Ship> playerShips = new DeletableObjectList<Ship>();

        private double time = 0;
        private float timeF = 0;

        public double Time { get { return this.time; } }
        public float TimeF { get { return this.timeF; } }

        public DeletableObjectList<Ship> Ships { get { return this.ships; } }
        public DeletableObjectList<Ship> PlayerShips { get { return this.playerShips; } }

        public GameState()
        {
            var ship = new Ship(this, Vector2.Zero, new KeyboardShipController(this));
            new PlayerView(this, ship);
            this.playerShips.Add(ship);

            for (int i = 0; i < 4; i++)
            {
                var x = 2 -1.33f * i;
                var a = Angle.FromDegrees(90);
                ship.AddEquipment(new Cannon(this, new Vector2(x, 2), a, GunControlGroup.Left));
                ship.AddEquipment(new Cannon(this, new Vector2(x, -2), -a, GunControlGroup.Right));
            }

            for (int i = 0; i < 5; i++)
            {
                var x = i * 20;
                new Ship(this, new Vector2(x, 50),  new SimpleEnemyShipController(this));
            }
        }

        public void Add(GameObject gameObject)
        {
            this.gameObjects.Add(gameObject);
        }

        public void Update(UpdateEventArgs args)
        {
            var elapsedTime = args.ElapsedTimeInS * 5;

            if (InputManager.IsKeyPressed(Key.Space))
                elapsedTime /= 30;

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