using System.Collections.Generic;
using TheseusAndMinotaur.Data;
using UnityEngine;
using UnityEngine.Pool;

namespace TheseusAndMinotaur.Maze
{
    public class CellController : MonoBehaviour
    {
        [SerializeField] private Transform top;
        [SerializeField] private Transform left;

        private ObjectPool<GameObject> _wallPool;
        private readonly List<GameObject> _spawnedWalls = new();

        private Dictionary<Direction, Transform> _transformByDirection;

        /// <summary>
        /// We spawn walls only on these directions (if we will spawn on all possible, some walls will be spawned twice by 2 neighbour cells)
        /// </summary>
        private static readonly Direction[] SpawnDirections = new[] { Direction.Left, Direction.Top };
        
        /// <summary>
        /// Called once when instance of prefab is initialized;
        /// </summary>
        /// <param name="wallPool"></param>
        public void Initialize(ObjectPool<GameObject> wallPool)
        {
            _wallPool = wallPool;
            _transformByDirection = new Dictionary<Direction, Transform>()
            {
                { Direction.Left, left },
                { Direction.Top, top }
            };
        }

        /// <summary>
        /// Called on new cells to setup walls on required directions
        /// </summary>
        public void SetupWalls(Vector2 position, Direction direction)
        {
            transform.position = position;
            foreach (var target in direction.Filter(SpawnDirections)) 
            {
                var wall = _wallPool.Get();
                _spawnedWalls.Add(wall);
                wall.transform.SetParent(_transformByDirection[target], false);  
            }       
        }

        /// <summary>
        /// Called when you don't need this cell anymore
        /// </summary>
        public void Release()
        {
            _spawnedWalls.ForEach(wallObject => _wallPool.Release(wallObject));
            _spawnedWalls.Clear();
        }
    }
}