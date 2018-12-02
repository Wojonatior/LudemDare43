using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace LudemDare
{
    public class PlayerFactory
    {
        public PlayerFactory()
        {
        }

        public static GameObject CreatePlayer(GraphicsDeviceManager graphics){
            var player = new GameObject();
            player.position = new Vector2(
                graphics.PreferredBackBufferWidth / 2,
                graphics.PreferredBackBufferHeight / 2);

            player.velocity = new Vector2(200, 200);
            player.update = UpdatePlayer;

            return player;
        }

        private static GameObject UpdatePlayer(KeyboardState kState, GameTime gameTime, GameObject player)
        {
            var newPos = player.position;
            //TODO: think about how rollover is handled
            if (kState.IsKeyDown(Keys.W))
                newPos.Y -= player.velocity.Y * (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (kState.IsKeyDown(Keys.S))
                newPos.Y += player.velocity.Y * (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (kState.IsKeyDown(Keys.A))
                newPos.X -= player.velocity.X * (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (kState.IsKeyDown(Keys.D))
                newPos.X += player.velocity.X * (float)gameTime.ElapsedGameTime.TotalSeconds;

            player.position = newPos;
            return player;
        }
    }
}
