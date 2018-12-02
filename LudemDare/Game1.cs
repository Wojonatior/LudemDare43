﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System;

namespace LudemDare.Desktop
{
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        static ProjectileFactory localProjectileFactory;
        private GameObject Player;
        private Dictionary<string, Texture2D> Textures
            = new Dictionary<string, Texture2D>();
        private Dictionary<string, GameObject> GameObjects
            = new Dictionary<string, GameObject>();


        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            //Initalizer.getStartingPlayer(seed);

            Player = PlayerFactory.CreatePlayer(graphics);
            Player.addItem = AddFromPlayer;

            GameObjects.Add("PLAYER", Player);
            base.Initialize();
        }

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            Textures["PLAYER"] = Content.Load<Texture2D>("btc_128");
            Textures["PROJECTILE"] = Content.Load<Texture2D>("btc_32");

            localProjectileFactory = new ProjectileFactory(Textures["PROJECTILE"]);

            var tempPlayer = GameObjects["PLAYER"];
            tempPlayer.texture = Textures["PLAYER"];
            GameObjects["PLAYER"] = tempPlayer;
        }

        protected override void UnloadContent() { // TODO: Unload any non ContentManager content here
        }


        private static GameObject? AddFromPlayer(KeyboardState kState, GameTime gameTime, GameObject player)
        {
            if (kState.IsKeyDown(Keys.Up))
                return localProjectileFactory.CreateUp(player.position);

            if (kState.IsKeyDown(Keys.Down))
                return localProjectileFactory.CreateDown(player.position);

            if (kState.IsKeyDown(Keys.Left))
                return localProjectileFactory.CreateLeft(player.position);

            if (kState.IsKeyDown(Keys.Right))
                return localProjectileFactory.CreateRight(player.position);

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

        protected override void Update(GameTime gameTime) {

            var kState = Keyboard.GetState();
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || kState.IsKeyDown(Keys.Escape))
                Exit();

            GameObjects = UpdateExistingObjects(GameObjects, kState, gameTime, graphics);
            var newObjects = CreateNewObjects(GameObjects, kState, gameTime);
            var toDelete = new List<string>();

            foreach (var obj in newObjects)
                GameObjects[obj.Key] = obj.Value;

            foreach (var obj in GameObjects)
                if (obj.Value.shouldDelete)
                    toDelete.Add(obj.Key);

            foreach (var key in toDelete)
                GameObjects.Remove(key);

            base.Update(gameTime);
        }

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
