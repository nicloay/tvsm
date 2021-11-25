using System.Collections.Generic;
using TheseusAndMinotaur.Data;
using UnityEngine;
using UnityEngine.Pool;

namespace TheseusAndMinotaur.WorldControllers
{
    public class HintController : MonoBehaviour
    {
        [Tooltip("Hint Arrays start at this z position")] [SerializeField]
        private float startZValue = -0.5f;

        [Tooltip(
            "if path cross the same field several time, each time z position will be increased, and next arrows will have the same level")]
        [SerializeField]
        private float zStep = -0.2f;

        [SerializeField] private HintStepController stepPrefab;

        private readonly Stack<HintStepController> _activeHints = new();

        private ObjectPool<HintStepController> _stepPool;
        public bool IsHintActive => _activeHints.Count > 0;

        private void Awake()
        {
            _stepPool = new ObjectPool<HintStepController>(
                () => Instantiate(stepPrefab),
                o => o.gameObject.SetActive(true),
                o => o.gameObject.SetActive(false), defaultCapacity: 30);
            var worldGameController = FindObjectOfType<WorldGameController>();
            worldGameController.GameStateChanged.AddListener(OnGameStateChanged);
            worldGameController.ShowHint.AddListener(OnShowHintRequested);
        }

        private void OnGameStateChanged(GameState gameState)
        {
            if (IsHintActive && gameState is GameState.HandleInput or GameState.NewGameStarted)
            {
                Clear();
            }
        }

        private Vector3 GetWorldPositionWithLevel(Vector2Int boardPosition, int level)
        {
            return boardPosition.GetWorldPosition() + new Vector3(0, 0, startZValue + level * zStep);
        }

        private void OnShowHintRequested(Vector2Int startPosition, List<Direction> directions)
        {
            var currentPosition = startPosition;
            _activeHints.Clear();
            var usedPosition = new HashSet<Vector2Int>();

            var currentLevel = 0;
            foreach (var direction in directions)
            {
                var instance = _stepPool.Get();
                instance.SetDirection(direction);
                if (usedPosition.Contains(currentPosition))
                {
                    currentLevel++;
                }

                instance.transform.position = GetWorldPositionWithLevel(currentPosition, currentLevel);

                _activeHints.Push(instance);

                usedPosition.Add(currentPosition);
                currentPosition = currentPosition.GetNeighbour(direction);
            }
        }

        private void Clear()
        {
            while (_activeHints.Count > 0)
            {
                _stepPool.Release(_activeHints.Pop());
            }
        }
    }
}