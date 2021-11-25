using System.Collections.Generic;
using TheseusAndMinotaur.Data;
using TMPro;
using UnityEngine;

namespace TheseusAndMinotaur.WorldControllers
{
    /// <summary>
    ///     Controll visual element of the single step hint path
    /// </summary>
    [RequireComponent(typeof(TextMeshPro))]
    public class HintStepController : MonoBehaviour
    {
        private static readonly Dictionary<Direction, string> signByDirection = new()
        {
            { Direction.Left, "←" },
            { Direction.Right, "→" },
            { Direction.Up, "↑" },
            { Direction.Down, "↓" },
            { Direction.None, "]" }
        };

        private static readonly Dictionary<Direction, Quaternion> RotationByDirection = new()
        {
            { Direction.Left, Quaternion.identity },
            { Direction.Right, Quaternion.identity },
            { Direction.Up, Quaternion.identity },
            { Direction.Down, Quaternion.identity },
            { Direction.None, Quaternion.Euler(0, 0, -90) }
        };

        private TextMeshPro _text;

        private void Awake()
        {
            _text = GetComponent<TextMeshPro>();
        }

        public void SetDirection(Direction direction)
        {
            _text.text = signByDirection[direction];
            _text.transform.rotation = RotationByDirection[direction];
        }
    }
}