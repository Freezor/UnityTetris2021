using System.Linq;
using Owahu.Tetris.Game;
using UnityEngine;

namespace Owahu.Tetris.Blocks
{
    public class BlockManager : MonoBehaviour
    {
        private float _lastFall = 0;

        #region UnityLifecycle

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                HandleMoveLeft();
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                HandleMoveRight();
            }
            else if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                HandleRotation();
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow) ||
                     Time.time - _lastFall >= 1)
            {
                HandleFallDown();
            }
        }

        void Start()
        {
            // Default position not valid? Then it's game over
            if (IsValidGridPos())
            {
                return;
            }

            Debug.Log("GAME OVER");
            Destroy(gameObject);
        }

        #endregion

        private bool IsValidGridPos()
        {
            return transform.Cast<Transform>().All(BlockInsideBorders);
        }

        private bool BlockInsideBorders(Transform childBlock)
        {
            var childPosition = TransformationVector.RoundVector2(childBlock.position);

            // Not inside Border?
            if (!Playfield.InsideGameBorder(childPosition))
            {
                return false;
            }

            // Block in grid cell (and not part of same group)?
            return Playfield.Grid[(int) childPosition.x, (int) childPosition.y] == null ||
                   Playfield.Grid[(int) childPosition.x, (int) childPosition.y].parent == transform;
        }

        void UpdateGrid()
        {
            RemoveOldChildrenFromGrid();

            AddNewChildrenToGrid();
        }

        private void AddNewChildrenToGrid()
        {
            foreach (Transform child in transform)
            {
                var v = TransformationVector.RoundVector2(child.position);
                Playfield.Grid[(int) v.x, (int) v.y] = child;
            }
        }

        private void RemoveOldChildrenFromGrid()
        {
            for (var y = 0; y < Playfield.Height; ++y)
            {
                for (var x = 0; x < Playfield.Wídth; ++x)
                {
                    if (Playfield.Grid[x, y] == null)
                    {
                        continue;
                    }

                    if (Playfield.Grid[x, y].parent == transform)
                    {
                        Playfield.Grid[x, y] = null;
                    }
                }
            }
        }


        private void HandleFallDown()
        {
            // Modify position
            transform.position += new Vector3(0, -1, 0);

            // See if valid
            if (IsValidGridPos())
            {
                // It's valid. Update grid.
                UpdateGrid();
            }
            else
            {
                // It's not valid. revert.
                transform.position += new Vector3(0, 1, 0);

                // Clear filled horizontal lines
                Playfield.DeleteFullRows();

                // Spawn next Group
                FindObjectOfType<Spawner.Spawner>().SpawnNext();

                // Disable script
                enabled = false;
            }

            _lastFall = Time.time;
        }

        private void HandleRotation()
        {
            transform.Rotate(0, 0, -90);

            // See if valid
            if (IsValidGridPos())
                // It's valid. Update grid.
                UpdateGrid();
            else
                // It's not valid. revert.
                transform.Rotate(0, 0, 90);
        }

        private void HandleMoveLeft()
        {
            // Modify position
            ModifyPosition(new Vector3(-1, 0, 0));

            // See if valid
            if (IsValidGridPos())
                // It's valid. Update grid.
                UpdateGrid();
            else
                // It's not valid. revert.
                RevertPositionModification(new Vector3(+1, 0, 0));
        }

        private void HandleMoveRight()
        {
            ModifyPosition(new Vector3(1, 0, 0));

            // See if it's valid
            if (IsValidGridPos())
                // It's valid. Update grid.
                UpdateGrid();
            else
                RevertPositionModification(new Vector3(-1, 0, 0));
        }

        private void RevertPositionModification(Vector3 vector3)
        {
            transform.position += vector3;
        }

        private void ModifyPosition(Vector3 vector3)
        {
            transform.position += vector3;
        }
    }
}