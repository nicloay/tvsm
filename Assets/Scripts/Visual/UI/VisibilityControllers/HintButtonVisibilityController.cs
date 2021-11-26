using TheseusAndMinotaur.WorldControllers;
using UnityEngine;
using UnityEngine.UI;

namespace TheseusAndMinotaur.UI
{
    /// <summary>
    ///     Make button available only when it's user input and hint is not yet shown on the screen
    /// </summary>
    public class HintButtonVisibilityController : MonoBehaviour
    {
        private Button _button;
        private HintManager _hintManager;
        private WorldGameManager _worldGameManager;

        private void Awake()
        {
            _button = GetComponent<Button>();
            _hintManager = FindObjectOfType<HintManager>();
            _worldGameManager = FindObjectOfType<WorldGameManager>();
            _worldGameManager.GameStateChanged.AddListener(OnStateChanged);
        }

        private void OnStateChanged(GameState gameState)
        {
            if (gameState == GameState.ListenUserInput && !_hintManager.IsHintActive)
            {
                _button.interactable = true;
            }
            else
            {
                _button.interactable = false;
            }
        }
    }
}