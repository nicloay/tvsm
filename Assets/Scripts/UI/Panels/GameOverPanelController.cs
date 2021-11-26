using TheseusAndMinotaur.Game;
using TheseusAndMinotaur.WorldControllers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TheseusAndMinotaur.UI
{
    /// <summary>
    ///     This controller handle the end screen when player won or failed
    ///     If player won and there is next leve, the corresponding button will be shown.
    /// </summary>
    public class GameOverPanelController : MonoBehaviour
    {
        [SerializeField] private Canvas canvas;
        [SerializeField] private Button restartButton;
        [SerializeField] private Button undoButton;
        [SerializeField] private Button nextLevelButton;

        [SerializeField] private GameObject gameOverHeader;
        [SerializeField] private GameObject victoryHeader;
        private LevelManager _levelManager;


        private WorldGameController _worldGameController;

        private void Awake()
        {
            _levelManager = FindObjectOfType<LevelManager>();
            _worldGameController = FindObjectOfType<WorldGameController>();
            _worldGameController.GameStateChanged.AddListener(OnGameStateChanged);
            restartButton.onClick.AddListener(OnRestartBoard);
            undoButton.onClick.AddListener(OnUndoLastTurn);
            nextLevelButton.onClick.AddListener(StartNext);
        }

        private void Update()
        {
            if (canvas.enabled)
            {
                if (Input.GetButtonUp(nameof(InputAction.Restart)))
                {
                    OnRestartBoard();
                }

                if (Input.GetButtonUp(nameof(InputAction.Undo)))
                {
                    OnUndoLastTurn();
                }

                if (nextLevelButton.interactable && Input.GetButtonUp(nameof(InputAction.Next)))
                {
                    StartNext();
                }
            }
        }

        private void StartNext()
        {
            _levelManager.StartNext();
        }

        private void OnGameStateChanged(GameState gameState)
        {
            canvas.enabled = gameState == GameState.GameOver || gameState == GameState.Victory;
            if (!canvas.enabled)
            {
                return;
            }

            gameOverHeader.SetActive(gameState == GameState.GameOver);
            victoryHeader.SetActive(gameState == GameState.Victory);
            nextLevelButton.interactable = _levelManager.HasMoreLevel;
            if (gameState == GameState.Victory && !_levelManager.HasMoreLevel)
            {
                victoryHeader.GetComponentInChildren<TextMeshProUGUI>().text = "Congratulation you finished the game!";
            }
        }

        private void OnUndoLastTurn()
        {
            _worldGameController.RequestUndoOnFinishedGame();
        }

        private void OnRestartBoard()
        {
            _worldGameController.RestartBoard();
        }
    }
}