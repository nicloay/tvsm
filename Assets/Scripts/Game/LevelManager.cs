using System;
using UnityEngine;
using UnityEngine.Assertions;

namespace TheseusAndMinotaur.Game
{
    /// <summary>
    /// contains list to the levels
    /// </summary>
    public class LevelManager : MonoBehaviour
    {

        [SerializeField] private string[] levels =
        {
            "Levels/level1.txt",
            "Levels/level2.txt",
            "Levels/level3.txt",
        };
        
        private int _currentLevel = 0;

        public bool HasMoreLevel => _currentLevel < (levels.Length - 1);
        
        private WorldGameController _worldGameController;

        private void Awake()
        {
            _worldGameController = FindObjectOfType<WorldGameController>();
        }

        void Start()
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