using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace LudemDare
{
    public class EnemyFatory
    {
        private static Texture2D Texture;

        public EnemyFatory(Texture2D texture)
        {
            Texture = texture;
        }

        public GameObject createEnemy(Vector2 position)
        {
            return new GameObject
            {
                position = position,
                velocity = new Vector2(0, -100),
                texture = Texture,
                update = UpdateEnemy,
                addItem = AddFromEnemy,
                shouldDelete = false
            };
        }

        private static GameObject UpdateEnemy(KeyboardState kState, GameTime gameTime, GameObject projectile, GraphicsDeviceManager graphics)
        {
            return projectile;
        }

        private static GameObject? AddFromEnemy(KeyboardState kState, GameTime gameTime, GameObject projectile)
        {
            return null;
        }
    }
}
