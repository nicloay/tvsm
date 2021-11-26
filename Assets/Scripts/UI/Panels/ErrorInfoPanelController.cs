using System.Collections;
using TheseusAndMinotaur.WorldControllers;
using TMPro;
using UnityEngine;

namespace TheseusAndMinotaur.UI
{
    public class ErrorInfoPanelController : MonoBehaviour
    {
        [SerializeField] private float timeOnScreen = 1.0f;
        [SerializeField] private GameObject textHolder;
        [SerializeField] private string wrongMovementText = "ERROR: You can't move there";
        [SerializeField] private string pathNotFound = "ERROR: path not found";


        private TextMeshProUGUI _textMeshPro;

        private void Awake()
        {
            _textMeshPro = GetComponentInChildren<TextMeshProUGUI>();
            textHolder.SetActive(false);
            var gameController = FindObjectOfType<WorldGameController>();
            gameController.WrongMovement.AddListener(() => ShowError(wrongMovementText));
            gameController.PathNotFound.AddListener(() => ShowError(pathNotFound));
            gameController.LoadLevelCommunicationProblem.AddListener(ShowError);
        }

        private void ShowError(string text)
        {
            StopAllCoroutines();
            StartCoroutine(ShowTextBox(text));
        }

        private IEnumerator ShowTextBox(string text)
        {
            _textMeshPro.text = text;
            textHolder.SetActive(true);
            yield return new WaitForSeconds(timeOnScreen); // don't cache to adjust values in runtime
            textHolder.SetActive(false);
        }
    }
}