using UnityEngine;

namespace Owahu.Tetris.Game
{
    public class Playfield : MonoBehaviour
    {
        public static int Wídth = 10;
        public static int Height = 20;
        public static Transform[,] Grid = new Transform[Wídth, Height];

        public static bool InsideGameBorder(Vector2 position)
        {
            return (int) position.x >= 0 &&
                   (int) position.x < Wídth &&
                   (int) position.y >= 0;
        }

        public static void DeleteRow(int rowIndex)
        {
            for (var column = 0; column < Wídth; ++column)
            {
                Destroy(Grid[column, rowIndex].gameObject);
                Grid[column, rowIndex] = null;
            }
        }

        /// <summary>
        /// Takes all blocks one above <paramref name="rowIndex"/> and reduces there y position
        /// </summary>
        /// <param name="rowIndex"></param>
        public static void MoveUpperRow(int rowIndex)
        {
            for (var x = 0; x < Wídth; ++x)
            {
                if (Grid[x, rowIndex] == null)
                {
                    continue;
                }

                // Move one towards bottom
                Grid[x, rowIndex - 1] = Grid[x, rowIndex];
                Grid[x, rowIndex] = null;

                // Update Block position
                Grid[x, rowIndex - 1].position += new Vector3(0, -1, 0);
            }
        }

        /// <summary>
        /// Decreases the y position for each block above <paramref name="rowIndex"/> 
        /// </summary>
        /// <param name="rowIndex"></param>
        public static void DecreaseRowsAbove(int rowIndex)
        {
            for (var i = rowIndex; i < Height; ++i)
                MoveUpperRow(i);
        }

        public static bool IsRowFull(int y)
        {
            for (var x = 0; x < Wídth; ++x)
            {
                if (Grid[x, y] == null)
                {
                    return false;
                }
            }

            return true;
        }

        public static void DeleteFullRows()
        {
            for (var y = 0; y < Height; ++y)
            {
                if (!IsRowFull(y))
                {
                    continue;
                }

                DeleteRow(y);
                DecreaseRowsAbove(y + 1);
                --y;
            }
        }
    }
}