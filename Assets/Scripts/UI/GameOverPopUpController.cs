using TheseusAndMinotaur.Game;
using UnityEngine;
using UnityEngine.UI;

namespace TheseusAndMinotaur.UI
{
    public class GameOverPopUpController : MonoBehaviour
    {
        [SerializeField] private Canvas canvas;
        [SerializeField] private Button restartButton;
        [SerializeField] private Button undoButton;
        private WorldGameController _worldGameController;


        private void Awake()
        {
            _worldGameController = FindObjectOfType<WorldGameController>();
            _worldGameController.GameStateChanged.AddListener(GameStateChanged);
            restartButton.onClick.AddListener(RestartBoard);
            undoButton.onClick.AddListener(UndoLastTurn);
        }

        private void Update()
        {
            if (canvas.enabled)
            {
                if (Input.GetButtonUp(nameof(InputAction.Restart)))
                    RestartBoard();
                if (Input.GetButtonUp(nameof(InputAction.Undo)))
                    UndoLastTurn();
            }
        }

        private void GameStateChanged(GameState gameState)
        {
            canvas.enabled = gameState == GameState.GameOver;
        }

        private void UndoLastTurn()
        {
            _worldGameController.RequestUndoOnFinishedGame();
        }

        private void RestartBoard()
        {
            _worldGameController.RestartBoard();
        }
    }
}