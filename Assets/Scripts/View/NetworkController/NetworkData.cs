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
        public int UniqueID => Me.Value.unique_id;
        public string PlayerName => Me.Value.name;
        public event Action<Player?> MeChanged;
        public Player? Me { get => me; set { me = value; MeChanged?.Invoke(me); } }
        private Player? me;
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
            StartGame,
            AssignSituationCard,
            LeaveGame, 
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
            Industrial,
            Normal, 
            Geolocation
        }
        [Serializable]
        public enum DistrictModifierType
        {
            Access,
            Priority,
            Toll
        }

        // Structs
        [Serializable]
        public struct LobbyList
        {
            public List<GameState> lobbies;
        }
        [Serializable]
        public struct GameState
        {
            public int id;
            public string name;
            public List<Player> players;
            public bool is_lobby;
            public string current_players_turn;
            public List<DistrictModifier> district_modifiers;
            public SituationCard? situation_card;
        }
        [Serializable]
        public struct Player
        {
            public int? connected_game_id;
            public string in_game_id;
            public int unique_id;
            public string name;
            public int? position_node_id;
            public int remaining_moves;
            public PlayerObjectiveCard? objective_card;
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
            public int player_id;
            public int game_id;
            public string input_type;
            public string related_role;
            public int? related_node_id;
            public DistrictModifier? district_modifier;
            public int? situation_card_id;
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
        [Serializable] 
        public struct SituationCard
        {
            public int card_id;
            public string title;
            public string description;
            public string goal;
            public List<CostTuple> costs;
        }
        [Serializable]
        public struct SituationCardList
        {
            public List<SituationCard> situation_cards;
        }
        [Serializable]
        public struct CostTuple
        {
            public string neighbourhood;
            public string traffic;
        }
        [Serializable]
        public enum Traffic
        {
            LevelOne,
            LevelTwo,
            LevelThree,
            LevelFour,
            LevelFive
        }
        [Serializable]
        public struct PlayerObjectiveCard
        {
            public int start_node_id;
            public int pick_up_node_id;
            public int drop_off_node_id;
            public List<string> special_vehicle_types;
            public bool picked_package_up;
            public bool dropped_package_off;
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
