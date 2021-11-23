using TheseusAndMinotaur.Game;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace TheseusAndMinotaur.UI
{
    /// <summary>
    ///     Listen Button click with corresponding Input.ButtonPress action
    /// </summary>
    [RequireComponent(typeof(Button))]
    public class DirectionButtonController : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        [Tooltip("Action which will be requested on " + nameof(GameManager))] [SerializeField]
        private InputAction action;

        private Button _button;

        private GameManager _gameManager;

        public bool IsPressed { get; private set; }

        private void Awake()
        {
            _gameManager = FindObjectOfType<GameManager>();
            _button = GetComponent<Button>();
        }

        private void Update()
        {
            if (_button.interactable && (IsPressed || Input.GetButton(action.ToString())))
                _gameManager.RequestAction(action);
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