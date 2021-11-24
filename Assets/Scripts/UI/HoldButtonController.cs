using TheseusAndMinotaur.Game;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace TheseusAndMinotaur.UI
{
    /// <summary>
    ///     Listen Button Hold state,
    ///     On Hold - send corresponding Input.ButtonPress action
    /// </summary>
    [RequireComponent(typeof(Button))]
    public class HoldButtonController : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        [Tooltip("Action which will be requested on " + nameof(WorldGameController))] [SerializeField]
        private InputAction action;

        private Button _button;

        private WorldGameController _worldGameController;

        public bool IsPressed { get; private set; }

        private void Awake()
        {
            _worldGameController = FindObjectOfType<WorldGameController>();
            _button = GetComponent<Button>();
        }

        private void Update()
        {
            if (_button.interactable && (IsPressed || Input.GetButton(action.ToString())))
                _worldGameController.RequestAction(action);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            IsPressed = true;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            IsPressed = false;
        }
    }
}