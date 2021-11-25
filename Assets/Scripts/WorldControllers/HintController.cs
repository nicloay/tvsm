using System.Collections.Generic;
using TheseusAndMinotaur.Data;
using UnityEngine;
using UnityEngine.Pool;

namespace TheseusAndMinotaur.WorldControllers
{
    public class HintController : MonoBehaviour
    {
        [SerializeField] private HintStepController stepPrefab;

        private ObjectPool<HintStepController> _stepPool;

        private readonly Stack<HintStepController> _activeHints = new ();
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

        public void OnShowHintRequested(Vector2Int startPosition, List<Direction> directions)
        {
            var currentPosition = startPosition;
            _activeHints.Clear();
            foreach (var direction in directions)
            {
                var instance = _stepPool.Get();
                instance.SetDirection(direction);
                instance.transform.position = currentPosition.GetWorldPosition();
                
                _activeHints.Push(instance);
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