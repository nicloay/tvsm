using System.IO;
using System.Linq;
using TheseusAndMinotaur.Data;
using TheseusAndMinotaur.Data.Converter;
using TheseusAndMinotaur.Data.Game;
using TheseusAndMinotaur.Data.Game.PathFinder;
using UnityEditor;
using UnityEngine;

namespace TheseusAndMinotaur.Editor
{
    /// <summary>
    ///     Generate board based on parameters
    /// </summary>
    public class LevelGenerator : ScriptableWizard
    {
        private const string FileName = "maze";
        private const string TargetLocation = "Generated";
        [SerializeField] private int resultFilesNumber = 20;

        [SerializeField] private int gridWidth = 5;
        [SerializeField] private int heightHeight = 5;

        /// <summary>
        ///     When board generated, pathfinder try to find the path for theseus
        ///     Board will be considered as complete
        ///     if only
        ///     1. single path exists (not more than 1)
        ///     2. steps of Theseus is bigger than this value
        /// </summary>
        [SerializeField] private int pathLenghtMin = 15;

        /// <summary>
        ///     Generator generate value between <see cref="verticalWallMin" /> and <see cref="verticalWallMax" />
        ///     and put vertical walls to the grid
        /// </summary>
        [SerializeField] private int verticalWallMin = 3;

        [SerializeField] private int verticalWallMax = 10;

        /// <summary>
        ///     horizontal number on the grid <see cref="verticalWallMin" /> for more info
        /// </summary>
        [SerializeField] private int horizontalWallMin = 3;

        [SerializeField] private int horizontalWallMax = 10;

        private void OnWizardCreate()
        {
            var currentAttemtp = 0;
            var successNumber = 0;
            while (currentAttemtp++ < 100000 && successNumber < resultFilesNumber)
            {
                var mazeConfig = BoardConfig.GetRandom(new Vector2Int(gridWidth, heightHeight),
                    Random.Range(horizontalWallMin, horizontalWallMax), Random.Range(verticalWallMin, verticalWallMax));
                var gameLogic = new GameLogic(mazeConfig);
                var pathfinder = new PathFinder(gameLogic);
                var (pathFound, path) = pathfinder.FindPath();
                if (pathFound == PathFinder.Result.SinglePathFound && path.Count >= pathLenghtMin)
                {
                    Debug.Log("maze found");
                    SaveConfig(mazeConfig);

                    successNumber++;
                }
            }

            Debug.Log($"made 100000 iterations, found {successNumber} mazes");
        }


        [MenuItem("Window/Generate level")]
        private static void CreateWizard()
        {
            DisplayWizard<LevelGenerator>("Generate Level", "Generate");
        }

        private void SaveConfig(BoardConfig config)
        {
            if (!Directory.Exists(TargetLocation))
            {
                Directory.CreateDirectory(TargetLocation);
            }

            var existing = Directory.GetFiles(TargetLocation).Select(s => Path.GetFileNameWithoutExtension(s))
                .ToArray();

            var newName = ObjectNames.GetUniqueName(existing, FileName) + ".txt";
            var targetPath = Path.Combine(TargetLocation, newName);

            Debug.Log($"save boardConfig to the {targetPath}");

            File.WriteAllText(targetPath, config.ConvertToTextConfig());
        }
    }
}