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

        private WorldGameController _worldGameController;

        private void Awake()
        {
            _button = GetComponent<Button>();
            _button.onClick.AddListener(RequestAction);
            _worldGameController = FindObjectOfType<WorldGameController>();
        }

        private void Update()
        {
            if (_button.isActiveAndEnabled && Input.GetButtonDown(inputAction.ToString())) RequestAction();
        }

        private void RequestAction()
        {
            _worldGameController.RequestAction(inputAction);
        }
    }
}