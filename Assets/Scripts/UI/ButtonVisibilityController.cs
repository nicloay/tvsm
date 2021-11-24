using TheseusAndMinotaur.Game;
using UnityEngine;
using UnityEngine.UI;

namespace TheseusAndMinotaur.UI
{
    /// <summary>
    /// Enable button only when GameState is in ListenUserInputState
    /// </summary>
    [RequireComponent(typeof(Button))]
    public class ButtonVisibilityController : MonoBehaviour
    {
        private const GameState AllowedState = GameState.ListenUserInput;

        private void Awake()
        {
            var button = GetComponent<Button>();
            FindObjectOfType<WorldGameController>().GameStateChanged
                .AddListener(state => button.interactable = state == AllowedState);
        }
    }
}