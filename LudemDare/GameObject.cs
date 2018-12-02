using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

public struct GameObject
{
    public Vector2 position;
    public Vector2 velocity;
    public Texture2D texture;
    public System.Func<KeyboardState, GameTime, GameObject, GameObject> update;
    public System.Func<KeyboardState, GameTime, GameObject, GameObject?> addItem;
    public bool shouldDelete;
}
