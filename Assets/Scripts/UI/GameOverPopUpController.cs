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
        [SerializeField] private Button nextLevelButton;

        [SerializeField] private GameObject gameOverHeader;
        [SerializeField] private GameObject victoryHeader;
        
        
        private WorldGameController _worldGameController;
        private LevelManager _levelManager;

        private void Awake()
        {
            _levelManager = FindObjectOfType<LevelManager>();
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
                if (nextLevelButton.interactable && Input.GetButtonUp(nameof(InputAction.Next)))
                    StartNext();
            }       
        }

        private void StartNext()
        {
            _levelManager.StartNext();
        }

        private void GameStateChanged(GameState gameState)
        {
            canvas.enabled = gameState == GameState.GameOver || gameState == GameState.Victory;
            if (!canvas.enabled) return;
            gameOverHeader.SetActive(gameState == GameState.GameOver);
            victoryHeader.SetActive(gameState == GameState.Victory);
            nextLevelButton.interactable = _levelManager.HasMoreLevel;
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