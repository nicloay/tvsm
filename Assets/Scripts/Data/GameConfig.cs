using UnityEngine;
using UnityEngine.Assertions;

namespace TheseusAndMinotaur.Data
{
    /// <summary>
    ///     We don't have DI system yet, so let's use singleton
    /// </summary>
    [CreateAssetMenu]
    public class GameConfig : ScriptableObject
    {
        private const string ResourceName = "GameConfig";

        private static GameConfig _instance;

        [Tooltip("Distance between 2 cells (vertically and horizontally")] [SerializeField]
        private Vector2 cellStep = Vector2.one;

        [Tooltip("Seconds per cell speed for Theseus, Minotaur and Undo back movement")] [SerializeField]
        private float movementSpeed = 0.3f;
        

        public static GameConfig Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = Resources.Load<GameConfig>(ResourceName);
                    Assert.IsTrue(_instance.movementSpeed > 0,
                        $"GameConfig {nameof(movementSpeed)} must be greater than zero");
                }

                return _instance;
            }
        }

        public Vector2 CellStep => cellStep;

        public float MovementSpeed => movementSpeed;
    }
}