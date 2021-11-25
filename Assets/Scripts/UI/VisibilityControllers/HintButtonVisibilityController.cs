using TheseusAndMinotaur.WorldControllers;
using UnityEngine;
using UnityEngine.UI;

namespace TheseusAndMinotaur.UI
{
    /// <summary>
    /// Make button available only when it's user input and hint is not yet shown on the screen
    /// </summary>
    public class HintButtonVisibilityController : MonoBehaviour
    {
        private Button _button;
        private WorldGameController _worldGameController;
        private HintController _hintController;
        private void Awake()
        {
            _button = GetComponent<Button>();
            _hintController = FindObjectOfType<HintController>();
            _worldGameController = FindObjectOfType<WorldGameController>();
            _worldGameController.GameStateChanged.AddListener(OnStateChanged);
        }

        private void OnStateChanged(GameState gameState)
        {
            if (gameState == GameState.ListenUserInput && !_hintController.IsHintActive)
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