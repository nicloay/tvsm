using UnityEngine;
using UnityEngine.Assertions;

namespace TheseusAndMinotaur.Game
{
    /// <summary>
    ///     contains list to the levels
    /// </summary>
    public class LevelManager : MonoBehaviour
    {
        [SerializeField] private string[] levels =
        {
            "Levels/level1.txt",
            "Levels/level2.txt",
            "Levels/level3.txt"
        };

        private int _currentLevel;

        private WorldGameController _worldGameController;

        public bool HasMoreLevel => _currentLevel < levels.Length - 1;

        private void Awake()
        {
            _worldGameController = FindObjectOfType<WorldGameController>();
        }

        private void Start()
        {
            _worldGameController.OpenBoard(levels[0]);
        }

        public void StartNext()
        {
            Assert.IsTrue(HasMoreLevel);
            _worldGameController.OpenBoard(levels[++_currentLevel]);
        }
    }
}