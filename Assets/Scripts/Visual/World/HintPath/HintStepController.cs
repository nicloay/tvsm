using System.Collections.Generic;
using TheseusAndMinotaur.Data;
using TMPro;
using UnityEngine;

namespace TheseusAndMinotaur.WorldControllers
{
    /// <summary>
    ///     Control visual element of the single step at the hint path
    /// </summary>
    [RequireComponent(typeof(TextMeshPro))]
    public class HintStepController : MonoBehaviour
    {
        private static readonly Dictionary<Direction, string> IconSignByDirection = new()
        {
            { Direction.Left, "←" },
            { Direction.Right, "→" },
            { Direction.Up, "↑" },
            { Direction.Down, "↓" },
            { Direction.None, "…" }
        };

        private TextMeshPro _text;

        private void Awake()
        {
            _text = GetComponent<TextMeshPro>();
        }

        public void SetDirection(Direction direction)
        {
            _text.text = IconSignByDirection[direction];
        }
    }
}