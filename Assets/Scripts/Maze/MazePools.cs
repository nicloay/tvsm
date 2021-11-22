using System.Collections.Generic;
using TheseusAndMinotaur.Data;
using UnityEngine;
using UnityEngine.Pool;

namespace TheseusAndMinotaur.Maze
{
    /// <summary>
    ///     Contains pools for walls and cells
    /// </summary>
    public class MazePools : MonoBehaviour
    {
        [SerializeField] private GameObject wallPrefab;
        [SerializeField] private CellController cellPrefab;

        private readonly List<CellController> _spawnedCells = new();
        private ObjectPool<CellController> _cellPool;

        private ObjectPool<GameObject> _wallsPool;

        private void Awake()
        {
            _wallsPool = new ObjectPool<GameObject>(
                () => Instantiate(wallPrefab),
                o => o.SetActive(true),
                o => o.SetActive(false));
            _cellPool = new ObjectPool<CellController>(
                () =>
                {
                    var instance = Instantiate(cellPrefab);
                    instance.Initialize(_wallsPool);
                    return instance;
                },
                controller => controller.gameObject.SetActive(true),
                controller =>
                {
                    controller.Release();
                    controller.gameObject.SetActive(false);
                });
        }

        public void AddCell(Vector2 position, Direction direction)
        {
            var cell = _cellPool.Get();
            cell.SetupWalls(position,
                direction); // TODO: find better place to calculate position (Vector2Int to Vector3)
            _spawnedCells.Add(cell);
        }

        /// <summary>
        ///     Return all cells back to object pools and hide
        /// </summary>
        public void Clear()
        {
            _spawnedCells.ForEach(cellController => _cellPool.Release(cellController));
            _spawnedCells.Clear();
        }
    }
}