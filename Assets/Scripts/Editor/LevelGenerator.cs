using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using TheseusAndMinotaur.Data;
using TheseusAndMinotaur.Data.Deserializer;
using TheseusAndMinotaur.Data.Game;
using TheseusAndMinotaur.Data.Game.PathFinder;
using UnityEditor;
using UnityEngine;

namespace TheseusAndMinotaur
{
    public class LevelGenerator : ScriptableWizard
    {
        [SerializeField] private int resultFilesNumber = 20;
        
        [SerializeField] private int width = 5;
        [SerializeField] private int height = 5;
        [SerializeField] private int pathLenghtMin = 15;

        [SerializeField] private int verticalWallMin = 3;
        [SerializeField] private int verticalWallMax = 10;
        
        
        [SerializeField] private int horizontalWallMin = 3;
        [SerializeField] private int horizontalWallMax = 10;

        
        [MenuItem("Window/Generate level")]
        static void CreateWizard()
        {
            ScriptableWizard.DisplayWizard<LevelGenerator>("Generate Level", "Generate");
        }

        void OnWizardCreate()
        {
            int currentAttemtp = 0;
            var successNumber = 0;
            while (currentAttemtp++ < 100000 && successNumber < resultFilesNumber)
            {
                var mazeConfig = BoardDeserializer.GenerateRandom(new Vector2Int(width, height),
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


        private const string fileName = "maze";
        private const string TargetLocation = "Generated";
        private void SaveConfig(BoardConfig config)
        {
            if (!Directory.Exists(TargetLocation))
            {
                Directory.CreateDirectory(TargetLocation);
            }

            var existing = Directory.GetFiles(TargetLocation).Select(s => Path.GetFileNameWithoutExtension(s)).ToArray();

            var newName = ObjectNames.GetUniqueName(existing, fileName)+".txt";
            var targetPath = Path.Combine(TargetLocation, newName);
            
            Debug.Log($"save boardConfig to the {targetPath}");

            var result = new StringBuilder();
            var top = new StringBuilder();
            var left = new StringBuilder();
            for (int y = config.Height -1 ; y >=0 ; y--)
            {
                top.Clear();
                left.Clear();
                for (int x = 0; x < config.Width; x++)
                {
                    var direction = config[y, x];
                    top.Append(".");
                    top.Append(direction.HasTop() ? "_" : " ");
                    left.Append(direction.HasLeft() ? "|" : " ");
                    left.Append(" ");
                }
                
                
                
                if (y == config.MinotaurStartPosition.y)
                {
                    left[1 + config.MinotaurStartPosition.x * 2] = 'M';
                }
                
                if (y == config.TheseusStartPosition.y)
                {
                    left[1 + config.TheseusStartPosition.x * 2] = 'T';
                }
                
                if (y == config.Exit.y)
                {
                    left[1 + config.Exit.x * 2] = 'E';
                }
                
                result.AppendLine(top.ToString());
                result.AppendLine(left.ToString());
            }
            File.WriteAllText(targetPath, result.ToString());
        }
        
    }
}
