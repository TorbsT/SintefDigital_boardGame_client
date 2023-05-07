using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Network;

public class TrainTransform : MonoBehaviour
{
    private bool should_be_train = false;

    private void Awake()
    {
        if (NetworkData.Instance.Me.Value.is_train)
        {
            SetButtonSpriteToTurnBackToVehicle();
        } else
        {
            SetButtonSpriteToTurnToTrain();
        }
    }

    public void ToggleTransform()
    {
        NetworkData.PlayerInput input = new()
        {
            player_id = NetworkData.Instance.UniqueID,
            game_id = GameStateSynchronizer.Instance.LobbyId.Value,
            input_type = NetworkData.PlayerInputType.SetPlayerTrainBool.ToString(),
            related_bool = should_be_train,
        };
        RestAPI.Instance.SendPlayerInput(success =>
        {
            if (should_be_train)
            {
                SetButtonSpriteToTurnBackToVehicle();
            } else
            {
                SetButtonSpriteToTurnToTrain();
            }
            NetworkData.Player player = NetworkData.Instance.Me.Value;
            player.is_train = should_be_train;
            NetworkData.Instance.Me = player;
        }, failure =>
        {
            Debug.Log(failure);
        }, input);
    }

    private void SetButtonSpriteToTurnToTrain()
    {
        this.gameObject.GetComponent<SpriteRenderer>().material.color = Color.green;
    }

    private void SetButtonSpriteToTurnBackToVehicle()
    {
        this.gameObject.GetComponent<SpriteRenderer>().material.color = Color.red;
    }

    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0))
        {
            should_be_train = !NetworkData.Instance.Me.Value.is_train;
            ToggleTransform();
        }
    }
}
