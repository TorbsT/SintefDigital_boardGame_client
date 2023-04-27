using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Network;
using TMPro;

namespace View
{
    public class GameCardController : MonoBehaviour
    {
        public static GameCardController Instance { get; private set; }

        [SerializeField] private TextMeshProUGUI explanationText;
        public Dictionary<int, GameCard> gamecards = new();
        public Point resetPoint;
        public Point spawnPoint;
        private GameCard currentGameCard;
        private List<GameCard> chosen = new();
        private void Awake()
        {
            Instance = this;
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
                        foreach (var traffic in card.costs)
                        {
                            trafficList.Add($"{traffic.Item1}: {traffic.Item2}");
                        }
                        string traffics = string.Join("\n", trafficList);

                        // Write data to card
                        GameCard gamecard = gamecards[id];
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

            card.Animator.SetBool("selected", add);
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
