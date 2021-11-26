using UnityEngine;

namespace TheseusAndMinotaur.Data
{
    /// <summary>
    ///     Metadata for the levels
    ///     Could contains some extra information for the level
    ///     Which can be e.g. presented on the screen
    /// </summary>
    [CreateAssetMenu]
    public class LevelMetadata : ScriptableObject
    {
        [Tooltip("text which will be used at the info box for level with the same name")]
        [Multiline(10)]
        [SerializeField]
        private string levelInfo;

        public string LevelInfo => levelInfo;
    }
}