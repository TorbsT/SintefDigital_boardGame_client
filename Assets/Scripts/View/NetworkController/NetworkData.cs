using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Network
{
    internal class NetworkData : MonoBehaviour
    {
        public static NetworkData Instance { get; private set; }
        [field: SerializeField] public int UniqueID { get; set; }
        [field: SerializeField] public string Name { get; set; }
        [field: SerializeField] public GameState State { get; set; }

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;
        }

        [Serializable]
        internal class GameState
        {
            public int ppplllayerTurn;
            public List<Player> players;
            [Serializable]
            public class Player
            {
                public string displayName;
                public List<int> colour;
            }
        }

        internal struct PlayerInfoAndLobbyName
        {
            private struct PlayerInfoStruct
            {
                public int UniqueID;
                public string Name;
            }

            private PlayerInfoStruct PlayerInfo;
            private string LobbyName;

            public PlayerInfoAndLobbyName(int uniqueId, string name, string lobbyName)
            {
                this.PlayerInfo = new()
                {
                    UniqueID = uniqueId,
                    Name = name
                };
                LobbyName = lobbyName;
            }
        }
    }
}
