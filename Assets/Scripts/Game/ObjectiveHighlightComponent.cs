using UnityEngine;
using Common;

namespace Game
{
    /// <summary>
    /// A player-owned gameobject with this attached will automatically
    /// expand and go to front for visibility (on given player turns)
    /// </summary>
    [RequireComponent(typeof(PlayerOwned), typeof(SpriteSorter))]
    internal class ObjectiveHighlightComponent : MonoBehaviour
    {
        public PlayerOwned PlayerOwned { get; private set; }
        public SpriteSorter SpriteSorter { get; private set; }

        private void OnEnable()
        {
            PlayerOwned = GetComponent<PlayerOwned>();
            SpriteSorter = GetComponent<SpriteSorter>();
            ObjectiveHighlightSystem.Instance.Track(this);
        }
        private void OnDisable()
        {
            ObjectiveHighlightSystem.Instance.Untrack(this);
        }
    }
}
