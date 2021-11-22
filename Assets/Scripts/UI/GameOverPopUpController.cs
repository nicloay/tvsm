using TheseusAndMinotaur.Game;
using UnityEngine;
using UnityEngine.UI;

namespace TheseusAndMinotaur.UI
{
    public class GameOverPopUpController : MonoBehaviour
    {
        [SerializeField] private GameManager gameManager;
        [SerializeField] private Canvas canvas;
        [SerializeField] private Button restartButton;
        [SerializeField] private Button undoButton;


        private void Awake()
        {
            gameManager.GameStateChanged.AddListener(GameStateChanged);
            restartButton.onClick.AddListener(RestartBoard);
            undoButton.onClick.AddListener(UndoLastTurn);
        }

        private void Update()
        {
            if (canvas.enabled)
                if (Input.GetButtonUp(nameof(InputAction.Restart)))
                    gameManager.RestartBoard();
        }

        private void GameStateChanged(GameState gameState)
        {
            canvas.enabled = gameState == GameState.GameOver;
        }

        private void UndoLastTurn()
        {
        }

        private void RestartBoard()
        {
            gameManager.RestartBoard();
        }
    }
}