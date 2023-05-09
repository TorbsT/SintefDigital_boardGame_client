using Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace View.Game
{
    /// <summary>
    /// Checks which train tracks are available,
    /// and displays a marker where the train is unavailable.
    /// Does not communicate with server, relatively hard-coded.
    /// </summary>
    internal class NoTrainSystem : MonoBehaviour
    {
        /// <summary>
        /// Describes an icon that will be shown on the given situation card ids
        /// (indicates it is disabled)
        /// </summary>
        [Serializable]
        private class IconSituationsPair
        {
            [field: SerializeField] public GameObject GO { get; private set; }
            [field: SerializeField] public List<int> SituationIds { get; private set; }
        }
        [SerializeField] private List<IconSituationsPair> pairs = new();

        private void OnEnable()
        {
            GameStateSynchronizer.Instance.StateChanged += StateChanged;
        }
        private void OnDisable()
        {
            GameStateSynchronizer.Instance.StateChanged -= StateChanged;
        }

        private void StateChanged(NetworkData.GameState? state)
        {
            if (!state.HasValue) return;
            if (!state.Value.situation_card.HasValue) return;
            NetworkData.SituationCard sitCard = state.Value.situation_card.Value;
            int id = sitCard.card_id;

            foreach (var pair in pairs)
            {
                bool show = pair.SituationIds.Contains(id);
                pair.GO.SetActive(show);
            }
        }
    }
}
