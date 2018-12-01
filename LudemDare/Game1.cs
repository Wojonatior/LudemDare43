using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Linq;

namespace LudemDare.Desktop
{
    public struct GameObject {
        public Vector2 position;
        public Vector2 velocity;
        public Texture2D texture;
        public System.Func<KeyboardState,GameTime,GameObject,GameObject> update;
    }
    
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        private GameObject Player;
        private System.Collections.Generic.Dictionary<string, GameObject> GameObjects
            = new System.Collections.Generic.Dictionary<string, GameObject>();


        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            //Initalizer.getStartingPlayer(seed);
            Player.position = new Vector2(
                graphics.PreferredBackBufferWidth / 2,
                graphics.PreferredBackBufferHeight / 2);

            Player.velocity = new Vector2(200, 200);
            Player.update = updatePlayer;

            GameObjects.Add("PLAYER", Player);
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
            Player.texture = Content.Load<Texture2D>("btc_128");

            var tempPlayer = GameObjects["PLAYER"];
            tempPlayer.texture = Content.Load<Texture2D>("btc_128");
            GameObjects["PLAYER"] = tempPlayer;


            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        private static GameObject updatePlayer(KeyboardState kState, GameTime gameTime, GameObject player){
            var newPos = player.position;
            //TODO: think about how rollover is handled
            if (kState.IsKeyDown(Keys.Up))
                newPos.Y -= player.velocity.Y * (float)gameTime.ElapsedGameTime.TotalSeconds;
            
            if(kState.IsKeyDown(Keys.Down))
                newPos.Y +=  player.velocity.Y * (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (kState.IsKeyDown(Keys.Left))
                newPos.X -= player.velocity.X * (float)gameTime.ElapsedGameTime.TotalSeconds;
            
            if(kState.IsKeyDown(Keys.Right))
                newPos.X +=  player.velocity.X * (float)gameTime.ElapsedGameTime.TotalSeconds;

            player.position = newPos;
            return player;
        }
        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            var kState = Keyboard.GetState();
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || kState.IsKeyDown(Keys.Escape))
                Exit();


            var newDict  = new System.Collections.Generic.Dictionary<string, GameObject>();
            foreach (var labeledObject in GameObjects) {
                newDict[labeledObject.Key] = 
                    labeledObject.Value.update(kState, gameTime, labeledObject.Value);
            }
            GameObjects = newDict;

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
