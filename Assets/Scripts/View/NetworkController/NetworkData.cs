using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UIElements;
namespace Network
{
    internal class NetworkData : MonoBehaviour
    {
        public static NetworkData Instance { get; private set; }
        public int UniqueID => Me.unique_id;
        public string PlayerName => Me.name;
        [field: SerializeField] public GameState CurrentGameState { get; set; }
        [field: SerializeField] public Player Me { get; set; }
        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;
        }

        [Serializable]
        public enum InGameID
        {
            Undecided,
            PlayerOne,
            PlayerTwo,
            PlayerThree,
            PlayerFour,
            PlayerFive,
            Orchestrator
        }
        [Serializable]
        public enum PlayerInputType
        {
            Movement
        }
        [Serializable]
        public class GameState
        {
            public int id;
            public string name;
            public List<Player> players;
            public bool is_lobby;
        }
        [Serializable]
        public class Player
        {
            public int? connected_game_id;
            public string in_game_id;
            public int unique_id;
            public string name;
            public Node? position;
        }
        [Serializable]
        public struct Node
        {
            public int id;
            public string name;
            public List<int> neighbours_id;
        }
        [Serializable]
        public struct NewGameInfo
        {
            public Player host;
            public string name;
        }
        [Serializable]
        public struct PlayerInput
        {
            public Player player;
            public PlayerInputType input_type;
            public Node related_node;
        }
    }
}
