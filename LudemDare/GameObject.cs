using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

public enum ObjectType
{
    Player,
    PlayerProjectile,
    Enemy
}

public struct GameObject
{
    public Vector2 position;
    public Vector2 velocity;
    public Texture2D texture;
    public System.Func<KeyboardState, GameTime, GameObject, GraphicsDeviceManager, GameObject> update;
    public System.Func<KeyboardState, GameTime, GameObject, GameObject?> addItem;
    public System.Func<GameObject, GameObject, GameObject> resolveCollision;
    public ObjectType objectType;
    public bool shouldDelete;
    public System.Collections.Generic.Dictionary<string, double> otherNums;
    public System.Collections.Generic.Dictionary<string, string> otherStrings;
}
