using UnityEngine;

namespace Owahu.Tetris.Spawner
{
    public class Spawner : MonoBehaviour
    {
        // Groups
        public GameObject[] groups;

        // Start is called before the first frame update
        void Start()
        {
            SpawnNext();
        }

        public void SpawnNext()
        {
            // Random Index
            var i = Random.Range(0, groups.Length);

            // Spawn Group at current Position
            Instantiate(groups[i],
                transform.position,
                Quaternion.identity);
        }
    }
}