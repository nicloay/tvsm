using TheseusAndMinotaur.Data.Deserializer;
using TheseusAndMinotaur.Maze;
using UnityEngine;

namespace TheseusAndMinotaur.Tests
{
    [RequireComponent(typeof(BoardGenerator))]
    public class SpawnTestBoard : MonoBehaviour
    {
        private bool _spawnFirst = true;
        private BoardGenerator _boardGenerator;

        private void Awake()
        {
            _boardGenerator = GetComponent<BoardGenerator>();
        }

        private void Start()
        {
            Spawn();
        }

        private void Spawn()
        {
            _boardGenerator.Clear();
            var relativePath = $"Test/test{(_spawnFirst ? 1 : 2)}.txt";
            var board = BoardDeserializer.DeserializeFromStreamingAssets(relativePath);
            _boardGenerator.SpawnBoard(board);
            _spawnFirst = !_spawnFirst;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Spawn();
            }
        }
    }
}