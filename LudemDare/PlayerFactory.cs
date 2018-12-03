using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace LudemDare
{
    public class PlayerFactory
    {
        public static GameObject CreatePlayer(GraphicsDeviceManager graphics){
            var player = new GameObject
            {
                position = new Vector2(
                graphics.PreferredBackBufferWidth / 2,
                graphics.PreferredBackBufferHeight / 2),

                velocity = new Vector2(200, 200),
                update = UpdatePlayer,
                shouldDelete = false
            };

            player.otherNums = new System.Collections.Generic.Dictionary<string, double>();
            player.otherStrings = new System.Collections.Generic.Dictionary<string, string>();
            player.otherNums["ATTACK_SPEED_SECONDS"] = .5;
            player.otherNums["lastAttackTime"] = 0;
            player.otherStrings["attackDirection"] = "";
            return player;
        }

        private struct AttackDirAndTime {
            public string direction;
            public double timeInMS;
        }

        private static AttackDirAndTime getAttackDirectionAndTime(GameTime gameTime, KeyboardState kState, double lastAttackTime, double attackSpeed){
            var direction = "";
            var time = lastAttackTime;

            var timeSinceLastAttack = gameTime.TotalGameTime.TotalMilliseconds - lastAttackTime;
            if (timeSinceLastAttack >= attackSpeed * 1000)
            {
                if (kState.IsKeyDown(Keys.Up))
                {
                    direction = "up";
                    time = gameTime.TotalGameTime.TotalMilliseconds;
                }
                else if (kState.IsKeyDown(Keys.Down))
                {
                    direction = "down";
                    time = gameTime.TotalGameTime.TotalMilliseconds;
                }
                else if (kState.IsKeyDown(Keys.Left))
                {
                    direction = "left";
                    time = gameTime.TotalGameTime.TotalMilliseconds;
                }
                else if (kState.IsKeyDown(Keys.Right))
                {
                    direction = "right";
                    time = gameTime.TotalGameTime.TotalMilliseconds;
                }
                else
                    direction = "";
            }
            else
                direction = "";

            return new AttackDirAndTime
            {
                direction = direction,
                timeInMS = time
            };
        }

        private static Vector2 getNewPosition(KeyboardState kState, GameTime gameTime, GameObject player){
            //TODO: think about how rollover is handled
            var newPos = player.position;
            if (kState.IsKeyDown(Keys.W))
                newPos.Y -= player.velocity.Y * (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (kState.IsKeyDown(Keys.S))
                newPos.Y += player.velocity.Y * (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (kState.IsKeyDown(Keys.A))
                newPos.X -= player.velocity.X * (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (kState.IsKeyDown(Keys.D))
                newPos.X += player.velocity.X * (float)gameTime.ElapsedGameTime.TotalSeconds;

            return newPos;
        }

        private static GameObject UpdatePlayer(KeyboardState kState, GameTime gameTime, GameObject player, GraphicsDeviceManager graphics)
        {
            var newStrings = player.otherStrings;
            var newNums = player.otherNums;

            var timeAndDir = getAttackDirectionAndTime(gameTime, kState, newNums["lastAttackTime"], newNums["ATTACK_SPEED_SECONDS"]);
            newNums["lastAttackTime"] = timeAndDir.timeInMS;
            newStrings["attackDirection"] = timeAndDir.direction;


            player.position = getNewPosition(kState, gameTime, player);
            player.otherStrings = newStrings;
            player.otherNums = newNums;
            return player;
        }
    }
}
