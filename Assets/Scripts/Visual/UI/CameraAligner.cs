using TheseusAndMinotaur.Data;
using TheseusAndMinotaur.WorldControllers;
using UnityEngine;

namespace TheseusAndMinotaur.UI
{
    /// <summary>
    ///     Set Camera X position so it looks to the center of the board, dont' touch any other values
    /// </summary>
    [RequireComponent(typeof(Camera))]
    public class CameraAligner : MonoBehaviour
    {
        private WorldGameManager _worldGameManager;

        private void Awake()
        {
            _worldGameManager = FindObjectOfType<WorldGameManager>();
            _worldGameManager.GameStateChanged.AddListener(OnGameStateChanged);
        }

        private void OnGameStateChanged(GameState gameState)
        {
            if (gameState == GameState.NewGameStarted)
            {
                var xOffset = (_worldGameManager.BoardWorldSize.x - GameConfig.Instance.CellStep.x) / 2f;
                var position = transform.position;
                position.x = xOffset;
                // ReSharper disable once Unity.InefficientPropertyAccess
                transform.position = position;
            }
        }
    }
}