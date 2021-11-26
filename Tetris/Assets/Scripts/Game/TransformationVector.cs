using UnityEngine;

namespace Owahu.Tetris.Game
{
    public class TransformationVector
    {
        public static Vector2 RoundVector2(Vector2 v)
        {
            return new Vector2(Mathf.Round(v.x),
                Mathf.Round(v.y));
        }
    }
}