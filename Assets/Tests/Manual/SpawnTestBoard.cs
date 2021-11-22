using TheseusAndMinotaur.Maze;
using UnityEngine;

namespace TheseusAndMinotaur.Tests
{
    [RequireComponent(typeof(BoardGenerator))]
    public class SpawnTestBoard : MonoBehaviour
    {
        private void Start()
        {
            var board = BoardDeserializer.DeserializeFromStreamingAssets("Test/test1.txt");
            GetComponent<BoardGenerator>().SpawnBoard(board);
        }
    }
}
