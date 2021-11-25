using UnityEngine;

namespace TheseusAndMinotaur.Data
{
    /// <summary>
    ///     Metadata for the levels
    ///     When <see cref="WorldGameController" /> start level, we define extra details by this config
    ///     like a text
    ///     Use the same name as the level config (txt file with board info)
    ///     You can use rich text for the field
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