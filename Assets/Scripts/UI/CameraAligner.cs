using TheseusAndMinotaur.Data;
using TheseusAndMinotaur.WorldControllers;
using UnityEngine;
using UnityEngine.Assertions;

namespace TheseusAndMinotaur.UI
{
    /// <summary>
    ///     This camera works only for landscape mode with wide screen
    ///     futureImplementation: consider screen width, and fit according to that
    /// </summary>
    [RequireComponent(typeof(Camera))]
    public class CameraAligner : MonoBehaviour
    {
        [Tooltip("If board world size is 10f, and mutliplier 1.4 the result camera size is 14f")] [SerializeField]
        private float cameraSizeMultiplier = 1.4f;

        [Tooltip("If board world size is 10f and offset 0.2f, camera vertical position will be 10/2+(10*0.2)")]
        [SerializeField]
        private float cameraVerticalExtraOffset = 0.2f;

        private Camera _camera;

        private WorldGameController _worldGameController;

        private void Awake()
        {
            _worldGameController = FindObjectOfType<WorldGameController>();
            _camera = GetComponent<Camera>();
            Assert.IsTrue(_camera.orthographic, "Camera must be orthographic");
            _worldGameController.GameStateChanged.AddListener(OnGameStateChanged);
        }

        private void OnGameStateChanged(GameState gameState)
        {
            if (gameState == GameState.NewGameStarted)
            {
                var aspect = _worldGameController.BoardWorldSize.y / 2f;
                var cameraPosition = _worldGameController.BoardWorldSize;

                transform.position = _worldGameController.BoardWorldSize / 2 +
                    Vector2.up * (aspect * cameraVerticalExtraOffset) - GameConfig.Instance.CellStep;
                _camera.orthographicSize = aspect * cameraSizeMultiplier;
            }
        }
    }
}