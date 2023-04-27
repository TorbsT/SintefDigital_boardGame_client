using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UIElements;
namespace Network
{
    public class NetworkData : MonoBehaviour
    {
        public static NetworkData Instance { get; private set; }
        public int UniqueID => Me.unique_id;
        public string PlayerName => Me.name;
        public event Action<Player> MeChanged;
        public Player Me { get => me; set { me = value; MeChanged?.Invoke(me); } }
        private Player me;
        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;
        }
        [Serializable]
        public enum InGameID
        {
            Undecided = 0,
            PlayerOne = 1,
            PlayerTwo = 2,
            PlayerThree = 3,
            PlayerFour = 4,
            PlayerFive = 5,
            Orchestrator = 6
        }
        [Serializable]
        public enum PlayerInputType
        {
            Movement,
            ChangeRole,
            All, // Do not use?
            NextTurn,
            UndoAction,
            ModifyDistrict,
            StartGame
        }
        [Serializable]
        public enum District
        {
            IndustryPark,
            Port,
            Suburbs,
            RingRoad,
            CityCentre,
            Airport
        }
        [Serializable]
        public enum VehicleType
        {
            Electric,
            Buss,
            Emergency,
            Industrial
        }
        [Serializable]
        public enum DistrictModifierType
        {
            Access,
            Priority,
            Toll
        }

        // Structs/classes >:)
        [Serializable]
        public class LobbyList
        {
            public List<GameState> lobbies;
        }
        [Serializable]
        public class GameState
        {
            public int id;
            public string name;
            public List<Player> players = new();
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
            public int remaining_moves;
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
        public struct GameStartInput
        {
            public int player_id;
            public string in_game_id;
            public int game_id;
        }
        [Serializable]
        public struct PlayerInput
        {
            public int player_id;
            public int game_id;
            public string input_type;
            public string related_role;
            public int? related_node_id;
            public DistrictModifier? district_modifier;
        }
        [Serializable]
        public struct DistrictModifier
        {
            public string district;
            public string modifier;
            public string vehicle_type;
            public int? associated_movement_value;
            public int? associated_money_value;
            public bool delete;
        }

        // Not on server
        [Serializable]
        public struct SituationCardList
        {
            public List<SituationCard> situation_cards;
        }
        [Serializable] 
        public struct SituationCard
        {
            public int card_id;
            public string title;
            public string description;
            public string goal;
            public List<(District, int)> costs;
        }

        public InGameID GetFirstAvailableRole(GameState state, bool skipOrchestrator)
        {  // Find a more appropriate location for this method
            List<InGameID> roles =
                new()
                {
                    InGameID.PlayerOne,
                    InGameID.PlayerTwo,
                    InGameID.PlayerThree,
                    InGameID.PlayerFour,
                    InGameID.PlayerFive,
                };
            if (!skipOrchestrator)
                roles.Insert(0, InGameID.Orchestrator);
            foreach (var player in state.players)
            {
                roles.Remove((InGameID)Enum.Parse(typeof(InGameID), player.in_game_id));
            }
            return roles[0];
        }
    }
}
