using TheseusAndMinotaur.Data.Deserializer;
using TheseusAndMinotaur.WorldControllers.Maze;
using UnityEngine;

namespace TheseusAndMinotaur.Tests
{
    // Test spawning new board (just grid, not the whole game)
    [RequireComponent(typeof(BoardGridSpawner))]
    public class SpawnTestBoard : MonoBehaviour
    {
        private BoardGridSpawner _boardGridSpawner;
        private bool _spawnFirst = true;

        private void Awake()
        {
            _boardGridSpawner = GetComponent<BoardGridSpawner>();
        }

        private void Start()
        {
            Spawn();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Spawn();
            }
        }

        private void Spawn()
        {
            var relativePath = $"Test/test{(_spawnFirst ? 1 : 2)}.txt";
            var board = BoardTextDeserializer.DeserializeFromStreamingAssets(relativePath);
            _boardGridSpawner.SpawnBoard(board);
            _spawnFirst = !_spawnFirst;
        }
    }
}