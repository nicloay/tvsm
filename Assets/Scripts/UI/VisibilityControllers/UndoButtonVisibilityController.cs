using TheseusAndMinotaur.WorldControllers;
using UnityEngine;
using UnityEngine.UI;

namespace TheseusAndMinotaur.UI
{
    /// <summary>
    ///     Make button interactable only when game is in ListenUserInput state and has undo layer
    /// </summary>
    [RequireComponent(typeof(Button))]
    public class UndoButtonVisibilityController : MonoBehaviour
    {
        private const GameState AllowedState = GameState.ListenUserInput;

        private void Awake()
        {
            var gameManager = FindObjectOfType<WorldGameController>();
            var button = GetComponent<Button>();
            FindObjectOfType<WorldGameController>().GameStateChanged
                .AddListener(state => button.interactable = state == AllowedState && gameManager.HasUndo);
        }
    }
}