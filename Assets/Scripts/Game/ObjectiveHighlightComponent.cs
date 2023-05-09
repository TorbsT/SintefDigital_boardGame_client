using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Common;

namespace Game
{
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
