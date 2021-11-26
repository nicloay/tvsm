using TheseusAndMinotaur.WorldControllers;
using UnityEngine;
using UnityEngine.UI;

namespace TheseusAndMinotaur.UI
{
    /// <summary>
    ///     Listen only single even like Click or Input.ButtonPressUp() and forward it to the GameManager
    /// </summary>
    public class PressButtonController : MonoBehaviour
    {
        [SerializeField] private InputAction inputAction;
        private Button _button;

        private WorldGameManager _worldGameManager;

        private void Awake()
        {
            _button = GetComponent<Button>();
            _button.onClick.AddListener(OnButtonClick);
            _worldGameManager = FindObjectOfType<WorldGameManager>();
        }

        private void Update()
        {
            if (_button.interactable && Input.GetButtonDown(inputAction.ToString()))
            {
                OnButtonClick();
            }
        }

        private void OnButtonClick()
        {
            _worldGameManager.RequestAction(inputAction);
        }
    }
}