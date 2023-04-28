using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Network;
using TMPro;
using UnityEngine.UI;

namespace View
{
    public class GameCardController : MonoBehaviour
    {
        public static GameCardController Instance { get; private set; }

        [field: SerializeField, Range(1, 10)] public int MaxCards { get; private set; } = 1;
        [SerializeField] private TextMeshProUGUI explanationText;
        [SerializeField] private Button confirmButton;
        public Dictionary<int, GameCard> gamecards = new();
        public Point resetPoint;
        public Point spawnPoint;
        private GameCard currentGameCard;
        private List<GameCard> chosen = new();
        private void Awake()
        {
            Instance = this;
            confirmButton.interactable = false;
        }
        private void OnEnable()
        {
            foreach (GameCard card in GetComponentsInChildren<GameCard>())
                if (card != null)
                    gamecards.Add(card.id, card);
            foreach (GameCard card in gamecards.Values)
                card.gameObject.SetActive(false);
            RestAPI.Instance.GetSituationCards(
                (success) =>
                {
                    foreach (NetworkData.SituationCard card in success.situation_cards)
                    {
                        // Organize data from backend
                        int id = card.card_id;
                        if (!gamecards.ContainsKey(id))
                        {
                            Debug.LogWarning
                            ($"CardScene does not have a card with the cardId {id}. Please add one");
                            continue;
                        }
                        string title = card.title;
                        string description = card.description;
                        string goal = card.goal;
                        List<string> trafficList = new();
                        if (card.costs != null)
                        foreach (var traffic in card.costs)
                        {
                            trafficList.Add($"{traffic.Item1}: {traffic.Item2}");
                        }
                        string traffics = string.Join("\n", trafficList);

                        // Write data to card
                        GameCard gamecard = gamecards[id];
                        gamecard.Source = card;
                        gamecard.Id.text = $"!{id}";
                        gamecard.Title.text = title;
                        gamecard.Description.text = description;
                        gamecard.Goal.text = goal;
                        gamecard.Traffic.text = traffics;
                        gamecard.gameObject.SetActive(true);
                    }
                },
                (failure) => { }
            );

            string orchestratorName = GameStateSynchronizer.Instance.Orchestrator.name;
            if (GameStateSynchronizer.Instance.IsOrchestrator)
                explanationText.text = "Select at least one situation card to play";
            else
                explanationText.text = $"Wait for {orchestratorName} to choose situation cards";
        }
        public GameCard GetCardById(int id)
            => gamecards[id];

        public GameCard GetSituationCard(Colors color)
        {
            int situationId = (int)color + 1;
            return gamecards[situationId];
        }

        public void Click(GameCard card)
        {
            bool add = !chosen.Contains(card);
            if (add) chosen.Add(card);
            else chosen.Remove(card);
            if (chosen.Count > MaxCards)
            {
                // Remove oldest
                GameCard old = chosen[0];
                old.Animator.SetBool("selected", false);
                chosen.RemoveAt(0);
            }
            
            card.Animator.SetBool("selected", add);
            confirmButton.interactable = chosen.Count > 0;
        }
        public void Confirm()
        {
            GameCard card = chosen[0];
            NetworkData.PlayerInput input = new()
            {
                player_id = NetworkData.Instance.Me.unique_id,
                game_id = (int)GameStateSynchronizer.Instance.LobbyId,
                input_type = NetworkData.PlayerInputType.AssignSituationCard.ToString(),
                related_role = NetworkData.InGameID.Orchestrator.ToString(),
                situation_card = card.Source
            };
            RestAPI.Instance.SendPlayerInput(
                (success) => { Debug.Log("success"); },
                (failure) => { Debug.Log("failure: "+failure); },
                input
            );
        }
        public void MoveCardIn(int id)
        {
            Debug.Log(3333);
            ResetCurrentCard();
            GameCard gc = GetCardById(id);
            if (gc != null)
            {
                gc.moveTo(spawnPoint.GetPos());
            }
            currentGameCard = gc;
        }

        public void ResetCurrentCard()
        {
            if (currentGameCard != null)
            {
                currentGameCard.moveTo(resetPoint.GetPos());
            }
        }
    }
}
