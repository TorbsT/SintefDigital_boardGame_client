using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace View
{
    public class GameCardController : MonoBehaviour
    {

        public GameCard[] gamecards;
        public Point resetPoint;
        public Point spawnPoint;
        private GameCard currentGameCard;
        void Start()
        {
            Debug.Log("Controller started");
            foreach (GameCard gc in gamecards)
            {
                gc.moveTo(resetPoint.GetPos());
            }
        }
        public GameCard GetCardById(int id)
        {
            foreach (GameCard gamecard in gamecards)
            {
                if (gamecard.GetId() == id)
                {
                    return gamecard;
                }
            }
            return null;
        }

        public GameCard GetSituationCard(Colors color)
        {
            int situationId = (int)color + 1;
            return gamecards[situationId];
        }

        public void MoveCardIn(int id)
        {
            if (currentGameCard != null)
            {
                currentGameCard.moveTo(resetPoint.GetPos());
            }
            GameCard gc = GetCardById(id);
            if (gc != null)
            {
                gc.moveTo(spawnPoint.GetPos());
            }
            currentGameCard = gc;
        }




    }
}
