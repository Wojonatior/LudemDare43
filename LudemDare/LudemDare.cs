using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System;

namespace LudemDare.Desktop
{
    public class BOIClone : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        static ProjectileFactory localProjectileFactory;
        static EnemyFatory localEnemyFactory;
        private GameObject Player;
        private Dictionary<string, Texture2D> Textures
            = new Dictionary<string, Texture2D>();
        private Dictionary<string, GameObject> GameObjects
            = new Dictionary<string, GameObject>();
        Random random;
        bool EHeld;


        public BOIClone()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            //Initalizer.getStartingPlayer(seed);

            EHeld = false;
            random = new Random();
            Player = PlayerFactory.CreatePlayer(graphics);
            Player.addItem = AddFromPlayer;

            GameObjects.Add("PLAYER", Player);
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            Textures["PLAYER"] = Content.Load<Texture2D>("btc_128");
            Textures["PROJECTILE"] = Content.Load<Texture2D>("btc_32");
            Textures["ENEMY"] = Content.Load<Texture2D>("blue_128");

            localProjectileFactory = new ProjectileFactory(Textures["PROJECTILE"]);
            localEnemyFactory = new EnemyFatory(Textures["ENEMY"]);

            var tempPlayer = GameObjects["PLAYER"];
            tempPlayer.texture = Textures["PLAYER"];
            GameObjects["PLAYER"] = tempPlayer;
        }

        protected override void UnloadContent() { // TODO: Unload any non ContentManager content here
        }

        private static GameObject? AddFromPlayer(KeyboardState kState, GameTime gameTime, GameObject player)
        {
            switch (player.otherStrings["attackDirection"]) {
                case "up":
                    return localProjectileFactory.CreateUp(player.position);
                case "down":
                    return localProjectileFactory.CreateDown(player.position);
                case "left":
                    return localProjectileFactory.CreateLeft(player.position);
                case "right":
                    return localProjectileFactory.CreateRight(player.position);
            }
            return null;
        }

        public static Dictionary<string, GameObject> UpdateExistingObjects (Dictionary<string, GameObject> GameObjects, KeyboardState kState, GameTime gameTime, GraphicsDeviceManager graphics){
            var newDict = new Dictionary<string, GameObject>();
            foreach (var labeledObject in GameObjects)
            {
                newDict[labeledObject.Key] =
                    labeledObject.Value.update(kState, gameTime, labeledObject.Value, graphics);
            }
            return newDict;
        }

        public static Dictionary<string, GameObject> CreateNewObjects (Dictionary<string, GameObject> GameObjects, KeyboardState kState, GameTime gameTime){
            var newDict = new Dictionary<string, GameObject>();
            foreach (var labeledObject in GameObjects)
            {
                var newObject = labeledObject.Value.addItem(kState, gameTime, labeledObject.Value);
                if (newObject.HasValue)
                    newDict[Guid.NewGuid().ToString()] = newObject.Value;
            }
            return newDict;
        }

        private GameObject MakeRandomEnemy(){
            return localEnemyFactory.createEnemy(new Vector2(
                random.Next(64, graphics.PreferredBackBufferWidth - 64),
                random.Next(64, graphics.PreferredBackBufferHeight - 64)
            ));
        }

        public static BoundingBox getBBfromGameObject(GameObject gObject)
        {
            var min = new Vector3(gObject.position.X - gObject.texture.Width / 2, gObject.position.Y - gObject.texture.Height / 2, 0);
            var max = new Vector3(gObject.position.X + gObject.texture.Width / 2, gObject.position.Y + gObject.texture.Height / 2, 0);
            return new BoundingBox(min, max);
        }

        private static bool CheckCollision(GameObject obj1, GameObject obj2, GameTime gameTime) {
            var posVector = new Vector3(obj1.position, 0);
            var velVector = new Vector3(obj1.velocity, 0);
            var adjustedVelVector = Vector3.Multiply(velVector, gameTime.ElapsedGameTime.Seconds);
            //var combinedVelVector = MATH // TODO: Add both velocity vectors together here
            var velocityRay = new Ray(posVector, adjustedVelVector);

            var leftPaddleBB = getBBfromGameObject(obj2);

            var collisionPoint = velocityRay.Intersects(leftPaddleBB);
            //leftCollision = leftPaddleCollisionPoint;
            return CheckCircleCollision(obj1, obj2);
            //return collisionPoint != null || CheckCircleCollision(obj1, obj2);
        }

        private static bool CheckRectangleCollision(GameObject obj1, GameObject obj2){
            return obj1.position.X < obj2.position.X + obj2.texture.Width &&
                obj1.position.X + obj1.texture.Width > obj2.position.X &&
                obj1.position.Y < obj2.position.Y + obj2.texture.Height &&
                obj1.position.Y + obj1.texture.Height > obj2.position.Y;
        }

        private static bool CheckCircleCollision(GameObject obj1, GameObject obj2)
        {
            var dist_x = obj1.position.X - obj2.position.X;
            var dist_y = obj1.position.Y - obj2.position.Y;
            var distance = Math.Sqrt(dist_x * dist_x + dist_y * dist_y);

            return distance < obj1.texture.Width/2 + obj2.texture.Width/2;
        }

        private static Dictionary<String,GameObject> CheckCollisionForAllObjects(Dictionary<String,GameObject> gameObjects, GameTime gameTime){

            var updatedObjects = new Dictionary<String,GameObject>(gameObjects);
            foreach (var obj in gameObjects) {
                foreach (var otherObj in gameObjects) {
                    if (obj.Key == otherObj.Key)
                        continue;

                    var didCollide = CheckCollision(obj.Value, otherObj.Value, gameTime);
                    updatedObjects[obj.Key] = didCollide ?
                        obj.Value.resolveCollision(obj.Value, otherObj.Value) :
                        obj.Value;
                }
            }
            return gameObjects.Count == 1 ? gameObjects : updatedObjects;
        }

        protected override void Update(GameTime gameTime) {

            var kState = Keyboard.GetState();
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || kState.IsKeyDown(Keys.Escape))
                Exit();

            GameObjects = UpdateExistingObjects(GameObjects, kState, gameTime, graphics);
            GameObjects = CheckCollisionForAllObjects(GameObjects, gameTime);
            var newObjects = CreateNewObjects(GameObjects, kState, gameTime);
            var toDelete = new List<string>();

            foreach (var obj in newObjects)
                GameObjects[obj.Key] = obj.Value;

            foreach (var obj in GameObjects)
                if (obj.Value.shouldDelete)
                    toDelete.Add(obj.Key);

            foreach (var key in toDelete)
                GameObjects.Remove(key);

            if (kState.IsKeyDown(Keys.E) && !EHeld) {
                EHeld = true;
                GameObjects[Guid.NewGuid().ToString()] = MakeRandomEnemy();
            }
            else if(kState.IsKeyUp(Keys.E))
                EHeld = false;

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();
            foreach (var gameObject in GameObjects) {
                spriteBatch.Draw(
                    gameObject.Value.texture,
                    gameObject.Value.position,
                    null,
                    Color.White,
                    0f,
                    new Vector2(gameObject.Value.texture.Width / 2, gameObject.Value.texture.Height / 2),
                    Vector2.One,
                    SpriteEffects.None,
                    0f);
            };
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
