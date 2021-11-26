using System;
using System.Collections;
using TheseusAndMinotaur.Data;
using TheseusAndMinotaur.WorldControllers;
using TMPro;
using UnityEngine;

namespace TheseusAndMinotaur.UI
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class InfoBoxTextController : MonoBehaviour
    {
        private TextMeshProUGUI _textMeshProUGUI;

        private void Awake()
        {
            _textMeshProUGUI = GetComponent<TextMeshProUGUI>();
            FindObjectOfType<WorldGameController>().LevelStart.AddListener(OnLevelStarted);
        }

        private void OnLevelStarted(string levelName)
        {
            StopAllCoroutines();
            var meta = Resources.Load<LevelMetadata>(levelName);
            if (meta != null)
            {
                _textMeshProUGUI.text = meta.LevelInfo;
            }
            else
            {
                StartCoroutine(ShowUpdate());
            }
        }

        private IEnumerator ShowUpdate()
        {
            var startTime = Time.timeSinceLevelLoad;
            while (true)
            {
                _textMeshProUGUI.text =
                    "Time: " + TimeSpan.FromSeconds(Time.timeSinceLevelLoad - startTime).ToString(@"mm\:ss");
                yield return new WaitForSeconds(1f);
            }
            // ReSharper disable once IteratorNeverReturns
        }
    }
}