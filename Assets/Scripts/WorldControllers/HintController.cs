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
        private bool HintActive => _activeHints.Count > 0;
        
        private void Awake()
        {
            _stepPool = new ObjectPool<HintStepController>(
                () => Instantiate<HintStepController>(stepPrefab),
                o => o.gameObject.SetActive(true),
                o => o.gameObject.SetActive(false));
            FindObjectOfType<WorldGameController>().GameStateChanged.AddListener(OnGameStateChanged);
        }

        private void OnGameStateChanged(GameState gameState)
        {
            if (HintActive && gameState == GameState.HandleInput)
            {
                Clear();
            }   
        }

        public void Show(Vector2Int startPosition, List<Direction> directions)
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