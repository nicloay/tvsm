using TheseusAndMinotaur.Data;
using TheseusAndMinotaur.Game;
using UnityEngine;
using UnityEngine.Assertions;

namespace TheseusAndMinotaur.UI
{
    /// <summary>
    /// This camera works only for landscape mode with wide screen
    /// futureImplementation: consider screen width, and fit according to that 
    /// </summary>
    [RequireComponent(typeof(Camera))]
    public class CameraAligner : MonoBehaviour
    {
        private Camera _camera;
        [SerializeField] private GameManager gameManager;
        [Tooltip("If board size is 10f, and mutliplier 1.4 the result camera size is 14f")]
        [SerializeField] private float cameraSizeMultiplier = 1.4f;
        [Tooltip("If board size is 10f and offset 0.2f, camera vertical position will be 10/2+(10*0.2)")]
        [SerializeField] private float cameraVerticalExtraOffset = 0.2f;
        private void Awake()
        {
            _camera = GetComponent<Camera>();
            Assert.IsTrue(_camera.orthographic, "Camera must be orthographic");
            gameManager.GameStateChanged.AddListener(OnGameStateChanged);
        }

        private void OnGameStateChanged(GameState gameState)
        {
            if (gameState == GameState.NewGameStarted)
            {
                var aspect = gameManager.BoardWorldSize.y / 2f;
                var cameraPosition = gameManager.BoardWorldSize;

                transform.position = gameManager.BoardWorldSize / 2 +
                    Vector2.up * (aspect * cameraVerticalExtraOffset) - GameConfig.Instance.CellStep;
                _camera.orthographicSize = aspect * cameraSizeMultiplier;
            }
        }
    }
}