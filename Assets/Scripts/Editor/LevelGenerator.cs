using System.IO;
using System.Linq;
using TheseusAndMinotaur.Data;
using TheseusAndMinotaur.Data.Deserializer;
using TheseusAndMinotaur.Data.Game;
using TheseusAndMinotaur.Data.Game.PathFinder;
using UnityEditor;
using UnityEngine;

namespace TheseusAndMinotaur.Editor
{
    public class LevelGenerator : ScriptableWizard
    {
        private const string FileName = "maze";
        private const string TargetLocation = "Generated";
        [SerializeField] private int resultFilesNumber = 20;

        [SerializeField] private int width = 5;
        [SerializeField] private int height = 5;
        [SerializeField] private int pathLenghtMin = 15;

        [SerializeField] private int verticalWallMin = 3;
        [SerializeField] private int verticalWallMax = 10;


        [SerializeField] private int horizontalWallMin = 3;
        [SerializeField] private int horizontalWallMax = 10;

        private void OnWizardCreate()
        {
            var currentAttemtp = 0;
            var successNumber = 0;
            while (currentAttemtp++ < 100000 && successNumber < resultFilesNumber)
            {
                var mazeConfig = BoardTextDeserializer.GenerateRandom(new Vector2Int(width, height),
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