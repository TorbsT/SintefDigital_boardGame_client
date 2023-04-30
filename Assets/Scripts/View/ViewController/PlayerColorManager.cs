using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Network;

namespace View
{
    internal class PlayerColorManager : MonoBehaviour
    {
        [Serializable]
        private class PlayerColorPair
        {
            [field: SerializeField] public NetworkData.InGameID Player { get; set; }
            [field: SerializeField] public Color Color { get; set; } = Color.white;
        }

        public static PlayerColorManager Instance { get; private set; }

        [SerializeField] private List<PlayerColorPair> colors = new();

        public Color GetColor(NetworkData.InGameID player)
        {
            return colors.Find(match => match.Player == player).Color;
        }
        public void Refresh(PlayerOwned playerOwned)
        {
            Color color = GetColor(playerOwned.Owner);
            playerOwned.SetColor(color);
        }
        private void Awake()
        {
            Instance = this;
        }
        private void OnEnable()
        {
            foreach (PlayerOwned owned in FindObjectsOfType<PlayerOwned>())
                Refresh(owned);
        }
        private void OnValidate()
        {
            foreach (PlayerOwned owned in FindObjectsOfType<PlayerOwned>())
                Refresh(owned);
        }
    }
}
