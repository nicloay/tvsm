using TheseusAndMinotaur.WorldControllers;
using UnityEngine;
using UnityEngine.Assertions;

namespace TheseusAndMinotaur.Game
{
    /// <summary>
    ///     contains list to the levels
    ///     Responsible to start next level when previous complete
    /// </summary>
    public class LevelManager : MonoBehaviour
    {
        [SerializeField] private string[] levels =
        {
            "Levels/level1.txt",
            "Levels/level2.txt",
            "Levels/level3.txt"
        };

        private int _nextLevel = 13;

        private WorldGameController _worldGameController;

        public bool HasMoreLevel => _nextLevel < levels.Length;

        private void Awake()
        {
            _worldGameController = FindObjectOfType<WorldGameController>();
        }

        private void Start()
        {
            StartNext();
        }

        public void StartNext()
        {
            Assert.IsTrue(HasMoreLevel);
            _worldGameController.OpenBoard(levels[_nextLevel++]);
        }
    }
}