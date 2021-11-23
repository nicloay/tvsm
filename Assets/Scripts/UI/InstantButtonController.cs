using TheseusAndMinotaur.Game;
using UnityEngine;
using UnityEngine.UI;

namespace TheseusAndMinotaur.UI
{
    /// <summary>
    ///     Listen only single even like Click or Input.ButtonPressUp() and forward it to the GameManager
    /// </summary>
    public class InstantButtonController : MonoBehaviour
    {
        [SerializeField] private InputAction inputAction;
        private Button _button;

        private GameManager _gameManager;

        private void Awake()
        {
            _button = GetComponent<Button>();
            _button.onClick.AddListener(RequestAction);
            _gameManager = FindObjectOfType<GameManager>();
        }

        private void Update()
        {
            if (_button.isActiveAndEnabled && Input.GetButtonDown(inputAction.ToString())) RequestAction();
        }

        private void RequestAction()
        {
            _gameManager.RequestAction(inputAction);
        }
    }
}