using TheseusAndMinotaur.Game;
using UnityEngine;
using UnityEngine.UI;

namespace TheseusAndMinotaur.UI
{
    [RequireComponent(typeof(Button))]
    public class InteractableUIElementController : MonoBehaviour
    {
        [Tooltip("button will be interactable only when the game in this state")] [SerializeField]
        private GameState allowedState = GameState.ListenUserInput;

        private void Awake()
        {
            var button = GetComponent<Button>();
            FindObjectOfType<GameManager>().GameStateChanged
                .AddListener(state => button.interactable = state == allowedState);
        }
    }
}